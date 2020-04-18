using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;

namespace Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Single)]
    public class WCFService : IWCFService
    {
        DataProvider dp = new DataProvider();

        /// <summary>
        /// Метод для покупки товаров
        /// </summary>
        public void BuyProduct(List<Product> Cart, int idProfile)
        {
            using (postgresContext context = new postgresContext())
            {
                List<TDeals> TDeals = context.TDeals.ToList();

                //Находим пользователя совершившего покупку
                TUsers profile = context.TUsers.FirstOrDefault(u => u.Id == idProfile);
                //Записываем все сделки в БД
                foreach (Product pr in Cart)
                {
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
        public void BuyProductWholesale(List<Tuple<Product, int>> Cart, int idProfile)
        {
            using (postgresContext context = new postgresContext())
            {
                List<TDeals> TDeals = context.TDeals.ToList();

                //Записываем все сделки в БД
                foreach (Tuple<Product, int> tuple in Cart)
                {
                    TDeals deal = dp.FormTDeal(idProfile, tuple.Item1, tuple.Item2, true);
                    context.TDeals.Add(deal);
                }

                //Сохраняем БД
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Метод для получения всех продуктов магазина
        /// </summary>
        public List<Product> GetProductTable()
        {
            using (postgresContext context = new postgresContext())
            {
                List<TProducts> TProducts = context.TProducts.Include(u => u.IdPublisherNavigation).Include(u => u.IdDeveloperNavigation).ToList();
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
            string path = $@"{BaseSettings.Default.SourcePath}\Users\{idMain}\Messages.json";
            List<UserMessage> messages = File.Exists(path) ? JsonConvert.DeserializeObject<List<UserMessage>>(File.ReadAllText(path)).Where(u => u.IDReceiver == idComrade).ToList() : null;
            return messages;
        }

        /// <summary>
        /// Метод для формирование полной версии профиля при переходе из подключённого
        /// </summary>
        /// <param name="id">ID нужного профиля</param>
        /// <returns></returns>
        public Profile CheckFriend(int id)
        {
            using (postgresContext context = new postgresContext())
            {
                Profile profile = CheckProfile(id);
                //Проверка что данный профиль не добавил вас в чёрный список
                bool blacklist = false;
                string path = $@"{BaseSettings.Default.SourcePath}\Users\{id}\Blacklist.json";
                if (File.Exists(path))
                {
                    List<int> Blacklist = JsonConvert.DeserializeObject<List<int>>(File.ReadAllText(path));
                    if (Blacklist.Where(u => u == id) != null)
                        blacklist = true;

                }

                return profile;
            }
        }


        /// <summary>
        /// Метод для удаления из чёрного списка
        /// </summary>
        /// <param name="id">ID пользователя который добавил в чёрный список</param>
        /// <param name="idUserInBlacklist">ID пользователя которого необходимо удалить из чёрного списока</param>
        public void RemoveFromBlacklist(int id, int idUserInBlacklist)
        {
            string path = $@"{BaseSettings.Default.SourcePath}\Users\{id}\Blacklist.json";//Сначала обновляем файл с друзьями у одного аккаунта
            List<int> Blacklist = File.Exists(path) ? JsonConvert.DeserializeObject<List<int>>(File.ReadAllText(path)) : new List<int>();
            Blacklist.Remove(idUserInBlacklist);
            File.WriteAllText(path, JsonConvert.SerializeObject(Blacklist));
        }

        /// <summary>
        /// Метод для добавления в чёрный список
        /// </summary>
        /// <param name="id">ID пользователя который добавил в чёрный список</param>
        /// <param name="idUserToBlacklist">ID пользователя которого добавили в чёрный список</param>
        public void AddToBlacklist(int id, int idUserToBlacklist)
        {
            string path = $@"{BaseSettings.Default.SourcePath}\Users\{id}\Blacklist.json";//Сначала обновляем файл с друзьями у одного аккаунта
            List<int> Blacklist = File.Exists(path) ? JsonConvert.DeserializeObject<List<int>>(File.ReadAllText(path)) : new List<int>();
            Blacklist.Add(idUserToBlacklist);
            File.WriteAllText(path, JsonConvert.SerializeObject(Blacklist));
        }

        /// <summary>
        /// Метод для добавления в список друзей
        /// </summary>
        public void AddFriend(int id, int idFriend)
        {
            //Обновляем файл с друзьями у одного аккаунта
            string path = $@"{BaseSettings.Default.SourcePath}\Users\{id}\Friends.json";//Сначала обновляем файл с друзьями у одного аккаунта
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

                TUsers TUser = new TUsers();
                TLogin Tlogin = new TLogin();

                if (profile.Name == "")
                    profile.Name = null;
                if (profile.Telephone == "")
                    profile.Telephone = null;

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
                    TOnlineUsers online = new TOnlineUsers { Id = profile.ID };
                    online.IdSession = OperationContext.Current.Channel.SessionId;

                    

                    TOnlineUsers user = context.TOnlineUsers.FirstOrDefault(u => u.Id == profile.ID);

                    if (user == null)
                        //Если пользователь не записан добавляем пользователя в БД
                        context.TOnlineUsers.Add(online);
                    else
                    {
                        //Если пользователь есть в бд, то обновляем его данные
                        string SessionId = user.IdSession;
                        //TODO: вызывать callback чтобы предыдущего usera выкидывало
                        user.IdSession = online.IdSession;
                        context.TOnlineUsers.Update(user);
                    }

                    //Сохраняем изменения в БД
                    context.SaveChanges();

                    //Добавляем события на выход из профиля
                    IContextChannel objClientHandle = OperationContext.Current.Channel;
                    objClientHandle.Closed += new EventHandler(ClientDisconnected);
                    objClientHandle.Faulted += new EventHandler(ClientDisconnected);

                    //Выводим сообщение в серверную консоль
                    Console.WriteLine($"{DateTime.Now.ToShortDateString()}, {DateTime.Now.ToShortTimeString()}: {Tlogin.Login} connect to server with channel {online.IdSession}");

                    return profile;
                }
                else
                    return null;//Если пользователь не найден возвращаем null
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
                Console.WriteLine($" {DateTime.Now.ToShortDateString()}, {DateTime.Now.ToShortTimeString()}: Account {context.TLogin.FirstOrDefault(u => u.Id == id).Login} deleted");
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
            string path = $@"{BaseSettings.Default.SourcePath}\Users\{msg.IDSender}\Messages.json";//Сначала обновляем файл с друзьями у одного аккаунта
            List<UserMessage> messages = File.Exists(path) ? JsonConvert.DeserializeObject<List<UserMessage>>(File.ReadAllText(path)) : new List<UserMessage>();
            messages.Add(msg);
            File.WriteAllText(path, JsonConvert.SerializeObject(messages));

            //Добавляем сообщения для второго пользователя
            path = $@"{BaseSettings.Default.SourcePath}\Users\{msg.IDReceiver}\Messages.json";//Сначала обновляем файл с друзьями у одного аккаунта
            messages = File.Exists(path) ? JsonConvert.DeserializeObject<List<UserMessage>>(File.ReadAllText(path)) : new List<UserMessage>();
            messages.Add(msg);
            File.WriteAllText(path, JsonConvert.SerializeObject(messages));

            //Если пользователь в сети, то нужно вызвать callback для отображения сообщения
            using (postgresContext context = new postgresContext())
            {
                TOnlineUsers UserReseived = context.TOnlineUsers.FirstOrDefault(u => u.Id == msg.IDReceiver);
                if (UserReseived != null)
                {
                    //TODO: вызвать callback
                }
            }
        }

        /// <summary>
        /// Метод для загрузки продукта
        /// </summary>
        public void DownloadProduct(Product pr)
        {

        }

        /// <summary>
        /// Событие при отключении клиента
        /// </summary>
        private void ClientDisconnected(object sender, EventArgs e)
        {
            using (postgresContext context = new postgresContext())
            {
                string sessionId = ((IClientChannel)sender).SessionId;

                TOnlineUsers online = context.TOnlineUsers.FirstOrDefault(u => u.IdSession == sessionId);
                if (online != null)
                {
                    context.TOnlineUsers.Remove(online);
                    context.SaveChanges();
                }

                //Вывод в серверную консоль
                Console.WriteLine($"{DateTime.Now.ToShortDateString()}, {DateTime.Now.ToShortTimeString()}: {context.TLogin.FirstOrDefault(u => u.Id == online.Id).Login} disconnect from server");
            }
        }

        /// <summary>
        /// Метод для сохранение корзины конкретного пользователя на сервере
        /// </summary>
        /// <param name="idUser">ID пользователя</param>
        /// <param name="Cart">Данные корзины</param>
        public void SaveCart(int idUser, List<Product> Cart)
        {
            string path = $@"{BaseSettings.Default.SourcePath}\Users\{idUser}\Cart.json";
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
            string path = $@"{BaseSettings.Default.SourcePath}\Users\{idSender}\FriendRequests.json";//Сначала обновляем файл с друзьями у одного аккаунта
            List<Tuple<int, int>> friends = File.Exists(path) ? JsonConvert.DeserializeObject<List<Tuple<int, int>>>(File.ReadAllText(path)) : new List<Tuple<int, int>>();
            friends.Add(new Tuple<int, int>(idSender, idReceiver));
            File.WriteAllText(path, JsonConvert.SerializeObject(friends));

            //Обновляем файл с запросами в друзья у другого аккаунта
            path = $@"{BaseSettings.Default.SourcePath}\Users\{idReceiver}\FriendRequests.json";
            friends = File.Exists(path) ? JsonConvert.DeserializeObject<List<Tuple<int, int>>>(File.ReadAllText(path)) : new List<Tuple<int, int>>();
            friends.Add(new Tuple<int, int>(idSender, idReceiver));
            File.WriteAllText(path, JsonConvert.SerializeObject(friends));

            //Если пользователь в сети, то нужно вызвать callback для отображения запроса
            using (postgresContext context = new postgresContext())
            {
                TOnlineUsers UserReseived = context.TOnlineUsers.FirstOrDefault(u => u.Id == idReceiver);
                if (UserReseived != null)
                {
                    //TODO: вызвать callback
                }
            }
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
        /// Мето для смены пароля
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