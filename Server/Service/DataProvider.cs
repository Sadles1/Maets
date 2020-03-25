using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Service
{
    class DataProvider
    {
        public void AddUser(Profile profile, string Password)
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

                Tlogin.IdOwner = TUser.Id;
                Tlogin.Login = profile.Login;
                Tlogin.Password = Password;
                context.TLogin.Add(Tlogin);
                context.SaveChanges();
            }
        }

        public Profile FormProfile(TLogin Tlogin)//Формируем профиль используя таблицу Tlogin и связанные с ней таблицы
        {
            Profile profile = new Profile();
            profile.ID = Tlogin.Id;
            profile.Login = Tlogin.Login;
            profile.Mail = Tlogin.IdOwnerNavigation.Mail;
            profile.Name = Tlogin.IdOwnerNavigation.Name;
            profile.Money = Tlogin.IdOwnerNavigation.Money;
            profile.Telephone = Tlogin.IdOwnerNavigation.Telephone;
            profile.Discount = Tlogin.IdOwnerNavigation.PersonalDiscount;
            return profile;
        }

        public Product FormProduct(TProducts TProduct)//Формируем продукт используя таблицу TProduct и связанные с ней таблицы
        {
            Product product = new Product();
            product.Id = TProduct.Id;
            product.Name = TProduct.Name;
            product.Publisher = TProduct.IdPublisherNavigation.Name;
            product.ReleaseDate = TProduct.ReleaseDate.Date;
            product.RetailPrice = TProduct.RetailPrice;
            product.MainImage = GetByteImage(BitmapFrame.Create(new Uri($@"{BaseSettings.Default.SourcePath}\Products\{TProduct.Id}\Screenshots\MainImage.jpg")));//В изображение записываем массив байтов
            product.Description = TProduct.Description;
            product.Developer = TProduct.IdDeveloperNavigation.Name;
            return product;
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
