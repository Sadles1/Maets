using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace Service
{
    class DataProvider
    {
        public void FormTableUser(Profile profile, string Password, ref TUsers TUser,ref TLogin Tlogin)//Метод формирует таблицу Users и Login используя класс профиль и пароль
        {
            using (postgresContext context = new postgresContext())
            {
                List<TUsers> Tusers = context.TUsers.ToList();
                List<TLogin> TLogins = context.TLogin.ToList();

                if (TLogins.FirstOrDefault(u => u.Login == profile.Login) != null)//Проверка что такой же логин не используеться
                    throw new FaultException("Данный логин уже занят");
                if (Tusers.FirstOrDefault(u => u.Mail == profile.Mail) != null)//Проверка что такая же почта не используеться
                    throw new FaultException("Данная почта уже зарегистрирована");
                if (Tusers.FirstOrDefault(u => u.Telephone == profile.Telephone) != null && profile.Telephone!=null)//Проверка что такой же номер телефона не используеться но номер может быть пустой
                    throw new FaultException("Данный номер телефона уже используеться");

                if (Tusers.Count == 0)//Для формирования Id
                    TUser.Id = 1;
                else
                    TUser.Id = (Tusers.Max(u => u.Id) + 1);

                TUser.Name = profile.Name;
                TUser.Mail = profile.Mail;
                TUser.Telephone = profile.Telephone;
                TUser.AccessRight = profile.AccessRight;

                if (TLogins.Count == 0)//Для формирования Id
                    Tlogin.Id = 1;
                else
                    Tlogin.Id = (TLogins.Max(u => u.Id) + 1);

                Tlogin.IdOwner = TUser.Id;
                Tlogin.Login = profile.Login;
                Tlogin.Password = Password;             
            }
        }

        public Profile FormProfile(TLogin Tlogin)//Формируем профиль используя таблицу Tlogin и связанные с ней таблицы
        {
            Profile profile = new Profile();
            profile.ID = Tlogin.Id;
            profile.Login = Tlogin.Login;
            profile.MainImage = GetByteImage(BitmapFrame.Create(new Uri($@"{BaseSettings.Default.SourcePath}\Users\{profile.ID}\Images\MainImage.jpg")));//В изображение записываем массив байтов
            profile.Mail = Tlogin.IdOwnerNavigation.Mail;
            profile.Name = Tlogin.IdOwnerNavigation.Name;        
            profile.Money = Tlogin.IdOwnerNavigation.Money;
            profile.Telephone = Tlogin.IdOwnerNavigation.Telephone;
            profile.Discount = Tlogin.IdOwnerNavigation.PersonalDiscount;
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

                product.MainImage = GetByteImage(BitmapFrame.Create(new Uri($@"{BaseSettings.Default.SourcePath}\Products\{TProduct.Id}\Images\MainImage.jpg")));//В изображение записываем массив байтов
                product.Description = TProduct.Description;
                product.Developer = TProduct.IdDeveloperNavigation.Name;
                return product;
            }
        }
        public TProducts FormTableProducts(Product product)//Метод для формирования таблицы продуктов используя класс продукт
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