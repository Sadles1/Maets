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
                CheckUserInfo(profile.Login, profile.Mail, profile.Telephone);


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

        public void CheckUserInfo(string login, string mail, string telephone)
        {
            using (postgresContext context = new postgresContext())
            {
                List<TUsers> Tusers = context.TUsers.ToList();
                List<TLogin> TLogins = context.TLogin.ToList();
                //Проверка что такой же логин не используеться
                if (TLogins.FirstOrDefault(u => u.Login == login) != null)
                    throw new FaultException("Данный логин уже занят");
                //Проверка что такая же почта не используеться
                if (Tusers.FirstOrDefault(u => u.Mail == mail) != null)
                    throw new FaultException("Данная почта уже зарегистрирована");
                //Проверка что такой же номер телефона не используеться, но номер может быть пустой
                if (Tusers.FirstOrDefault(u => u.Telephone == telephone) != null && telephone != null)
                    throw new FaultException("Данный номер телефона уже используеться");
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
            profile.Friends = GetProfileFriends(profile.ID);          

            return profile;
        }

        public Profile SimpleFormProfile(TLogin Tlogin)//Упрощённое формирования профиля
        {
            Profile profile = new Profile();

            profile.ID = Tlogin.Id;
            profile.Login = Tlogin.Login;

            profile.status = WCFService.onlineUsers.FirstOrDefault(u => u.UserProfile.ID == profile.ID) != null ? "Online" : "Offline";

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
                product.Rate = TProduct.Rate;
                product.RetailPrice = TProduct.RetailPrice;
                product.WholesalePrice = TProduct.WholesalePrice;

                product.GameGenre = new List<string>();
                foreach (TGameGenre genre in gameGenre)
                    product.GameGenre.Add(genre.IdGenreNavigation.Name);

                product.MinGameSysReq = new List<string>();
                foreach (TMinGameSysReq req in minGameSysReq)
                    product.MinGameSysReq.Add(req.IdSysReqNavigation.Name + ": " + req.Description);

                product.RecGameSysReq = new List<string>();
                foreach (TRecGameSysReq req in recGameSysReq)
                    product.RecGameSysReq.Add(req.IdSysReqNavigation.Name + ": " + req.Description);

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
                List<int> idGames = context.TUsersGames.Where(u => u.Iduser == id).Select(u => u.Idproduct).ToList();
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
            using (postgresContext context = new postgresContext())
            {
                //Находим нужную нам игру
                TProducts product = context.TProducts.FirstOrDefault(u => u.Id == idGame);

                //Вычисляем её среднюю оценку
                product.Rate = context.TComments.Where(u => u.IdProduct == idGame).Average(u => u.Score);

                //Обновляем её в БД
                context.TProducts.Update(product);

                //Сохраняем изменения в БД
                context.SaveChanges();
            }
        }

        public string GenRandomString(string Alphabet, int Length)
        {
            //создаем объект Random, генерирующий случайные числа
            Random rnd = new Random();
            //объект StringBuilder с заранее заданным размером буфера под результирующую строку
            StringBuilder sb = new StringBuilder(Length - 1);
            //переменную для хранения случайной позиции символа из строки Alphabet
            int Position = 0;
            for (int i = 0; i < Length; i++)
            {
                //получаем случайное число от 0 до последнего
                //символа в строке Alphabet
                Position = rnd.Next(0, Alphabet.Length - 1);
                //добавляем выбранный символ в объект
                //StringBuilder
                sb.Append(Alphabet[Position]);
            }
            //Возвращаем сгенерированную строку
            return sb.ToString();
        }


        /// <summary>
        /// Метод для удаления товара с модерации
        /// </summary>
        public void RemoveFromModeration(int idModerationProduct)
        {
            using(postgresContext context = new postgresContext())
            {
                string pathToModeratateProduct = $@"{BaseSettings.Default.SourcePath}\ModerateProducts\{idModerationProduct}";

                //Удалить все файлы
                if(Directory.Exists(pathToModeratateProduct))
                    foreach (string newPath in Directory.GetFiles(pathToModeratateProduct, "*.*", SearchOption.AllDirectories))
                        File.Delete(newPath);
                Directory.Delete($@"{pathToModeratateProduct}\Images");
                Directory.Delete(pathToModeratateProduct);

                List<TModerateEmployers> moderateEmployers = context.TModerateEmployers.Where(u => u.IdModerateProduct == idModerationProduct).ToList();
                foreach (TModerateEmployers moderator in moderateEmployers)
                    context.TModerateEmployers.Remove(moderator);

                context.SaveChanges();

                TModerateProducts moderateProduct = context.TModerateProducts.FirstOrDefault(u => u.Id == idModerationProduct);
                context.TModerateProducts.Remove(moderateProduct);
                context.SaveChanges();
            }
        }


        /// <summary>
        /// Метод для добавления продукта в магазин
        /// </summary>
        public void AddProduct(int idModerateProduct)
        {
            using (postgresContext context = new postgresContext())
            {
                TProducts TProduct = new TProducts();
                TModerateProducts moderateProduct = context.TModerateProducts.FirstOrDefault(u => u.Id == idModerateProduct);
                List<TProducts> products = context.TProducts.ToList();

                TProduct.Id = products.Count > 0 ? (products.Max(u => u.Id) + 1) : 1;
                TProduct.Name = moderateProduct.Name;
                TProduct.Description = moderateProduct.Description;
                TProduct.IdDeveloper = moderateProduct.IdDeveloper;
                TProduct.IdPublisher = moderateProduct.IdPublisher;

                string pathToModeratateProduct = $@"{BaseSettings.Default.SourcePath}\ModerateProducts\{idModerateProduct}";
                string pathToProduct = $@"{BaseSettings.Default.SourcePath}\Products\{TProduct.Id}";

                //Создать идентичное дерево каталогов
                foreach (string dirPath in Directory.GetDirectories(pathToModeratateProduct, "*", SearchOption.AllDirectories))
                    Directory.CreateDirectory(dirPath.Replace(pathToModeratateProduct, pathToProduct));

                //Скопировать все файлы. И перезаписать(если такие существуют)
                foreach (string newPath in Directory.GetFiles(pathToModeratateProduct, "*.*", SearchOption.AllDirectories))
                    File.Copy(newPath, newPath.Replace(pathToModeratateProduct, pathToProduct), true);

                //Удалить все файлы
                foreach (string newPath in Directory.GetFiles(pathToModeratateProduct, "*.*", SearchOption.AllDirectories))
                    File.Delete(newPath);



                TProduct.Quantity = 100;
                TProduct.ReleaseDate = DateTime.Now.Date;
                TProduct.RetailPrice = moderateProduct.RetailPrice;
                TProduct.WholesalePrice = moderateProduct.WholesalePrice;
                context.TProducts.Add(TProduct);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Метод для предоставления скидки пользователю
        /// </summary>
        /// <param name="id"></param>
        public void CheckDiscount(int id)
        {
            using(postgresContext context = new postgresContext())
            {
                TUsers user = context.TUsers.FirstOrDefault(u => u.Id == id);
                if (user.TotalSpentMoney >= 1500 && user.TotalSpentMoney < 3000)
                {
                    user.PersonalDiscount = 3;
                    Console.WriteLine("Test1 " + user.PersonalDiscount);
                }
                else if (user.TotalSpentMoney >= 3000 && user.TotalSpentMoney < 6000)
                {
                    user.PersonalDiscount = 5;
                    Console.WriteLine("Test2 " + user.PersonalDiscount);
                }
                else if (user.TotalSpentMoney >= 15000)
                {
                    user.PersonalDiscount =10;
                    Console.WriteLine("Test3 " + user.PersonalDiscount);
                }
                Console.WriteLine("Test4 " + user.PersonalDiscount);
                context.TUsers.Update(user);
                context.SaveChanges();
            }
        }

        public List<Profile> GetProfileFriends(int id)
        {
            List<Profile> Friends = new List<Profile>();
            string path = $@"{BaseSettings.Default.SourcePath}\Users\{id}\";
            using (postgresContext context = new postgresContext())
            {
                //Загружаем таблицу TLogins
                List<TLogin> TLogins = context.TLogin.ToList();

                //Формируем список друзей
                if (File.Exists($@"{path}Friends.json"))
                {
                    List<int> IdFriends = JsonConvert.DeserializeObject<List<int>>(File.ReadAllText($@"{path}Friends.json"));
                    foreach (int idFriend in IdFriends)
                    {
                        Profile friend = SimpleFormProfile(TLogins.FirstOrDefault(u => u.Id == idFriend));
                        if (friend != null)
                            Friends.Add(friend);
                    }
                }
            }
            return Friends;
        }
    }
}