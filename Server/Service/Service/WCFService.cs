using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;

namespace Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
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

                //Записываем все сделки в БД
                foreach (int id in Cart)
                {
                    TProducts product = context.TProducts.FirstOrDefault(u => u.Id == id);
                    Product pr = new Product { Id = product.Id, RetailPrice = product.RetailPrice };

                    TDeals deal = dp.FormTDeal(idProfile, pr, 1, false);
                    context.TDeals.Add(deal);
                    profile.Money -= pr.RetailPrice * (1 - profile.PersonalDiscount);
                    profile.TotalSpentMoney += pr.RetailPrice * (1 - profile.PersonalDiscount);
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

                //Записываем все сделки в БД
                foreach (Tuple<int, int> tuple in Cart)
                {
                    TProducts product = context.TProducts.FirstOrDefault(u => u.Id == tuple.Item1);
                    Product pr = new Product { Id = product.Id, WholesalePrice = product.WholesalePrice };

                    TDeals deal = dp.FormTDeal(idProfile, pr, tuple.Item2, true);
                    context.TDeals.Add(deal);
                    profile.Money -= pr.WholesalePrice * (1 - profile.PersonalDiscount) * tuple.Item2;
                    profile.TotalSpentMoney += pr.WholesalePrice * (1 - profile.PersonalDiscount) * tuple.Item2;
                }

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
        public void AddModerationProduct(Product product)
        {
            using (postgresContext context = new postgresContext())
            {
                TModerateProducts moderateProduct = dp.FormModerateProduct(product);
                context.TModerateProducts.Add(moderateProduct);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Метод для добавления продукта в магазин
        /// </summary>
        public void AddProduct(Product product)
        {
            using (postgresContext context = new postgresContext())
            {
                TProducts TProduct = dp.FormTableProducts(product);
                context.TProducts.Add(TProduct);
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
            List<int> Blacklist = JsonConvert.DeserializeObject<List<int>>(File.ReadAllText(path));

            //Если он пустой то возвращем false так как мы не в чёрном списке
            if (Blacklist == null)
                return false;
            else
                //Если он не пустой, то ищем пользователя с нужным нам id, если его там нет то возвращаем false если есть true
                return Blacklist.Where(u => u == IdMainUser) == null ? false : true;
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
                DirectoryInfo dirInfo = new DirectoryInfo($@"{path}\{TUser.Id}\Images");
                dirInfo.Create();

                //Копируем Файл со стандартным изображением в папку нового профиля  
                File.Copy($@"{path}\DefaultImage\MainImage.encr", $@"{path}\{TUser.Id}\Images\MainImage.encr", true);

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
                    Profile profile = dp.FormProfile(Tlogin);
                    profile.status = true;

                    //Записываем подключеного пользователя
                    OnlineUser ActiveUser = onlineUsers.FirstOrDefault(u => u.UserProfile.ID == profile.ID);
                    if (ActiveUser != null)
                    {
                        ActiveUser.operationContext.GetCallbackChannel<IWCFServiceCalbback>().ConnectionFromAnotherDevice();
                        onlineUsers.Remove(ActiveUser);
                    }
                    ActiveUser = new OnlineUser { UserProfile = profile, operationContext = OperationContext.Current };
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
        public Stream DownloadProduct(int idProduct)
        {
            //Указываем путь
            string path = $@"C:\Users\snayp\Documents\GitHub\DownloadGame\{idProduct}\Game.zip";

            //Записываем файл в массив байтов
            byte[] buffer = File.ReadAllBytes(path);

            //Передаём его через поток
            Stream stream = new MemoryStream(buffer);
            return stream;
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
                string Name = User.UserProfile.Login;
                //Выводим сообщение в серверную консоль
                Console.WriteLine($"{DateTime.Now.ToShortDateString()}, {DateTime.Now.ToShortTimeString()}:{Name} with Session Id {sessionId} disconnect");

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
            //Обновляем файл с запросами в друзьями у одного аккаунта
            string path = $@"{BaseSettings.Default.SourcePath}\Users\{idSender}\FriendRequests.json";
            List<Tuple<int, int>> friends = File.Exists(path) ? JsonConvert.DeserializeObject<List<Tuple<int, int>>>(File.ReadAllText(path)) : new List<Tuple<int, int>>();
            friends.Add(new Tuple<int, int>(idSender, idReceiver));
            File.WriteAllText(path, JsonConvert.SerializeObject(friends));

            //Обновляем файл с запросами в друзья у другого аккаунта
            path = $@"{BaseSettings.Default.SourcePath}\Users\{idReceiver}\FriendRequests.json";
            friends = File.Exists(path) ? JsonConvert.DeserializeObject<List<Tuple<int, int>>>(File.ReadAllText(path)) : new List<Tuple<int, int>>();
            friends.Add(new Tuple<int, int>(idSender, idReceiver));
            File.WriteAllText(path, JsonConvert.SerializeObject(friends));

            //Если пользователь в сети, то нужно вызвать callback для отображения запроса
            OnlineUser ReceiverUser = onlineUsers.FirstOrDefault(u => u.UserProfile.ID == idReceiver);
            if (ReceiverUser != null)
                ReceiverUser.operationContext.GetCallbackChannel<IWCFServiceCalbback>().GetFriendRequest(idSender);
        }



        /// <summary>
        /// Метод заполняет профиль
        /// </summary>
        /// <param name="idUser">ID пользователя</param>
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
    }
}