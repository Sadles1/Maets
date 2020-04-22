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

        public TDeals FormTDeal(int idDeal, int idProfile, Product pr, int Count, bool Wholesale)//Формирует таблицу сделок
        {
            TDeals deal = new TDeals();

            deal.Id = idDeal;
            deal.Date = DateTime.Now.Date;
            deal.IdBuyers = idProfile;
            deal.IdProduct = pr.Id;
            deal.BuyingPrice = Wholesale == true ? pr.WholesalePrice : pr.RetailPrice;
            deal.Count = Count;
            deal.Wholesale = Wholesale;

            return deal;
        }

        public Profile FormActiveUser(TLogin Tlogin)
        {
            Profile profile = FormProfile(Tlogin);

            profile.Money = Tlogin.IdNavigation.Money;
            profile.AccessRight = Tlogin.IdNavigation.AccessRight;
            profile.Discount = Tlogin.IdNavigation.PersonalDiscount;

            //Формируем список игр
            profile.Games = GetUserGames(profile.ID);

            return profile;
        }

        public Profile FormProfile(TLogin Tlogin)//Формируем профиль используя таблицу login и связанные с ней таблицы
        {
            Profile profile = SimpleFormProfile(Tlogin);

            //Все данные получем через связанную таблицу
            profile.Mail = Tlogin.IdNavigation.Mail;
            profile.Name = Tlogin.IdNavigation.Name;          
            profile.Telephone = Tlogin.IdNavigation.Telephone;             
            
            //Определяем путь
            string path = $@"{BaseSettings.Default.SourcePath}\Users\{profile.ID}\";

            profile.Friends = new List<Profile>();

            using (postgresContext context = new postgresContext())
            {
                //Загружаем таблицу TLogins
                List<TLogin> TLogins = context.TLogin.ToList();
                
                //Формируем список друзей
                if (File.Exists($@"{path}Friends.json"))
                {
                    List<int> IdFriends = JsonConvert.DeserializeObject<List<int>>(File.ReadAllText($@"{path}Friends.json"));
                    foreach (int id in IdFriends)
                    {
                        Profile friend = SimpleFormProfile(TLogins.FirstOrDefault(u => u.Id == id));
                        if (friend != null)
                            profile.Friends.Add(friend);
                    }
                }
            }

            return profile;
        }

        public Profile SimpleFormProfile(TLogin Tlogin)//Упрощённое формирования профиля
        {
            Profile profile = new Profile();

            profile.ID = Tlogin.Id;
            profile.Login = Tlogin.Login;

            profile.status = WCFService.onlineUsers.FirstOrDefault(u => u.UserProfile.ID == profile.ID) != null;

            //Получем основное изображение профиля
            using (FileStream fstream = File.OpenRead($@"{BaseSettings.Default.SourcePath}\Users\{profile.ID}\MainImage.encr"))
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

                //Получаем основное изображение игры
                using (FileStream fstream = File.OpenRead($@"{BaseSettings.Default.SourcePath}\Products\{product.Id}\MainImage.encr"))
                {
                    product.MainImage = new byte[fstream.Length];
                    fstream.Read(product.MainImage, 0, product.MainImage.Length);
                }

                return product;
            }
        }

        public List<Product> GetUserGames(int id)
        {
            List<Product> Games = new List<Product>(); 
            using (postgresContext context = new postgresContext())
            {
                List<int> idGames = context.TDeals.Where(u => u.IdBuyers == id && u.Wholesale == false).Select(u => u.IdProduct).ToList();
                if (idGames.Count != 0)
                {
                    List<TProducts> TProducts = context.TProducts.Include(u => u.IdPublisherNavigation).Include(u => u.IdDeveloperNavigation).ToList();
                    foreach (int Id in idGames)
                        Games.Add(FormProduct(TProducts.FirstOrDefault(u => u.Id == Id)));
                }
            }
            return Games;
        }

        public void CalculateGameScore(int idGame)
        {
            using(postgresContext context = new postgresContext())
            {
                //Находим нужную нам игру
                TProducts product = context.TProducts.FirstOrDefault(u=>u.Id == idGame);

                //Вычисляем её среднюю оценку
                product.Rate = context.TComments.Where(u=>u.IdProduct == idGame).Average(u=>u.Score);

                //Обновляем её в БД
                context.TProducts.Update(product);

                //Сохраняем изменения в БД
                context.SaveChanges();
            }
        }

        public Comment FormComment(TComments TComment)
        {
            Comment comment = new Comment();
            comment.id = TComment.Id;
            comment.idProduct = TComment.IdProduct;
            comment.idUser = TComment.IdUser;
            comment.Score = TComment.Score;
            comment.comment = TComment.Comment;
            return comment;
        }

        public string RandomString(int length)
        {
            var result = new char[length];
            var r = new Random();
            for (int i = 0; i < result.Length; i++)
            {
                do
                    result[i] = (char)r.Next(127);
                while (result[i] < '!');
            }
            return new string(result);
        }

    }
}