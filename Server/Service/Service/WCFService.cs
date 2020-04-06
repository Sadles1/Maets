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
        public void UpdateChat(Message message)
        {

        }
        public void AddFriend(int id, int idFriend)//Методя для добавления в список друзей
        {
            string path = $@"{BaseSettings.Default.SourcePath}\Users\{id}\Friends.json";
            List<int> friends = File.Exists(path) ? JsonConvert.DeserializeObject<List<int>>(File.ReadAllText(path)) : new List<int>();
            friends.Add(idFriend);
            File.WriteAllText(path, JsonConvert.SerializeObject(friends));
        }
        public void Register(Profile profile, string Password)//Метод для регистрации
        {
            using (postgresContext context = new postgresContext())
            {
                TUsers TUser = new TUsers();
                TLogin Tlogin = new TLogin();
                dp.FormTableUser(profile, Password, ref TUser, ref Tlogin);
                string path = $@"{BaseSettings.Default.SourcePath}\Users\{TUser.Id}";
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                if (dirInfo.Exists)
                    dirInfo.Delete();
                dirInfo.Create();
                context.TLogin.Add(Tlogin);
                context.TUsers.Add(TUser);
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
