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
        public Exception AddUser(Profile profile, string Password)//Метод для добавления нового пользователя, в случае неудачи возвращает ошибка
        {
            using (postgresContext context = new postgresContext())
            {
                List<TUsers> Tusers = context.TUsers.ToList();
                List<TLogin> TLogins = context.TLogin.ToList();
                TUsers TUser = new TUsers();
                TLogin Tlogin = new TLogin();
                if (TLogins.FirstOrDefault(u => u.Login == profile.Login) != null)//Проверка что такой же логин не используеться
                    return new Exception("Данный логин уже занят");
                if (Tusers.FirstOrDefault(u => u.Mail == profile.Mail) != null)//Проверка что такая же почта не используеться
                    return new Exception("Данная почта уже зарегестрирована");
                if (Tusers.FirstOrDefault(u => u.Telephone == profile.Telephone) != null)//Проверка что такой же номер телефона не используеться
                    return new Exception("Данный номер телефона уже используеться");

                TUser.Id = (Tusers.Max(u => u.Id) + 1);
                TUser.Name = profile.Name;
                TUser.Mail = profile.Mail;
                TUser.Telephone = profile.Telephone;
                Tlogin.Id = (TLogins.Max(u => u.Id) + 1);
                Tlogin.IdOwner = TUser.Id;
                Tlogin.Login = profile.Login;
                Tlogin.Password = Password;
                context.TUsers.Add(TUser);//Добавление локальных изменений
                context.TLogin.Add(Tlogin);//Добавление локальных изменений
                context.SaveChanges();//Сохранение локальных изменений в БД
                return null;
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
