using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Serialization;

namespace Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class WCFService : IWCFService
    {
        DataProvider dp = new DataProvider();
        public void BuyProduct(List<Product> Cart, int idProfile)//Метод для покупки товаров
        {
            using (postgresContext context = new postgresContext())
            {
                List<TDeals> TDeals = context.TDeals.ToList();
                int id;
                if (TDeals.Count == 0)//Для формирования Id
                    id = 1;
                else
                    id = (TDeals.Max(u => u.Id) + 1);
                TUsers profile = context.TUsers.FirstOrDefault(u => u.Id == idProfile);

                foreach (Product pr in Cart)
                {
                    TDeals deal = dp.FormTDeal(id, idProfile, pr, 1, false);
                    context.TDeals.Add(deal);
                    profile.Money -= pr.RetailPrice * (1 - profile.PersonalDiscount);
                    profile.TotalSpentMoney += pr.RetailPrice * (1 - profile.PersonalDiscount);
                    id++;
                }
                context.TUsers.Update(profile);
                context.SaveChanges();
            }
        }

        public void BuyProductWholesale(List<Tuple<Product, int>> Cart, int idProfile)//Метод для оптовой покупки товаров
        {
            using (postgresContext context = new postgresContext())
            {
                List<TDeals> TDeals = context.TDeals.ToList();
                int id;
                if (TDeals.Count == 0)//Для формирования Id
                    id = 1;
                else
                    id = (TDeals.Max(u => u.Id) + 1);

                foreach (Tuple<Product, int> tuple in Cart)
                {
                    TDeals deal = dp.FormTDeal(id, idProfile, tuple.Item1, tuple.Item2, true);
                    context.TDeals.Add(deal);
                    id++;
                }

                context.SaveChanges();
            }
        }

        public List<Product> GetProductTable()//Методя для получения всех продуктов магазина
        {
            using (postgresContext context = new postgresContext())
            {
                List<TProducts> TProducts = context.TProducts.Include(u => u.IdPublisherNavigation).Include(u => u.IdDeveloperNavigation).ToList();
                List<Product> products = new List<Product>();
                foreach (TProducts TProduct in TProducts)
                {
                    var product = dp.FormProduct(TProduct);
                    products.Add(product);
                }
                return products;
            }
        }

        public void AddProduct(Product product)//Метод для добавления продукта в магазин
        {
            using (postgresContext context = new postgresContext())
            {
                TProducts TProduct = dp.FormTableProducts(product);
                context.TProducts.Add(TProduct);
                context.SaveChanges();
            }
        }

        public List<Message> GetChat(int id)//Метод для получения сообщений
        {
            string path = $@"{BaseSettings.Default.SourcePath}\Users\{id}\Messages.json";
            List<Message> messages = JsonConvert.DeserializeObject<List<Message>>(File.ReadAllText(path));
            return messages;
        }

        public void AddFriend(int id, int idFriend)//Метод для добавления в список друзей
        {
            string path = $@"{BaseSettings.Default.SourcePath}\Users\{id}\Friends.json";//Сначала обновляем файл с друзьями у одного аккаунта
            List<int> friends = File.Exists(path) ? JsonConvert.DeserializeObject<List<int>>(File.ReadAllText(path)) : new List<int>();
            friends.Add(idFriend);
            File.WriteAllText(path, JsonConvert.SerializeObject(friends));

            path = $@"{BaseSettings.Default.SourcePath}\Users\{idFriend}\Friends.json";//Теперь обновляем файл с друзьями у другого аккаунта
            friends = File.Exists(path) ? JsonConvert.DeserializeObject<List<int>>(File.ReadAllText(path)) : new List<int>();
            friends.Add(id);
            File.WriteAllText(path, JsonConvert.SerializeObject(friends));
        }

        public void Register(Profile profile, string Password)//Метод для регистрации
        {
            using (postgresContext context = new postgresContext())
            {
                TUsers TUser = new TUsers();
                TLogin Tlogin = new TLogin();
                dp.FormTableUser(profile, Password, ref TUser, ref Tlogin);
                string path = $@"{BaseSettings.Default.SourcePath}\Users";
                DirectoryInfo dirInfo = new DirectoryInfo($@"{path}\{TUser.Id}\Images");
                if (dirInfo.Exists)
                    dirInfo.Delete();
                dirInfo.Create();
                File.Copy($@"{path}\DefaultImage\MainImage.encr", $@"{path}\{TUser.Id}\Images\MainImage.encr", true);
                context.TLogin.Add(Tlogin);
                context.TUsers.Add(TUser);
                context.SaveChanges();
            }
        }

        public void AddMoney(int id, int money)
        {
            using (postgresContext context = new postgresContext())
            {
                TUsers user = context.TUsers.FirstOrDefault(u => u.Id == id);
                user.Money += money;
                context.TUsers.Update(user);
                context.SaveChanges();
            }
        }
        public Profile Connect(string Login, string Password)//Метод для подключения к магазину
        {
            using (postgresContext context = new postgresContext())
            {
                List<TLogin> TLogins = context.TLogin.Include(u => u.IdOwnerNavigation).ToList();
                List<TOnlineUsers> onlineUsers = context.TOnlineUsers.Include(u => u.IdUsersNavigation).ToList();
                TLogin Tlogin = TLogins.FirstOrDefault(u => u.Login == Login && u.Password == Password);
                if (Tlogin != null)
                {
                    Profile profile = dp.FormProfile(Tlogin);
                    TOnlineUsers online = new TOnlineUsers();
                    online.Id = profile.ID;
                    string path = $@"{BaseSettings.Default.SourcePath}\Users\{profile.ID}\";
                    if (File.Exists($@"{path}Friends.json"))
                    {
                        profile.Friends = new List<Profile>();
                        List<int> IdFriends = JsonConvert.DeserializeObject<List<int>>(File.ReadAllText($@"{path}Friends.json"));
                        foreach (int id in IdFriends)
                        {
                            Profile friend = dp.FormProfile(TLogins.FirstOrDefault(u => u.IdOwner == id));
                            if (friend != null)
                            {
                                profile.Friends.Add(friend);
                                if (context.TOnlineUsers.FirstOrDefault(u => u.Id == friend.ID) != null)
                                    friend.status = true;
                                else
                                    friend.status = false;
                            }
                        }

                    }
                    context.TOnlineUsers.Add(online);
                    context.SaveChanges();
                    Console.WriteLine($"{DateTime.Now.ToShortDateString()}, {DateTime.Now.ToShortTimeString()}: {Tlogin.Login} connect to server");//Вывод в серверную консоль
                    return profile;
                }
                else
                    return null;
            }
        }

        public void Disconnect(int id)//Метод для отключения от магазина
        {
            using (postgresContext context = new postgresContext())
            {
                TOnlineUsers online = new TOnlineUsers { Id = id };
                context.TOnlineUsers.Remove(online);
                context.SaveChanges();
            }
        }

    }
}
