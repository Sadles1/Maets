using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Service
{
    class DataProvider
    {
        public void FormTableUser(Profile profile, string Password, ref TUsers TUser,ref TLogin Tlogin)//Метод формирует таблицы Users и Login используя класс профиль и пароль
        {
            using (postgresContext context = new postgresContext())
            {
                List<TUsers> Tusers = context.TUsers.ToList();
                List<TLogin> TLogins = context.TLogin.ToList();

                if (TLogins.FirstOrDefault(u => u.Login == profile.Login) != null)//Проверка что такой же логин не используеться
                    throw new FaultException("Данный логин уже занят");
                if (Tusers.FirstOrDefault(u => u.Mail == profile.Mail) != null)//Проверка что такая же почта не используеться
                    throw new FaultException("Данный почта уже зарегистрирована");
                if (Tusers.FirstOrDefault(u => u.Telephone == profile.Telephone) != null && profile.Telephone!=null)//Проверка что такой же номер телефона не используеться, но номер может быть пустой
                    throw new FaultException("Данный номер телефона уже используеться");

                if (Tusers.Count == 0)//Для формирования Id
                    TUser.Id = 1;
                else
                    TUser.Id = (Tusers.Max(u => u.Id) + 1);

                TUser.Name = profile.Name;
                TUser.Mail = profile.Mail;
                TUser.Telephone = profile.Telephone;
                TUser.AccessRight = profile.AccessRight;
                TUser.Money = 0;

                if (TLogins.Count == 0)//Для формирования Id
                    Tlogin.Id = 1;
                else
                    Tlogin.Id = (TLogins.Max(u => u.Id) + 1);

                Tlogin.IdOwner = TUser.Id;
                Tlogin.Login = profile.Login;
                Tlogin.Password = Password;             
            }
        }
        public TDeals FormTDeal(int id,int idProfile,Product pr,int Count,bool Wholesale)//Формирует таблицу сделок
        {
            TDeals deal = new TDeals();
            deal.Id = id;
            deal.Date = DateTime.Now.Date;
            deal.IdBuyers = idProfile;
            deal.IdProduct = pr.Id;
            deal.BuyingPrice = pr.RetailPrice;
            deal.Count = Count;
            deal.Wholesale = Wholesale;
            return deal;
        }
        public Profile FormProfile(TLogin Tlogin)//Формируем профиль используя таблицу login и связанные с ней таблицы
        {
            Profile profile = new Profile();
            profile.ID = Tlogin.Id;
            profile.Login = Tlogin.Login;
            using (FileStream fstream = File.OpenRead($@"{BaseSettings.Default.SourcePath}\Users\{profile.ID}\Images\MainImage.encr"))
            {
                profile.MainImage = new byte[fstream.Length];
                fstream.Read(profile.MainImage, 0, profile.MainImage.Length);
            }
            profile.Mail = Tlogin.IdOwnerNavigation.Mail;
            profile.Name = Tlogin.IdOwnerNavigation.Name;        
            profile.Money = Tlogin.IdOwnerNavigation.Money;
            profile.Telephone = Tlogin.IdOwnerNavigation.Telephone;
            profile.Discount = Tlogin.IdOwnerNavigation.PersonalDiscount;
            profile.AccessRight = Tlogin.IdOwnerNavigation.AccessRight;
            return profile;
        }       
        public Product FormProduct(TProducts TProduct)//Формируем продукт используя таблицу TProduct и связанные с ней таблицы
        {
            using (postgresContext context = new postgresContext())
            {
                List<TGameGenre> gameGenre = context.TGameGenre.Include(u=>u.IdGameNavigation).Include(u=>u.IdGenreNavigation).Where(u => u.IdGame == TProduct.Id).ToList();
                List<TMinGameSysReq> minGameSysReq = context.TMinGameSysReq.Include(u => u.IdSysReqNavigation).Include(u => u.IdGameNavigation).Where(u => u.IdGame == TProduct.Id).ToList();
                List<TRecGameSysReq> recGameSysReq = context.TRecGameSysReq.Include(u => u.IdSysReqNavigation).Include(u => u.IdGameNavigation).Where(u => u.IdGame == TProduct.Id).ToList();
                Product product = new Product();

                product.Id = TProduct.Id;
                product.Name = TProduct.Name;
                product.Publisher = TProduct.IdPublisherNavigation.Name;
                product.ReleaseDate = TProduct.ReleaseDate.Date;
                product.RetailPrice = TProduct.RetailPrice;

                product.GameGenre = new List<string>();
                foreach(TGameGenre genre in gameGenre)
                    product.GameGenre.Add(genre.IdGenreNavigation.Name);

                product.MinGameSysReq = new List<string>();
                foreach(TMinGameSysReq req in minGameSysReq)
                    product.MinGameSysReq.Add(req.IdSysReqNavigation.Name + req.Description);

                product.RecGameSysReq = new List<string>();
                foreach(TRecGameSysReq req in recGameSysReq)
                    product.RecGameSysReq.Add(req.IdSysReqNavigation.Name + req.Description);

                using (FileStream fstream = File.OpenRead($@"{BaseSettings.Default.SourcePath}\Products\{product.Id}\Images\MainImage.encr"))
                {
                    product.MainImage = new byte[fstream.Length];
                    fstream.Read(product.MainImage, 0, product.MainImage.Length);
                }

                product.Description = TProduct.Description;
                product.Developer = TProduct.IdDeveloperNavigation.Name;
                return product;
            }
        }
        public TProducts FormTableProducts(Product product)//Формирует таблицу product используя класс product
        {
            using (postgresContext context = new postgresContext())
            {
                TProducts TProduct = new TProducts();
                TProduct.Id = (context.TProducts.ToList().Max(u=>u.Id) +1);
                TProduct.Name = product.Name;
                TProduct.IdDeveloper = context.TDeveloper.FirstOrDefault(u => u.Name == product.Developer).Id;
                TProduct.IdPublisher = context.TPublisher.FirstOrDefault(u => u.Name == product.Publisher).Id;
                TProduct.Quantity = 100;
                TProduct.ReleaseDate = product.ReleaseDate;
                TProduct.RetailPrice = product.RetailPrice;
                TProduct.WholesalePrice = product?.WholesalePrice;
                TProduct.Description = product.Description;
                return TProduct;
            }
        }
        public byte[] GetByteImage(BitmapFrame image)//Преобразуем изображение в массив байтов
        {
            byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(image);
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }
            return data;
        }
    }
}