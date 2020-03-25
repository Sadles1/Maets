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
        public List<Product> GetProductTable()//Методя для получения всех продуктов магазина
        {
            using (postgresContext context = new postgresContext())
            {
                List<TProducts> TProducts = context.TProducts.Include(u => u.IdpublisherNavigation).Include(u => u.IddeveloperNavigation).ToList();
                List<Product> products = new List<Product>();
                foreach (TProducts TProduct in TProducts)
                {
                    var product = DataProvider.FormProduct(TProduct);
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
                TProduct.Iddeveloper = context.TDeveloper.FirstOrDefault(u => u.Name == product.Developer).Id;
                TProduct.Idpublisher = context.TPublisher.FirstOrDefault(u => u.Name == product.Publisher).Id;
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
        public bool Register(Profile profile, string Password)
        {
            using (postgresContext context = new postgresContext())
            {
                TUsers TUser = new TUsers();
                TLogin Tlogin = new TLogin();

                TUser.Name = profile.Name;
                TUser.Mail = profile.Mail;
                TUser.Telephone = profile.Telephone;
                context.TUsers.Add(TUser);
                context.SaveChanges();

                Tlogin.Login = profile.Login;
                Tlogin.Password = Password;
                Tlogin.Idowner = 1;
                context.TLogin.Add(Tlogin);
                context.SaveChanges();
                return false;
            }
        }
        public bool Connect(string Login, string Password)//Метод для подключения к магазину
        {
            using (postgresContext context = new postgresContext())
            {
                List<TLogin> TLogins = context.TLogin.ToList();
                TLogin Tlogin = TLogins.FirstOrDefault(u => u.Login == Login);
                if (Tlogin != null)
                {
                    Console.WriteLine($"{DateTime.Now.ToShortDateString()}, {DateTime.Now.ToShortTimeString()}: {Tlogin.Login} connect to server");
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public void Disconnect()//Метод для отключения от магазина
        {

        }
    }
}
