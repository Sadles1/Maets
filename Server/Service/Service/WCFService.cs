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
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
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
                TProducts TProduct = new TProducts();
                TProduct.Id = product.Id;
                TProduct.Name = product.Name;
                TProduct.IdDeveloper = context.TDeveloper.FirstOrDefault(u => u.Name == product.Developer).Id;
                TProduct.IdPublisher = context.TPublisher.FirstOrDefault(u => u.Name == product.Publisher).Id;
                TProduct.Quantity = 100;
                TProduct.ReleaseDate = product.ReleaseDate;
                TProduct.RetailPrice = product.RetailPrice;
                TProduct.WholesalePrice = product.WholesalePrice;
                TProduct.Description = product.Description;
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
        public Exception Register(Profile profile, string Password)
        {
            Exception ex = dp.AddUser(profile, Password);
            return ex;
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
