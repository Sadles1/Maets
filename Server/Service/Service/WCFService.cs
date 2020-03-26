using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
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
            using(postgresContext context = new postgresContext())
            {
                TProducts TProduct=dp.FormTableProducts(product);
                context.TProducts.Add(TProduct);
                context.SaveChanges();
            }
        }
        public void GetChat()
        {

        }
        public void UpdateChat(Message message)
        {
            XmlSerializer xml = new XmlSerializer(typeof(Message));
            using (FileStream fs = new FileStream($@"C:\Users\snayp\Documents\GitHub\MTPProject\Messages\User{message.IDSender}.xml", FileMode.OpenOrCreate))
            {
                xml.Serialize(fs, message);
            }
        }
        public void Register(Profile profile, string Password)
        {
            using (postgresContext context = new postgresContext())
            {
                TUsers TUser = new TUsers();
                TLogin Tlogin = new TLogin();
                dp.FormTableUser(profile, Password,ref TUser,ref Tlogin);
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
                TLogin Tlogin = TLogins.FirstOrDefault(u => u.Login == Login && u.Password == Password);
                if (Tlogin != null)
                {
                    Console.WriteLine($"{DateTime.Now.ToShortDateString()}, {DateTime.Now.ToShortTimeString()}: {Tlogin.Login} connect to server");
                    return dp.FormProfile(Tlogin);
                }
                else
                {
                    return null;
                }
            }
        }
        public void Disconnect()//Метод для отключения от магазина
        {

        }
    }
}
