using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceModel;

namespace Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class WCFService : IWCFService, IDownloadService
    {
        public static List<OnlineUser> onlineUsers = new List<OnlineUser>();
        DataProvider dp = new DataProvider();

        /// <summary>
        /// Метод для покупки товаров
        /// </summary>
        public void BuyProduct(List<int> Cart, int idProfile)
        {
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

                    TDeals deal = dp.FormTDeal(idDeal, idProfile, pr, 1, false);
                    context.TDeals.Add(deal);
                    profile.Money -= pr.RetailPrice * (1 - profile.PersonalDiscount);
                    profile.TotalSpentMoney += pr.RetailPrice * (1 - profile.PersonalDiscount);

                    idDeal++;
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
        public void BuyProductWholesale(List<Tuple<int, int>> Cart, int idProfile)
        {
            using (postgresContext context = new postgresContext())
            {
                //Получаем таблицу TDeals из БД
                List<TDeals> TDeals = context.TDeals.ToList();

                //Находим пользователя совершившего покупку
                TUsers profile = context.TUsers.FirstOrDefault(u => u.Id == idProfile);

                int idDeal = TDeals.Count > 0 ? (TDeals.Max(u => u.Id) + 1) : 1;

                //Записываем все сделки в БД
                foreach (Tuple<int, int> tuple in Cart)
                {
                    TProducts product = context.TProducts.FirstOrDefault(u => u.Id == tuple.Item1);
                    Product pr = new Product { Id = product.Id, WholesalePrice = product.WholesalePrice };

                    TDeals deal = dp.FormTDeal(idDeal, idProfile, pr, tuple.Item2, true);
                    context.TDeals.Add(deal);
                    profile.Money -= pr.WholesalePrice * (1 - profile.PersonalDiscount) * tuple.Item2;
                    profile.TotalSpentMoney += pr.WholesalePrice * (1 - profile.PersonalDiscount) * tuple.Item2;

                    idDeal++;
                }

                //Обновляем данные о пользователе в БД
                context.TUsers.Update(profile);
                //Сохраняем БД
                context.SaveChanges();
            }
        }


        /// <summary>
        /// Метод для получения изображений игры
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<byte[]> GetGameImages(int id)
        {
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
        public void AddModerationProduct(Product product, List<byte[]> Images)
        {
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
        }


        /// <summary>
        /// Метод для удаления пользователя из друзей
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idFriend"></param>
        public void DeleteFriend(int id, int idFriend)
        {
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
        }

        public List<Profile> GetFriendRequests(int id)
        {
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

        public string CheckMail(string Mail)
        {
            string Code = dp.GenRandomString("QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890", 4);
            MailAddress from = new MailAddress("maetsofficial@gmail.com", "maets");
            MailAddress to = new MailAddress(Mail);
            MailMessage message = new MailMessage(from, to);
            message.Subject = "Регистрация на Maets";
            // текст письма
            message.Body = $"Доброго времени суток!\nЕсли вы видите это письмо, значит вам нужно подтвердить свою личность для Maets.\nВаш код подтверждения: {Code}\nЕсли вы не ожидали получить это письмо, то просто игнорируйте его.\nС уважением, команда Maets";
            message.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("maetsofficial@gmail.com", "Zxcv1234zxcv12345"),
                EnableSsl = true
            };
            smtp.Send(message);
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
        public Profile Connect(string Login, string Password)
        {
            using (postgresContext context = new postgresContext())
            {
                //Ищем пользователя, если находим, то подключаем
                TLogin Tlogin = context.TLogin.Include(u => u.IdNavigation).FirstOrDefault(u => u.Login == Login && u.Password == Password);

                if (Tlogin != null)
                {
                    //Формируем профиль
                    Profile profile = dp.FormActiveUser(Tlogin);
                    profile.status = "Online";

                    //Записываем подключеного пользователя
                    OnlineUser ActiveUser = onlineUsers.FirstOrDefault(u => u.UserProfile.ID == profile.ID);
                    if (ActiveUser != null)
                    {
                        ActiveUser.operationContext.GetCallbackChannel<IWCFServiceCalbback>().ConnectionFromAnotherDevice();
                        onlineUsers.Remove(ActiveUser);
                    }

                    ActiveUser = new OnlineUser { UserProfile = profile, operationContext = OperationContext.Current };
                    ActiveUser.sessionID = OperationContext.Current.Channel;
                    ActiveUser.sessionID.Faulted += new EventHandler(ClientFault);

                    onlineUsers.Add(ActiveUser);

                    //Отсылаем уведомления друзьям находящимся в онлайне
                    if (profile.Friends != null)
                    {
                        foreach (Profile friend in profile.Friends)
                        {

                            OnlineUser OnlineFriend = onlineUsers.FirstOrDefault(u => u.UserProfile == friend);
                            if (OnlineFriend != null)
                                ActiveUser.operationContext.GetCallbackChannel<IWCFServiceCalbback>().FriendOnline(profile.ID);
                        }
                    }

                    //Сохраняем изменения в БД
                    context.SaveChanges();

                    //Выводим сообщение в серверную консоль
                    Console.WriteLine($"{DateTime.Now.ToShortDateString()}, {DateTime.Now.ToShortTimeString()}: {Tlogin.Login} connect to server with channel {OperationContext.Current.SessionId}");

                    return profile;
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
            using (postgresContext context = new postgresContext())
            {
                //Обновляем файл с запросами в друзьями у одного аккаунта
                string path = $@"{BaseSettings.Default.SourcePath}\Users\{id}\FriendRequests.json";
                List<int> friendsReq = File.Exists(path) ? JsonConvert.DeserializeObject<List<int>>(File.ReadAllText(path)) : new List<int>();
                friendsReq.Remove(idRequest);
                File.WriteAllText(path, JsonConvert.SerializeObject(friendsReq));
            }
        }

        public List<Profile> GetProfileByFilter(string filter)
        {
            using (postgresContext context = new postgresContext())
            {
                //Получаем всех пользователей из БД
                List<TLogin> AllUsers = context.TLogin.Include(u => u.IdNavigation).Where(u => u.Login.Contains(filter)).ToList();

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
        /// Метод отправляет сообщение ползователю
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
        public Stream DownloadProduct(int idProduct, int idUser, string path)
        {

            string pathToJson = $@"{BaseSettings.Default.SourcePath}\Users\{idUser}\GamesPath.json";
            List<Tuple<int, string>> GamesPath = File.Exists(pathToJson) ? JsonConvert.DeserializeObject<List<Tuple<int, string>>>(File.ReadAllText(pathToJson)) : new List<Tuple<int, string>>();
            GamesPath.Add(Tuple.Create(idProduct, path));
            File.WriteAllText(pathToJson, JsonConvert.SerializeObject(GamesPath));

            string pathToGame = $@"C:\Users\snayp\Documents\GitHub\DownloadGame\{idProduct}\Game.zip";
            FileStream gameFile = File.OpenRead(pathToGame);
            return gameFile;
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

                //Удаляем пользователя из списка активных пользователей
                onlineUsers.Remove(User);
            }
        }

        /// <summary>
        /// Метод для получения списка всех пользователей
        /// </summary>
        public List<Profile> GetAllUsers()
        {
            using (postgresContext context = new postgresContext())
            {
                //Получаем всех пользователей из БД
                List<TLogin> AllUsers = context.TLogin.Include(u => u.IdNavigation).ToList();

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
            using (postgresContext context = new postgresContext())
            {
                //Ищем пользователя в БД
                TLogin Tlogin = context.TLogin.Include(u => u.IdNavigation).FirstOrDefault(u => u.Id == idUser);

                //Формируем профиль и возвращаем его
                Profile profile = dp.FormActiveUser(Tlogin);
                return profile;
            }
        }


        /// <summary>
        /// Метод для смены пароля
        /// </summary>
        /// <param name="idUser">ID пользователя</param>
        /// <param name="password">пароль в хэшшированом виде</param>
        public void changePassword(int idUser, string password)
        {
            using (postgresContext context = new postgresContext())
            {
                //Ищем пользователя в БД
                TLogin login = context.TLogin.FirstOrDefault(u => u.Id == idUser);

                //Меняем пароль
                login.Password = password;

                //Добавляем изменения в БД
                context.TLogin.Update(login);

                //Сохраняем изменения в БД
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Метод для смены Логина, пароля, почты, и телефона
        /// </summary>
        /// <param name="profile">Передавать только те параметры которые надо изменить</param>
        public void ChangeProfileInforamtion(Profile profile)
        {
            using (postgresContext context = new postgresContext())
            {
                //Ищем пользователя в БД
                TLogin login = context.TLogin.Include(u => u.IdNavigation).FirstOrDefault(u => u.Id == profile.ID);

                //Обновляем только те данные которые необходимо обновить
                if (profile.Login != null)
                    login.Login = profile.Login;
                if (profile.Name != null)
                    login.IdNavigation.Name = profile.Name;
                if (profile.Mail != null)
                    login.IdNavigation.Mail = profile.Mail;
                if (profile.Telephone != null)
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
            string path = $@"{BaseSettings.Default.SourcePath}\Users\{id}\Messages.json";

            //Десериализуем файл, если файла нет то создаём пустой список
            List<UserMessage> newMessages;
            if (File.Exists(path))
                newMessages = JsonConvert.DeserializeObject<List<UserMessage>>(File.ReadAllText(path)).Where(u => u.isRead == false).ToList();
            else
                newMessages = new List<UserMessage>();
            return newMessages;
        }


        /// <summary>
        /// Метод возвращает все игры пользователя
        /// </summary>
        public List<Product> GetUserGames(int id)
        {
            List<Product> games = dp.GetUserGames(id);
            return games;
        }


        /// <summary>
        /// Метод для добавления нового комментария
        /// </summary>
        public void AddComment(int idUser, int idGame, string Comment, int Score)
        {
            using (postgresContext context = new postgresContext())
            {
                //Вычисляем новое значение id
                List<TComments> comments = context.TComments.ToList();
                int id = comments.Count > 0 ? (context.TComments.Max(u => u.Id) + 1) : 1;

                //Формируем новый комментарий
                TComments comment = new TComments();
                comment.Id = id;
                comment.IdProduct = idGame;
                comment.IdUser = idUser;
                comment.Comment = Comment;
                comment.Score = Score;

                //Добавляем комментарий в БД
                context.TComments.Add(comment);

                //Сохранием изменения в БД
                context.SaveChanges();
            }

            //Высчитываем новый рейтинг для игры
            dp.CalculateGameScore(idGame);
        }


        /// <summary>
        /// Метод для удаления существующего комментария
        /// </summary>
        public void DeleteComment(int idComment)
        {
            using (postgresContext context = new postgresContext())
            {
                TComments comment = context.TComments.FirstOrDefault(u => u.Id == idComment);
                context.TComments.Remove(comment);
                context.SaveChanges();
            }
        }


        /// <summary>
        /// Метод возвращает все комментарии кокретной игры
        /// </summary>
        /// <param name="idGame"></param>
        /// <returns></returns>
        public List<Comment> GetAllGameComments(int idGame)
        {
            using (postgresContext context = new postgresContext())
            {
                //Находим и возвращаем список комментариев для определённой игры
                List<TComments> Tcomments = context.TComments.Where(u => u.IdProduct == idGame).ToList();
                List<Comment> comments = new List<Comment>();
                foreach (TComments Tcomment in Tcomments)
                {
                    Comment comment = dp.FormComment(Tcomment);
                    comments.Add(comment);
                }
                return comments;
            }
        }

        public Profile GetEasyProfile(int id)
        {
            using (postgresContext context = new postgresContext())
            {
                List<TLogin> TLogins = context.TLogin.ToList();
                Profile pr = dp.SimpleFormProfile(TLogins.FirstOrDefault(u => u.Id == id));
                return pr;
            }
        }

        public string GetWayToGame(int idUser, int idGame)
        {
            string pathToJson = $@"{BaseSettings.Default.SourcePath}\Users\{idUser}\GamesPath.json";
            List<Tuple<int, string>> messages = File.Exists(pathToJson) ? JsonConvert.DeserializeObject<List<Tuple<int, string>>>(File.ReadAllText(pathToJson)) : new List<Tuple<int, string>>();
            foreach (Tuple<int, string> tuple in messages)
            {
                if (tuple.Item1 == idGame)
                {
                    return tuple.Item2;
                }
            }
            return null;
        }

        public void ChangeModerationStatus(int idUser, int idModerationProduct, bool result)
        {
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
                    foreach(TModerateEmployers moderate in employers)
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
                        List<int> idModerators = context.TUsers.Where(u=>u.AccessRight == 3).Select(u => u.Id).ToList();
                        foreach (TModerateEmployers moderate in employers)
                            idModerators.Remove(moderate.IdEmployee);
                        Random r = new Random();

                        TModerateEmployers newModerator = new TModerateEmployers();
                        newModerator.IdEmployee = idModerators[r.Next(0,idModerators.Count)];
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
    }
}