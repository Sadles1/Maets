using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        public void FormTableUser(Profile profile, string Password, ref TUsers TUser, ref TLogin Tlogin)//Метод формирует таблицы Users и Login используя класс профиль и пароль
        {
            using (postgresContext context = new postgresContext())
            {

                List<TUsers> Tusers = context.TUsers.ToList();
                List<TLogin> TLogins = context.TLogin.ToList();

                //Проверка что такой же логин не используеться
                if (TLogins.FirstOrDefault(u => u.Login == profile.Login) != null)
                    throw new FaultException("Данный логин уже занят");

                //Проверка что такая же почта не используеться
                if (Tusers.FirstOrDefault(u => u.Mail == profile.Mail) != null)
                    throw new FaultException("Данная почта уже зарегистрирована");

                //Проверка что такой же номер телефона не используеться, но номер может быть пустой
                if (Tusers.FirstOrDefault(u => u.Telephone == profile.Telephone) != null && profile.Telephone != null)
                    throw new FaultException("Данный номер телефона уже используеться");

                //Формируем ID
                TUser.Id = Tusers.Count == 0 ? 0 : (Tusers.Max(u => u.Id) + 1);

                //Формируем таблицу TUser
                TUser.Name = profile.Name;
                TUser.Mail = profile.Mail;
                TUser.Telephone = profile.Telephone;
                TUser.AccessRight = profile.AccessRight;
                TUser.Money = 0;
                TUser.PersonalDiscount = 0;

                //Формируем таблицу TLogin
                Tlogin.Id = TUser.Id;
                Tlogin.Login = profile.Login;
                Tlogin.Password = Password;

            }
        }

        public TDeals FormTDeal(int id, int idProfile, Product pr, int Count, bool Wholesale)//Формирует таблицу сделок
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

            //Все данные получем через связанную таблицу
            profile.Mail = Tlogin.IdNavigation.Mail;
            profile.Name = Tlogin.IdNavigation.Name;
            profile.Money = Tlogin.IdNavigation.Money;
            profile.Telephone = Tlogin.IdNavigation.Telephone;
            profile.Discount = Tlogin.IdNavigation.PersonalDiscount;
            profile.AccessRight = Tlogin.IdNavigation.AccessRight;


            using (postgresContext context = new postgresContext())
            {
                //Формируем список друзей
                string path = $@"{BaseSettings.Default.SourcePath}\Users\{profile.ID}\";
                if (File.Exists($@"{path}Friends.json"))
                {
                    List<TLogin> TLogins = context.TLogin.Include(u => u.IdNavigation).ToList();

                    profile.Friends = new List<Profile>();
                    List<int> IdFriends = JsonConvert.DeserializeObject<List<int>>(File.ReadAllText($@"{path}Friends.json"));
                    foreach (int id in IdFriends)
                    {
                        Profile friend = SimpleFormProfile(TLogins.FirstOrDefault(u => u.Id == id));
                        if (friend != null)
                            profile.Friends.Add(friend);
                    }
                }

                //Определяем статус подключения
                profile.status = context.TOnlineUsers.FirstOrDefault(u => u.Id == profile.ID) != null ? true : false;//Определяет в сети ли человек
            }

            //Получем основное изображение профиля
            using (FileStream fstream = File.OpenRead($@"{BaseSettings.Default.SourcePath}\Users\{profile.ID}\Images\MainImage.encr"))
            {
                profile.MainImage = new byte[fstream.Length];
                fstream.Read(profile.MainImage, 0, profile.MainImage.Length);
            }

            return profile;
        }

        private Profile SimpleFormProfile(TLogin Tlogin)//Упрощённое формирования профиля для друзей
        {
            Profile profile = new Profile();

            profile.ID = Tlogin.Id;
            profile.Login = Tlogin.Login;

            //Определяем статус
            using (postgresContext context = new postgresContext())
            {
                profile.status = context.TOnlineUsers.FirstOrDefault(u => u.Id == profile.ID) != null ? true : false;//Определяет в сети ли человек
            }

            //Получем основное изображение
            using (FileStream fstream = File.OpenRead($@"{BaseSettings.Default.SourcePath}\Users\{profile.ID}\Images\MainImage.encr"))
            {
                profile.MainImage = new byte[fstream.Length];
                fstream.Read(profile.MainImage, 0, profile.MainImage.Length);
            }
            return profile;
        }

        public Product FormProduct(TProducts TProduct)//Формируем продукт используя таблицу TProduct и связанные с ней таблицы
        {
            using (postgresContext context = new postgresContext())
            {
                //Составляем листы для жанров мин. и рек. системных требований
                List<TGameGenre> gameGenre = context.TGameGenre.Include(u => u.IdGameNavigation).Include(u => u.IdGenreNavigation).Where(u => u.IdGame == TProduct.Id).ToList();
                List<TMinGameSysReq> minGameSysReq = context.TMinGameSysReq.Include(u => u.IdSysReqNavigation).Include(u => u.IdGameNavigation).Where(u => u.IdGame == TProduct.Id).ToList();
                List<TRecGameSysReq> recGameSysReq = context.TRecGameSysReq.Include(u => u.IdSysReqNavigation).Include(u => u.IdGameNavigation).Where(u => u.IdGame == TProduct.Id).ToList();

                Product product = new Product();

                product.Id = TProduct.Id;
                product.Name = TProduct.Name;
                product.Description = TProduct.Description;
                product.Publisher = TProduct.IdPublisherNavigation.Name;
                product.Developer = TProduct.IdDeveloperNavigation.Name;
                product.ReleaseDate = TProduct.ReleaseDate.Date;
                product.RetailPrice = TProduct.RetailPrice;

                product.GameGenre = new List<string>();
                foreach (TGameGenre genre in gameGenre)
                    product.GameGenre.Add(genre.IdGenreNavigation.Name);

                product.MinGameSysReq = new List<string>();
                foreach (TMinGameSysReq req in minGameSysReq)
                    product.MinGameSysReq.Add(req.IdSysReqNavigation.Name + req.Description);

                product.RecGameSysReq = new List<string>();
                foreach (TRecGameSysReq req in recGameSysReq)
                    product.RecGameSysReq.Add(req.IdSysReqNavigation.Name + req.Description);


                using (FileStream fstream = File.OpenRead($@"{BaseSettings.Default.SourcePath}\Products\{product.Id}\Images\MainImage.encr"))
                {
                    product.MainImage = new byte[fstream.Length];
                    fstream.Read(product.MainImage, 0, product.MainImage.Length);
                }

                return product;
            }
        }

        public TModerateProducts FormModerateProduct(Product product)//Метод для формирования таблицы TModerateProduct
        {
            using (postgresContext context = new postgresContext())
            {
                TModerateProducts TModerateProduct = new TModerateProducts();

                //Формируем продукт
                TModerateProduct.Id = product.Id;
                TModerateProduct.Name = product.Name;
                TModerateProduct.Description = product.Description;
                TModerateProduct.IdDeveloper = context.TDeveloper.FirstOrDefault(u => u.Name == product.Developer).Id;
                TModerateProduct.IdPublisher = context.TPublisher.FirstOrDefault(u => u.Name == product.Publisher).Id;
                TModerateProduct.RequestDate = product.ReleaseDate;
                TModerateProduct.RetailPrice = product.RetailPrice;
                TModerateProduct.WholesalePrice = product?.WholesalePrice;

                //Выбираем двух случаных модераторов для модерирования
                List<int> Moderators = context.TUsers.Where(u => u.AccessRight == 3).Select(u=>u.Id).ToList();//Список всех id модераторов

                Random rnd = new Random();
                int rnd1 = rnd.Next(0, Moderators.Count);
                int rnd2 = rnd.Next(0, Moderators.Count);
                while (rnd2 == rnd1)
                    rnd2 = rnd.Next(0, Moderators.Count);
                int moderator1 = Moderators[rnd1];
                int moderator2 = Moderators[rnd2];

                TModerateEmployers employer = new TModerateEmployers();
                employer.IdEmployee = moderator1;
                employer.IdModerateProduct = product.Id;
                employer.Result = null;
                TModerateProduct.TModerateEmployers.Add(employer);
                context.TModerateEmployers.Add(employer);

                employer = new TModerateEmployers();
                employer.IdEmployee = moderator2;
                employer.IdModerateProduct = product.Id;
                employer.Result = null;
                TModerateProduct.TModerateEmployers.Add(employer);
                context.TModerateEmployers.Add(employer);

                context.SaveChanges();
                return TModerateProduct;
            }
        }

        public TProducts FormTableProducts(Product product)//Формирует таблицу product используя класс product
        {
            using (postgresContext context = new postgresContext())
            {
                TProducts TProduct = new TProducts();

                TProduct.Id = (context.TProducts.ToList().Max(u => u.Id) + 1);
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