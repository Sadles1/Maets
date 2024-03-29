﻿using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceModel;
using System.Threading;

namespace Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class WCFService : IWCFService, IDownloadService
    {
        public static List<OnlineUser> onlineUsers = new List<OnlineUser>();
        DataProvider dp = new DataProvider();
        Logs log = new Logs();

        /// <summary>
        /// Метод для покупки товара
        /// </summary>
        public void BuyProduct(List<int> Cart, int idProfile)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            using (postgresContext context = new postgresContext())
            {
                //Получаем таблицу TDeals из БД
                List<TDeals> TDeals = context.TDeals.ToList();

                //Находим пользователя совершившего покупку
                TUsers profile = context.TUsers.FirstOrDefault(u => u.Id == idProfile);

                //Определяем id
                int idDeal = TDeals.Count > 0 ? (TDeals.Max(u => u.Id) + 1) : 1;

                //Записываем все сделки в БД
                foreach (int id in Cart)
                {
                    TProducts product = context.TProducts.FirstOrDefault(u => u.Id == id);
                    Product pr = new Product { Id = product.Id, RetailPrice = product.RetailPrice };

                    context.TUsersGames.Add(new TUsersGames { Idproduct = product.Id, Iduser = idProfile });

                    TDeals deal = dp.FormTDeal(idDeal, idProfile, pr, 1, false);
                    context.TDeals.Add(deal);
                    profile.Money -= pr.RetailPrice * (1-(double)(profile.PersonalDiscount / 100));
                    profile.TotalSpentMoney += pr.RetailPrice * (1 - (double)(profile.PersonalDiscount / 100));

                    idDeal++;
                }

                if (profile.TotalSpentMoney >= 1500 && profile.TotalSpentMoney < 3000)
                {
                    profile.PersonalDiscount = 3;
                }
                else if (profile.TotalSpentMoney >= 3000 && profile.TotalSpentMoney < 6000)
                {
                    profile.PersonalDiscount = 5;
                }
                else if (profile.TotalSpentMoney >= 15000)
                {
                    profile.PersonalDiscount = 10;
                }



                //Обновляем данные о пользователе в БД
                context.TUsers.Update(profile);
                //Сохраняем БД
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Метод для оптовой покупки товаров
        /// </summary>
        /// <param name="Cart">Необходимо передать пары Product + количество</param>
        public List<Product> BuyProductWholesale(List<Tuple<int, int>> Cart, int idProfile)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            using (postgresContext context = new postgresContext())
            {
                //Получаем таблицу TDeals из БД
                List<TDeals> TDeals = context.TDeals.ToList();

                //Находим пользователя совершившего покупку
                TUsers profile = context.TUsers.FirstOrDefault(u => u.Id == idProfile);

                int idDeal = TDeals.Count > 0 ? (TDeals.Max(u => u.Id) + 1) : 1;
                List<Product> ProductAbsent = new List<Product>();
                List<TProductKeys> soldKeys = new List<TProductKeys>();
                string mailMessage = "<p>Доброго времени суток, благодарим <strong>Вас</strong> за покупку игр на Maets</p><h4>Купленные вами игры:</h4>";
                //Записываем все сделки в БД
                foreach (Tuple<int, int> tuple in Cart)
                {

                    TProducts product = context.TProducts.FirstOrDefault(u => u.Id == tuple.Item1);
                    if (product.Quantity >= tuple.Item2)
                    {
                        mailMessage += $"<p><center><strong>{product.Name}</strong></center></p>";
                        Product pr = new Product { Id = product.Id, WholesalePrice = product.WholesalePrice };

                        List<TProductKeys> productKeys = context.TProductKeys.Where(u => u.IdProduct == product.Id && u.IsSold == false).ToList();
                        for (int i = 0; i < tuple.Item2; i++)
                        {
                            soldKeys.Add(productKeys[i]);
                            productKeys[i].IsSold = true;
                            context.TProductKeys.Update(productKeys[i]);
                            mailMessage += $"<p><center>{Convert.ToString(productKeys[i].Key)}</center></p>";
                        }
                        product.Quantity -= tuple.Item2;
                        context.TProducts.Update(product);

                        TDeals deal = dp.FormTDeal(idDeal, idProfile, pr, tuple.Item2, true);
                        context.TDeals.Add(deal);
                        profile.Money -= pr.WholesalePrice * (1 - (double)(profile.PersonalDiscount/100)) * tuple.Item2;
                        profile.TotalSpentMoney += pr.WholesalePrice * (1 - (double)(profile.PersonalDiscount / 100)) * tuple.Item2;
                        mailMessage += $"<br></br>";
                        idDeal++;
                    }
                    else
                        ProductAbsent.Add(new Product { Id = product.Id, Name = product.Name });
                }

                Mail message = new Mail(profile.Mail);
                message.Head = "Покупка на Maets";
                message.Body = mailMessage + "<p>С уважением, команда <strong>Maets</strong></p>";
                message.SendMsg();

                if (profile.TotalSpentMoney >= 1500 && profile.TotalSpentMoney < 3000)
                {
                    profile.PersonalDiscount = 3;
                }
                else if (profile.TotalSpentMoney >= 3000 && profile.TotalSpentMoney < 6000)
                {
                    profile.PersonalDiscount = 5;
                }
                else if (profile.TotalSpentMoney >= 15000)
                {
                    profile.PersonalDiscount = 10;
                }


                //Обновляем данные о пользователе в БД
                context.TUsers.Update(profile);
                //Сохраняем БД
                context.SaveChanges();

                return ProductAbsent;
            }
        }


        /// <summary>
        /// Метод для получения изображений игры
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<byte[]> GetGameImages(int id)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            using (postgresContext context = new postgresContext())
            {
                List<byte[]> Images = new List<byte[]>();
                DirectoryInfo dir = new DirectoryInfo($@"{BaseSettings.Default.SourcePath}\Products\{id}\Images");
                foreach (FileInfo file in dir.EnumerateFiles("*.encr"))
                {
                    using (FileStream fstream = file.OpenRead())
                    {
                        byte[] image = new byte[fstream.Length];
                        fstream.Read(image, 0, image.Length);
                        Images.Add(image);
                    }
                }
                return Images;
            }
        }

        /// <summary>
        /// Метод для получения всех продуктов магазина
        /// </summary>
        public List<Product> GetProductTable()
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            using (postgresContext context = new postgresContext())
            {
                //Получаем таблицу TProducts из бд
                List<TProducts> TProducts = context.TProducts.Include(u => u.IdPublisherNavigation).Include(u => u.IdDeveloperNavigation).ToList();

                //Создаём общий список товаров
                List<Product> products = new List<Product>();

                //Формируем каждый продукт и добавляем его в общий список
                foreach (TProducts TProduct in TProducts)
                {
                    var product = dp.FormProduct(TProduct);
                    products.Add(product);
                }

                //возвращает общий список товаров
                return products;
            }
        }

        /// <summary>
        /// Метод для добавления продукта на модерацию
        /// </summary>
        /// <param name="product"></param>
        public void AddModerationProduct(string mail, Product product, List<byte[]> Images)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            using (postgresContext context = new postgresContext())
            {
                TModerateProducts TModerateProduct = new TModerateProducts();
                List<TModerateProducts> moderateProducts = context.TModerateProducts.ToList();

                //Формируем продукт
                TModerateProduct.Id = moderateProducts.Count > 0 ? (moderateProducts.Max(u => u.Id) + 1) : 1;
                TModerateProduct.Name = product.Name;
                TModerateProduct.Description = product.Description;
                TModerateProduct.RequestDate = DateTime.Now.Date;
                TModerateProduct.RetailPrice = product.RetailPrice;
                TModerateProduct.WholesalePrice = product.WholesalePrice;
                TModerateProduct.ResultModeration = null;

                //Если такого же разработчика нету в БД, то добавляем
                List<TDeveloper> TDevelopers = context.TDeveloper.ToList();
                TDeveloper developer = TDevelopers.FirstOrDefault(u => u.Name == product.Developer);
                if (developer == null)
                {
                    int ID = TDevelopers.Count == 0 ? 1 : (TDevelopers.Max(u => u.Id) + 1);
                    TModerateProduct.IdDeveloper = ID;
                    context.TDeveloper.Add(new TDeveloper { Id = ID, Name = product.Developer });
                    context.SaveChanges();
                }
                else
                    TModerateProduct.IdDeveloper = developer.Id;

                //Если такого же издателя нету в БД, то добавляем
                List<TPublisher> TPublishers = context.TPublisher.ToList();
                TPublisher publisher = TPublishers.FirstOrDefault(u => u.Name == product.Publisher);
                if (publisher == null)
                {
                    int ID = TPublishers.Count == 0 ? 1 : (TPublishers.Max(u => u.Id) + 1);
                    TModerateProduct.IdPublisher = ID;
                    context.TPublisher.Add(new TPublisher { Id = ID, Name = product.Publisher });
                    context.SaveChanges();
                }
                else
                    TModerateProduct.IdPublisher = publisher.Id;
                //Указываем путь
                string path = $@"{BaseSettings.Default.SourcePath}\ModerateProducts\{TModerateProduct.Id}";
                DirectoryInfo dirInfo = new DirectoryInfo($@"{path}\Images");
                dirInfo.Create();

                //записываем основное изображение
                File.WriteAllBytes($@"{path}\MainImage.encr", product.MainImage);

                //Записываем все остальные изображения игры
                int i = 1;
                foreach (byte[] image in Images)
                {
                    File.WriteAllBytes($@"{path}\Images\{i}.encr", image);
                    i++;
                }

                //Выбираем двух случаных модераторов для модерирования
                List<int> Moderators = context.TUsers.Where(u => u.AccessRight == 3).Select(u => u.Id).ToList();//Список всех id модераторов

                Random rnd = new Random();
                int rnd1 = rnd.Next(0, Moderators.Count);
                int rnd2 = rnd.Next(0, Moderators.Count);
                while (rnd2 == rnd1)
                    rnd2 = rnd.Next(0, Moderators.Count);
                int moderator1 = Moderators[rnd1];
                int moderator2 = Moderators[rnd2];

                TModerateEmployers employer;
                employer = new TModerateEmployers { IdEmployee = moderator1, IdModerateProduct = product.Id, Result = null };
                TModerateProduct.TModerateEmployers.Add(employer);
                context.TModerateEmployers.Add(employer);

                employer = new TModerateEmployers { IdEmployee = moderator2, IdModerateProduct = product.Id, Result = null };
                TModerateProduct.TModerateEmployers.Add(employer);
                context.TModerateEmployers.Add(employer);


                Mail message = new Mail(mail);
                message.Head = "Добавления товара на Maets";
                message.Body = $"<html><head></head><body><p><center> Доброго времени суток!</center></p><p> Ваш товар был добавлен на проверку. Наши модераторы проверят его и если нарушений не будет, то мы добавим ваш товар в магазин</p><p> Ваш код подтверждения: </p><p></p><p> Если вы не ожидали получить это письмо, то просто игнорируйте его.</p><p></p><p> С уважением, команда Maets</p><p></p></body></html>";


                context.TModerateProducts.Add(TModerateProduct);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Метод для получения сообщений с конкретным пользователем
        /// </summary>
        /// <param name="idMain">ID основного пользователя</param>
        /// <param name="idComrade">ID пользователя с которым нужно получить сообщения</param>
        public List<UserMessage> GetChat(int idMain, int idComrade)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            //Прописываем путь
            string path = $@"{BaseSettings.Default.SourcePath}\Users\{idMain}\Messages.json";

            //Десериализуем файл, если файла нет то создаём пустой список
            List<UserMessage> messages = File.Exists(path) ? JsonConvert.DeserializeObject<List<UserMessage>>(File.ReadAllText(path)).Where(u => u.IDReceiver == idComrade || u.IDSender == idComrade).ToList() : new List<UserMessage>();
            return messages;
        }

        /// <summary>
        /// Метод для проверки есть ли текущий пользователь в чёрном списке другого пользователя
        /// </summary>
        /// <param name="id">ID нужного профиля</param>
        /// <returns></returns>
        public bool CheckBlacklist(int IdMainUser, int IdSeconUser)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            //Прописываем путь
            string path = $@"{BaseSettings.Default.SourcePath}\Users\{IdSeconUser}\Blacklist.json";

            //Десериализуем файл
            int? Blacklist = File.Exists(path) ? JsonConvert.DeserializeObject<List<int?>>(File.ReadAllText(path)).FirstOrDefault(u => u == IdMainUser) : null;

            //Если он пустой то возвращем false так как мы не в чёрном списке
            if (Blacklist == null)
                return false;
            else
                //Если он не пустой, то ищем пользователя с нужным нам id, если его там нет то возвращаем false если есть true
                return true;
        }

        /// <summary>
        /// Метод для удаления из чёрного списка
        /// </summary>
        /// <param name="id">ID пользователя который добавил в чёрный список</param>
        /// <param name="idUserInBlacklist">ID пользователя которого необходимо удалить из чёрного списока</param>
        public void RemoveFromBlacklist(int id, int idUserInBlacklist)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            //Прописываем путь
            string path = $@"{BaseSettings.Default.SourcePath}\Users\{id}\Blacklist.json";

            //Десериализуем файл
            List<int> Blacklist = JsonConvert.DeserializeObject<List<int>>(File.ReadAllText(path));

            //Удаляем из списка user которого надо убрать из чёрного списка
            Blacklist.Remove(idUserInBlacklist);

            //Сериализуем новый список в файл
            File.WriteAllText(path, JsonConvert.SerializeObject(Blacklist));
        }

        /// <summary>
        /// Метод для добавления в чёрный список
        /// </summary>
        /// <param name="id">ID пользователя который добавил в чёрный список</param>
        /// <param name="idUserToBlacklist">ID пользователя которого добавили в чёрный список</param>
        public void AddToBlacklist(int id, int idUserToBlacklist)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            //Указываем путь
            string path = $@"{BaseSettings.Default.SourcePath}\Users\{id}\Blacklist.json";

            //Если файл существует десериализуем его, если нет то создаём новый список
            List<int> Blacklist = File.Exists(path) ? JsonConvert.DeserializeObject<List<int>>(File.ReadAllText(path)) : new List<int>();

            //Добавляем к текущему списку нового пользователя
            Blacklist.Add(idUserToBlacklist);

            //Сериализуем список
            File.WriteAllText(path, JsonConvert.SerializeObject(Blacklist));
        }

        /// <summary>
        /// Метод для добавления в список друзей
        /// </summary>
        public void AddFriend(int id, int idFriend)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            //Обновляем файл с друзьями у одного аккаунта
            string path = $@"{BaseSettings.Default.SourcePath}\Users\{id}\Friends.json";
            List<int> friends = File.Exists(path) ? JsonConvert.DeserializeObject<List<int>>(File.ReadAllText(path)) : new List<int>();
            friends.Add(idFriend);
            File.WriteAllText(path, JsonConvert.SerializeObject(friends));


            //Обновляем файл с друзьями у другого аккаунта
            path = $@"{BaseSettings.Default.SourcePath}\Users\{idFriend}\Friends.json";
            friends = File.Exists(path) ? JsonConvert.DeserializeObject<List<int>>(File.ReadAllText(path)) : new List<int>();
            friends.Add(id);
            File.WriteAllText(path, JsonConvert.SerializeObject(friends));

            OnlineUser OnlineFriend = onlineUsers.FirstOrDefault(u => u.UserProfile.ID == idFriend);
            if (OnlineFriend != null)
            {
                Profile pr;
                using (postgresContext context = new postgresContext())
                {
                    TLogin login = context.TLogin.FirstOrDefault(u => u.Id == id);
                    pr = dp.SimpleFormProfile(login);
                }
                OnlineFriend.operationContext.GetCallbackChannel<IWCFServiceCalbback>().AcceptFriendRequest(pr);
            }
        }

        /// <summary>
        /// Метод для удаления пользователя из друзей
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idFriend"></param>
        public void DeleteFriend(int id, int idFriend)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            //Обновляем файл с друзьями у одного аккаунта
            string path = $@"{BaseSettings.Default.SourcePath}\Users\{id}\Friends.json";
            List<int> friends = File.Exists(path) ? JsonConvert.DeserializeObject<List<int>>(File.ReadAllText(path)) : new List<int>();
            friends.Remove(idFriend);
            File.WriteAllText(path, JsonConvert.SerializeObject(friends));


            //Обновляем файл с друзьями у другого аккаунта
            path = $@"{BaseSettings.Default.SourcePath}\Users\{idFriend}\Friends.json";
            friends = File.Exists(path) ? JsonConvert.DeserializeObject<List<int>>(File.ReadAllText(path)) : new List<int>();
            friends.Remove(id);
            File.WriteAllText(path, JsonConvert.SerializeObject(friends));

            OnlineUser OnlineFriend = onlineUsers.FirstOrDefault(u => u.UserProfile.ID == idFriend);
            if (OnlineFriend != null)
            {
                OnlineFriend.operationContext.GetCallbackChannel<IWCFServiceCalbback>().DeleteFromFriend(id);
            }
        }


        /// <summary>
        /// Метод возвращает список всех запросов в друзья конкретного пользователя
        /// </summary>
        public List<Profile> GetFriendRequests(int id)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            using (postgresContext context = new postgresContext())
            {
                string path = $@"{BaseSettings.Default.SourcePath}\Users\{id}\FriendRequests.json";
                List<Profile> FriendRequests = new List<Profile>();
                //Формируем список заявок в друзья
                if (File.Exists(path))
                {
                    List<TLogin> TLogins = context.TLogin.ToList();
                    List<int> IdFriendsReq = File.Exists(path) ? JsonConvert.DeserializeObject<List<int>>(File.ReadAllText(path)) : new List<int>();
                    foreach (int pr in IdFriendsReq)
                    {
                        Profile req = dp.SimpleFormProfile(TLogins.FirstOrDefault(u => u.Id == pr));
                        FriendRequests.Add(req);
                    }
                }
                return FriendRequests;
            }
        }


        /// <summary>
        /// Метод выполняет проверку почты при регистрации
        /// </summary>
        public string CheckMailRegister(string email)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            using (postgresContext context = new postgresContext())
            {
                if (context.TUsers.FirstOrDefault(u => u.Mail == email) != null)
                    throw new FaultException("Данная почта уже зарегистрирована");
            }
            string Code = dp.GenRandomString("QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890", 4);
            Mail message = new Mail(email);
            message.Head = "Регистрация Maets";
            message.Body = $"<html><head></head><body><p><center> Доброго времени суток!</center></p><p> Если вы видите это письмо, значит вам нужно подтвердить свою личность для Maets.</p><p> Ваш код подтверждения: </p><p></p><h2><center>{Code}</center></h2><p> Если вы не ожидали получить это письмо, то просто игнорируйте его.</p><p></p><p> С уважением, команда Maets</p><p></p></body></html>";
            message.SendMsg();
            return Code;
        }

        /// <summary>
        /// Метод выполняет проверку почты при сбросе пароля
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public string CheckMailResetPassword(string email)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            string Code = dp.GenRandomString("QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890", 4);
            Mail message = new Mail(email);
            message.Head = "Смена пароля Maets";
            message.Body = $"<html><head></head><body><p><center> Доброго времени суток!</center></p><p>Если вы видите это письмо, значит происходит смена пароля Вашего аккаунта на Maets.</p><p> Ваш код подтверждения: </p><p></p><h2><center>{Code}</center></h2><p> Если вы не ожидали получить это письмо, то просто игнорируйте его.</p><p></p><p> С уважением, команда Maets</p><p></p></body></html>";
            message.SendMsg();
            return Code;
        }

        /// <summary>
        /// Метод для регистрации пользователя
        /// </summary>
        /// <param name="profile">Регистрируемый профиль. Необходимо передать поля Login и Mail.
        /// по желанию можно передать поля Telephone и Name, либо прировнять их к null</param>
        /// <param name="Password">Пароль</param>
        public void Register(Profile profile, string Password)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            using (postgresContext context = new postgresContext())
            {
                //Создаём таблицы TUser и TLogin
                TUsers TUser = new TUsers();
                TLogin Tlogin = new TLogin();

                //Т.к имя и телефон можно не указывать, необходимо прировнять их к null
                if (profile.Name == "")
                    profile.Name = null;
                if (profile.Telephone == "")
                    profile.Telephone = null;

                //Формируем таблицы TUser и TLogin
                dp.FormTableUser(profile, Password, ref TUser, ref Tlogin);

                //Создаём папку для нового пользователя
                string path = $@"{BaseSettings.Default.SourcePath}\Users";
                DirectoryInfo dirInfo = new DirectoryInfo($@"{path}\{TUser.Id}");
                dirInfo.Create();

                //Копируем Файл со стандартным изображением в папку нового профиля  
                File.Copy($@"{path}\DefaultImage\MainImage.encr", $@"{path}\{TUser.Id}\MainImage.encr", true);

                //Добавляем профиль в БД
                context.TLogin.Add(Tlogin);
                context.TUsers.Add(TUser);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Метод для изменения счёта пользователя
        /// </summary>
        /// <param name="id">ID пользователя</param>
        /// <param name="money">Сумма на которую измениться счёт</param>
        public void AddMoney(int id, int money)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            using (postgresContext context = new postgresContext())
            {
                //Находим пользователя
                TUsers user = context.TUsers.FirstOrDefault(u => u.Id == id);

                //Прибавляем к его текущему счёт дополнительные средства
                user.Money += money;

                //Сохраняем изменение в БД
                context.TUsers.Update(user);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Метод выполняет соединение используя передоваемый логин и пароль
        /// </summary>
        /// <param name="Login">Логин</param>
        /// <param name="Password">Пароль</param>
        /// <returns></returns>
        public void Connect(string Login, string Password)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name,OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            using (postgresContext context = new postgresContext())
            {
                //Ищем пользователя, если находим, то подключаем
                TLogin Tlogin = context.TLogin.FirstOrDefault(u => u.Login == Login && u.Password == Password);

                if (Tlogin != null)
                {
                    //Выводим сообщение в серверную консоль
                    Console.WriteLine($"{DateTime.Now.ToShortDateString()}, {DateTime.Now.ToShortTimeString()}: {Tlogin.Login} connect to server with channel {OperationContext.Current.SessionId}");

                    //Записываем подключеного пользователя
                    OnlineUser ActiveUser = onlineUsers.FirstOrDefault(u => u.UserProfile.ID == Tlogin.Id);
                    if (ActiveUser != null)
                    {
                        ActiveUser.operationContext.GetCallbackChannel<IWCFServiceCalbback>().ConnectionFromAnotherDevice();
                        onlineUsers.Remove(ActiveUser);
                    }

                    string path = $@"{BaseSettings.Default.SourcePath}\Users\{Tlogin.Id}\";

                    List<Profile> Friends = dp.GetProfileFriends(Tlogin.Id);

                    if (Friends != null)
                    {
                        foreach (Profile friend in Friends)
                        {
                            OnlineUser OnlineFriend = onlineUsers.FirstOrDefault(u => u.UserProfile.ID == friend.ID);
                            if (OnlineFriend != null)
                                OnlineFriend.operationContext.GetCallbackChannel<IWCFServiceCalbback>().FriendOnline(Tlogin.Id);
                        }
                    }

                    ActiveUser = new OnlineUser { UserProfile = new Profile { ID = Tlogin.Id, Login = Tlogin.Login }, operationContext = OperationContext.Current };
                    ActiveUser.sessionID = OperationContext.Current.Channel;
                    ActiveUser.sessionID.Faulted += new EventHandler(ClientFault);

                    onlineUsers.Add(ActiveUser);
                }
                else
                    throw new FaultException("Логин или пароль не верны");//Если пользователь не найден возвращаем null
            }
        }

        /// <summary>
        /// Метод чтобы Паша не ронял сервер после каждого входа
        /// </summary>
        private void ClientFault(object sender, EventArgs e)
        {
            //Находим пользователя которого необходимо отключить
            OnlineUser User = onlineUsers.FirstOrDefault(u => u.sessionID == (IClientChannel)sender);

            if (User != null)
            {
                //Получаем id сессии
                string sessionId = User.operationContext.SessionId;
                string Login = User.UserProfile.Login;

                //Выводим сообщение в серверную консоль
                Console.WriteLine($"{DateTime.Now.ToShortDateString()}, {DateTime.Now.ToShortTimeString()}: {Login} with Session Id {sessionId} faulted");

                //Удаляем пользователя из списка активных пользователей
                onlineUsers.Remove(User);
            }
        }

        /// <summary>
        /// Метод удаляет аккаунт
        /// </summary>
        /// <param name="id">ID аккаунта для удаления</param>
        public void DeleteAccount(int id)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            using (postgresContext context = new postgresContext())
            {
                TLogin login = context.TLogin.FirstOrDefault(u => u.Id == id);

                //Удаляем из БД
                context.TLogin.Remove(login);
                context.SaveChanges();

                //Вывод в серверную консоль
                Console.WriteLine($"{DateTime.Now.ToShortDateString()}, {DateTime.Now.ToShortTimeString()}: Account {context.TLogin.FirstOrDefault(u => u.Id == id).Login} deleted");
            }
        }


        /// <summary>
        /// Метод для удаления заявки в друзья
        /// </summary>
        public void DeleteFriendReqest(int id, int idRequest)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            using (postgresContext context = new postgresContext())
            {
                //Обновляем файл с запросами в друзья
                string path = $@"{BaseSettings.Default.SourcePath}\Users\{id}\FriendRequests.json";
                List<int> friendsReq = File.Exists(path) ? JsonConvert.DeserializeObject<List<int>>(File.ReadAllText(path)) : new List<int>();
                friendsReq.Remove(idRequest);
                File.WriteAllText(path, JsonConvert.SerializeObject(friendsReq));
            }
        }


        /// <summary>
        /// Метод возвращает список пользователей используя фильтр
        /// </summary>
        public List<Profile> GetProfileByFilter(string filter)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            using (postgresContext context = new postgresContext())
            {
                //Получаем всех пользователей из БД
                List<TLogin> Tlogin = context.TLogin.Where(u => u.Login.Contains(filter)).ToList();

                //Создаём пустой список в который будем записывать всех пользователей
                List<Profile> AllProfile = new List<Profile>();

                //Для каждого пользователя формируем профиль
                foreach (TLogin user in Tlogin)
                {
                    Profile pr = dp.SimpleFormProfile(user);

                    //Добавляем сформированный профиль в общий список
                    AllProfile.Add(pr);
                }
                return AllProfile;
            }
        }

        public List<Product> GetProductByFilter(string filter)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            using (postgresContext context = new postgresContext())
            {
                //Получаем всех пользователей из БД
                List<TProducts> TProducts = context.TProducts.Where(u => u.Name.Contains(filter) || u.Description.Contains(filter)).ToList();

                //Создаём пустой список в который будем записывать всех пользователей
                List<Product> AllProducts = new List<Product>();

                //Для каждого пользователя формируем профиль
                foreach (TProducts product in TProducts)
                {
                    Product pr = new Product { Id = product.Id, Name = product.Name, Description = product.Description };

                    using (FileStream fstream = File.OpenRead($@"{BaseSettings.Default.SourcePath}\Products\{product.Id}\MainImage.encr"))
                    {
                        pr.MainImage = new byte[fstream.Length];
                        fstream.Read(pr.MainImage, 0, pr.MainImage.Length);
                    }

                    //Добавляем сформированный профиль в общий список
                    AllProducts.Add(pr);
                }
                return AllProducts;
            }
        }

        /// <summary>
        /// Метод отправляет сообщение пользователю
        /// </summary>
        /// <param name="msg">
        /// Данные о сообщении.
        /// Необходимо заполнить поля: 
        /// IDSender - ID отправителя,
        /// IDReceiver - ID получателя,
        /// message - передаваемое сообещение
        /// </param>
        public void SendMsg(UserMessage msg)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            msg.date = DateTime.Now;
            msg.isRead = false;

            //Добавляем сообщения для одного пользователя
            string path = $@"{BaseSettings.Default.SourcePath}\Users\{msg.IDSender}\Messages.json";
            List<UserMessage> messages = File.Exists(path) ? JsonConvert.DeserializeObject<List<UserMessage>>(File.ReadAllText(path)) : new List<UserMessage>();
            messages.Add(msg);
            File.WriteAllText(path, JsonConvert.SerializeObject(messages));

            //Добавляем сообщения для второго пользователя
            path = $@"{BaseSettings.Default.SourcePath}\Users\{msg.IDReceiver}\Messages.json";
            messages = File.Exists(path) ? JsonConvert.DeserializeObject<List<UserMessage>>(File.ReadAllText(path)) : new List<UserMessage>();
            messages.Add(msg);
            File.WriteAllText(path, JsonConvert.SerializeObject(messages));

            //Если пользователь в сети, то нужно вызвать callback для отображения сообщения
            OnlineUser ReceiverUser = onlineUsers.FirstOrDefault(u => u.UserProfile.ID == msg.IDReceiver);
            if (ReceiverUser != null)
                ReceiverUser.operationContext.GetCallbackChannel<IWCFServiceCalbback>().GetMessage(msg);
        }

        /// <summary>
        /// Метод для загрузки продукта
        /// </summary>
        public Stream DownloadProduct(int idProduct, int idUser, long startPoint)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);

            string pathToGame = $@"C:\Users\snayp\Documents\GitHub\DownloadGame\{idProduct}\Game.zip";
            FileStream gameFile = new FileStream(pathToGame, FileMode.Open, FileAccess.Read);

            if (startPoint != 0)
                gameFile.Position = startPoint;

            return gameFile;
        }

        public long GetFileSize(int idProduct)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            string pathToGame = $@"C:\Users\snayp\Documents\GitHub\DownloadGame\{idProduct}\Game.zip";
            FileInfo file = new FileInfo(pathToGame);
            return file.Length;
        }

        /// <summary>
        /// Вызываеться для отключения от сервера
        /// </summary>
        /// <param name="Id"></param>
        public void Disconnect(int Id)
        {           
            //Находим пользователя которого необходимо отключить
            OnlineUser User = onlineUsers.FirstOrDefault(u => u.UserProfile.ID == Id);
            if (User != null)
            {
                //Получаем id сессии
                string sessionId = (OperationContext.Current.SessionId);
                string Login = User.UserProfile.Login;
                //Выводим сообщение в серверную консоль
                Console.WriteLine($"{DateTime.Now.ToShortDateString()}, {DateTime.Now.ToShortTimeString()}: {Login} with Session Id {sessionId} disconnect");


                string path = $@"{BaseSettings.Default.SourcePath}\Users\{Id}\";
                //List<TLogin> Tlogins = context.TLogin.ToList();
                if (File.Exists($@"{path}Friends.json"))
                {
                    List<int> IdFriends = JsonConvert.DeserializeObject<List<int>>(File.ReadAllText($@"{path}Friends.json"));
                    foreach (int id in IdFriends)
                    {
                        OnlineUser OnlineFriend = onlineUsers.FirstOrDefault(u => u.UserProfile.ID == id);
                        if (OnlineFriend != null)
                            OnlineFriend.operationContext.GetCallbackChannel<IWCFServiceCalbback>().FriendOffline(Id);
                    }
                }

                //Удаляем пользователя из списка активных пользователей
                onlineUsers.Remove(User);
            }
        }

        /// <summary>
        /// Метод для получения списка всех пользователей
        /// </summary>
        public List<Profile> GetAllUsers()
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            using (postgresContext context = new postgresContext())
            {
                //Получаем всех пользователей из БД
                List<TLogin> AllUsers = context.TLogin.ToList();

                //Создаём пустой список в который будем записывать всех пользователей
                List<Profile> AllProfile = new List<Profile>();

                //Для каждого пользователя формируем профиль
                foreach (TLogin user in AllUsers)
                {
                    Profile pr = dp.SimpleFormProfile(user);

                    //Добавляем сформированный профиль в общий список
                    AllProfile.Add(pr);
                }
                return AllProfile;
            }
        }

        /// <summary>
        /// Метод для сохранение корзины конкретного пользователя на сервере
        /// </summary>
        /// <param name="idUser">ID пользователя</param>
        /// <param name="Cart">Данные корзины</param>
        public void SaveCart(int idUser, List<Product> Cart)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            //Указываем путь
            string path = $@"{BaseSettings.Default.SourcePath}\Users\{idUser}\Cart.json";

            //Сериализуем корзину с товарами
            File.WriteAllText(path, JsonConvert.SerializeObject(Cart));
        }

        /// <summary>
        /// Метод для отправки запроса на дружбу
        /// </summary>
        /// <param name="idSender">Id отправителя</param>
        /// <param name="idReceiver">Id получателя</param>
        public void SendFriendRequest(int idSender, int idReceiver)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            using (postgresContext context = new postgresContext())
            {
                //Обновляем файл с запросами в друзьями у одного аккаунта
                string path = $@"{BaseSettings.Default.SourcePath}\Users\{idReceiver}\FriendRequests.json";
                List<int> friendsReq = File.Exists(path) ? JsonConvert.DeserializeObject<List<int>>(File.ReadAllText(path)) : new List<int>();
                friendsReq.Add(idSender);
                File.WriteAllText(path, JsonConvert.SerializeObject(friendsReq));

                TLogin login = context.TLogin.FirstOrDefault(u => u.Id == idSender);
                Profile profile = dp.SimpleFormProfile(login);

                //Если пользователь в сети, то нужно вызвать callback для отображения запроса
                OnlineUser ReceiverUser = onlineUsers.FirstOrDefault(u => u.UserProfile.ID == idReceiver);
                if (ReceiverUser != null)
                    ReceiverUser.operationContext.GetCallbackChannel<IWCFServiceCalbback>().GetFriendRequest(profile);
            }
        }

        /// <summary>
        /// Метод для заполнения чужого профиля
        /// </summary>
        public Profile CheckProfile(int idUser)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            using (postgresContext context = new postgresContext())
            {
                //Ищем пользователя в БД
                TLogin Tlogin = context.TLogin.Include(u => u.IdNavigation).FirstOrDefault(u => u.Id == idUser);

                //Формируем профиль и возвращаем его
                Profile profile = dp.FormProfile(Tlogin);
                return profile;
            }
        }

        /// <summary>
        /// Метод для заполнения текущего профиля
        /// </summary>
        public Profile CheckActiveProfile(int idUser)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            using (postgresContext context = new postgresContext())
            {
                //Ищем пользователя в БД
                TLogin Tlogin = context.TLogin.Include(u => u.IdNavigation).FirstOrDefault(u => u.Id == idUser);

                //Формируем профиль и возвращаем его
                Profile profile = dp.FormActiveUser(Tlogin);
                return profile;
            }
        }

        public void changeProfileImage(int idUser, byte[] MainImage)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            File.WriteAllBytes($@"{BaseSettings.Default.SourcePath}\Users\{idUser}\MainImage.encr", MainImage);
        }

        /// <summary>
        /// Метод для восстановления пароля
        /// </summary>
        /// <param name="idUser">ID пользователя</param>
        /// <param name="password">пароль в хэшшированом виде</param>
        public void resetPassword(int idUser, string newPassword)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            using (postgresContext context = new postgresContext())
            {
                //Ищем пользователя в БД
                TLogin login = context.TLogin.FirstOrDefault(u => u.Id == idUser);

                //Меняем пароль
                login.Password = newPassword;

                //Добавляем изменения в БД
                context.TLogin.Update(login);

                //Сохраняем изменения в БД
                context.SaveChanges();
            }
        }

        public void changePassword(int idUser, string password, string newPassword)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            using (postgresContext context = new postgresContext())
            {
                //Ищем пользователя в БД
                TLogin login = context.TLogin.FirstOrDefault(u => u.Id == idUser && u.Password == password);

                if (login != null)
                {
                    //Меняем пароль
                    login.Password = newPassword;

                    //Добавляем изменения в БД
                    context.TLogin.Update(login);

                    //Сохраняем изменения в БД
                    context.SaveChanges();
                }
                else
                    throw new FaultException("Cтарый пароль введён неверно");
            }
        }

        /// <summary>
        /// Метод для смены Логина, пароля, почты, и телефона
        /// </summary>
        /// <param name="profile">Передавать только те параметры которые надо изменить</param>
        public void ChangeProfileInformation(Profile profile)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            using (postgresContext context = new postgresContext())
            {
                //Ищем пользователя в БД
                TLogin login = context.TLogin.Include(u => u.IdNavigation).FirstOrDefault(u => u.Id == profile.ID);

                dp.CheckUserInfo(profile.Login, profile.Mail, profile.Telephone);

                //Обновляем только те данные которые необходимо обновить
                if (profile.Login != "")
                    login.Login = profile.Login;
                if (profile.Name != "")
                    login.IdNavigation.Name = profile.Name;
                if (profile.Mail != "")
                    login.IdNavigation.Mail = profile.Mail;
                if (profile.Telephone != "")
                    login.IdNavigation.Telephone = profile.Telephone;

                //Добавляем изменения в БД
                context.TLogin.Update(login);

                //Сохраняем изменения в БД
                context.SaveChanges();
            }
        }


        /// <summary>
        /// Метод возвращает все новые сообщения пользователя
        /// </summary>
        public List<UserMessage> GetNewMessages(int id)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            string path = $@"{BaseSettings.Default.SourcePath}\Users\{id}\Messages.json";

            //Десериализуем файл, если файла нет то создаём пустой список
            List<UserMessage> newMessages;
            if (File.Exists(path))
                newMessages = JsonConvert.DeserializeObject<List<UserMessage>>(File.ReadAllText(path)).Where(u => u.isRead == false && u.IDReceiver == id).ToList();
            else
                newMessages = new List<UserMessage>();
            return newMessages;
        }


        /// <summary>
        /// Метод возвращает все игры пользователя
        /// </summary>
        public List<Product> GetUserGames(int id)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            List<Product> games = dp.GetUserGames(id);
            return games;
        }

        /// <summary>
        /// Метод для добавления нового комментария
        /// </summary>
        public void AddComment(Comment comment)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            using (postgresContext context = new postgresContext())
            {
                //Формируем новый комментарий
                TComments Tcomment = new TComments();
                Tcomment.IdProduct = comment.idProduct;
                Tcomment.IdUser = comment.idUser;
                Tcomment.Comment = comment.comment;
                Tcomment.Score = comment.Score;

                //Добавляем комментарий в БД
                context.TComments.Add(Tcomment);

                //Сохранием изменения в БД
                context.SaveChanges();
            }

            //Высчитываем новый рейтинг для игры
            dp.CalculateGameScore(comment.idProduct);
        }


        /// <summary>
        /// Метод для удаления существующего комментария
        /// </summary>
        public void DeleteComment(int idUser, int idProduct)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            using (postgresContext context = new postgresContext())
            {
                TComments comment = context.TComments.FirstOrDefault(u => u.IdUser == idUser && u.IdProduct == idProduct);
                context.TComments.Remove(comment);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Метод возвращает все комментарии кокретной игры
        /// </summary>
        /// <param name="idGame"></param>
        /// <returns></returns>
        public List<Tuple<Comment, Profile>> GetAllGameComments(int idGame)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            using (postgresContext context = new postgresContext())
            {
                //Находим и возвращаем список комментариев для определённой игры
                List<TLogin> logins = context.TLogin.ToList();
                List<TComments> Tcomments = context.TComments.Where(u => u.IdProduct == idGame).ToList();
                List<Tuple<Comment, Profile>> comments = new List<Tuple<Comment, Profile>>();
                foreach (TComments Tcomment in Tcomments)
                {
                    Comment comment = new Comment();
                    comment.idProduct = Tcomment.IdProduct;
                    comment.idUser = Tcomment.IdUser;
                    comment.Score = Tcomment.Score;
                    comment.comment = Tcomment.Comment;

                    Profile pr = dp.SimpleFormProfile(logins.FirstOrDefault(u => u.Id == comment.idUser));

                    comments.Add(Tuple.Create(comment, pr));
                }
                return comments;
            }
        }

        public Profile GetEasyProfile(int id)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            using (postgresContext context = new postgresContext())
            {
                List<TLogin> TLogins = context.TLogin.ToList();
                Profile pr = dp.SimpleFormProfile(TLogins.FirstOrDefault(u => u.Id == id));
                return pr;
            }
        }
        public void ChangeAccessRight(int idUser, int AccessRight)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            using (postgresContext context = new postgresContext())
            {
                TLogin user = context.TLogin.Include(u => u.IdNavigation).FirstOrDefault(u => u.Id == idUser);
                user.IdNavigation.AccessRight = AccessRight;
                Console.WriteLine($"{DateTime.Now.ToShortDateString()}, {DateTime.Now.ToShortTimeString()}: Changed access rights of {user.Login} to {AccessRight} ");
                context.TLogin.Update(user);
                context.SaveChanges();
            }
        }

        public void ChangeModerationStatus(int idUser, int idModerationProduct, bool result)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            using (postgresContext context = new postgresContext())
            {
                List<TModerateEmployers> employers = context.TModerateEmployers.Where(u => u.IdModerateProduct == idModerationProduct).ToList();
                TModerateEmployers employer = employers.FirstOrDefault(u => u.IdEmployee == idUser);
                employer.Result = result;
                context.Update(employer);
                context.SaveChanges();

                int Result = 0;
                if (employers.Count == 2)
                {
                    foreach (TModerateEmployers moderate in employers)
                    {
                        if (moderate.Result == null)
                            return;
                        Result += moderate.Result == true ? 1 : -1;
                    }
                    if (Result == 2)
                    {
                        dp.AddProduct(idModerationProduct);
                        dp.RemoveFromModeration(idModerationProduct);
                    }
                    else if (Result == -2)
                    {
                        dp.RemoveFromModeration(idModerationProduct);
                    }
                    else
                    {
                        List<int> idModerators = context.TUsers.Where(u => u.AccessRight == 3).Select(u => u.Id).ToList();
                        foreach (TModerateEmployers moderate in employers)
                            idModerators.Remove(moderate.IdEmployee);
                        Random r = new Random();

                        TModerateEmployers newModerator = new TModerateEmployers();
                        newModerator.IdEmployee = idModerators[r.Next(0, idModerators.Count)];
                        newModerator.IdModerateProduct = idModerationProduct;

                        context.TModerateEmployers.Add(newModerator);
                        context.SaveChanges();
                    }
                }
                else if (employers.Count == 3)
                {
                    if (result == true)
                    {
                        dp.AddProduct(idModerationProduct);
                        dp.RemoveFromModeration(idModerationProduct);
                    }
                    else
                    {
                        dp.RemoveFromModeration(idModerationProduct);
                    }
                }
            }
        }

        public List<Product> GetModerationProduct(int idUser)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            using (postgresContext context = new postgresContext())
            {
                List<int> idModerateProduct = context.TModerateEmployers.Where(u => u.IdEmployee == idUser && u.Result == null).Select(u => u.IdModerateProduct).ToList();
                List<TModerateProducts> TmoderateProducts = context.TModerateProducts.ToList();
                List<Product> products = new List<Product>();
                foreach (int id in idModerateProduct)
                {
                    TModerateProducts product = TmoderateProducts.FirstOrDefault(u => u.Id == id);
                    Product pr = new Product();
                    pr.Id = product.Id;
                    pr.Name = product.Name;
                    products.Add(pr);
                }
                return products;
            }
        }

        public void SetMessageRead(int id, int idChatedUser)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            string path = $@"{BaseSettings.Default.SourcePath}\Users\{id}\Messages.json";
            List<UserMessage> allMessages = JsonConvert.DeserializeObject<List<UserMessage>>(File.ReadAllText(path)).Where(u => u.IDSender == idChatedUser || u.IDReceiver == id).ToList();
            foreach (UserMessage currentMessage in allMessages)
            {
                if (currentMessage.isRead == false)
                    currentMessage.isRead = true;
            }
            File.WriteAllText(path, JsonConvert.SerializeObject(allMessages));
        }

        public void ActivateLicenseKey(int idUser, string Key)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            using (postgresContext context = new postgresContext())
            {
                TProductKeys key = context.TProductKeys.FirstOrDefault(u => u.Key == Key);
                if (key.IsSold == true && key.IsActivate == false)
                {
                    key.IsActivate = true;
                    context.TProductKeys.Update(key);
                    context.TUsersGames.Add(new TUsersGames { Idproduct = key.IdProduct, Iduser = idUser });
                    context.SaveChanges();
                }
                else if (key.IsActivate == true)
                    throw new FaultException("Ошибка, ключ уже активирован");
                else
                    throw new FaultException("Ошибка, ключ недоступен");
            }
        }

        public Profile ActiveProfile(string Login, string Password)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            using (postgresContext context = new postgresContext())
            {
                //Ищем пользователя, если находим, то подключаем
                TLogin Tlogin = context.TLogin.Include(u => u.IdNavigation).FirstOrDefault(u => u.Login == Login && u.Password == Password);
                if (Tlogin != null)
                {
                    //Формируем профиль
                    Profile profile = dp.FormActiveUser(Tlogin);
                    profile.status = "Online";
                    return profile;
                }
                else
                    return null;

            }
        }
        public void ChangeComment(Comment comment)
        {
            log.WriteLog(new StackTrace(false).GetFrame(0).GetMethod().Name, OperationContext.Current.SessionId, Thread.CurrentThread.ManagedThreadId);
            using (postgresContext context = new postgresContext())
            {
                //Формируем новый комментарий
                TComments Tcomment = context.TComments.FirstOrDefault(u => u.IdUser == comment.idUser && u.IdProduct == comment.idProduct);
                Tcomment.Comment = comment.comment;
                Tcomment.Score = comment.Score;

                //Добавляем комментарий в БД
                context.TComments.Update(Tcomment);

                //Сохранием изменения в БД
                context.SaveChanges();
            }

            //Высчитываем новый рейтинг для игры
            dp.CalculateGameScore(comment.idProduct);
        }
    }
}