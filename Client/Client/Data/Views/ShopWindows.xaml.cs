using Client.Data.Models;
using Client.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using WinForms = System.Windows.Forms;
namespace Client
{
    /// <summary>
    /// Логика взаимодействия для ShopWindows.xaml
    /// </summary>

    public partial class ShopWindows : Window
    {
        static public List<Service.Profile> r;
        public static int isender=-1;
        List<byte[]> screns = new List<byte[]>(); 
        List<ModelImage> modelProducts;
        static public List<Service.Profile> fr;
        static public List<UserMessage> frmail;
        public static List<ModelProductCart> mainfprofile = new List<ModelProductCart>();
        public static WCFServiceClient client;
        static public Korzina buyProduct;
        public Profile profile;
        List<Service.Product> products;
        List<Service.Product> productsmoderation;
        int ishop = 0;
        List<Service.Product> w;
        List<Service.Profile> p;
        static public string imagesmaets;
        DataProvider dp = new DataProvider();
        List<Service.Profile> profilesq;


        public ShopWindows(string Login, string Password)
        {
            this.Title = "Maets";
            string data1 = Environment.CurrentDirectory + "\\Content\\maets.cur";
            var cursor = new System.Windows.Input.Cursor(data1);
            this.Cursor = cursor;
            this.Title = "Maets";
            client = new WCFServiceClient(new System.ServiceModel.InstanceContext(new CallbackClass(this)), "NetTcpBinding_IWCFService");
            
            client.Connect(Login, Password);
            profile = client.ActiveProfile(Login, Password);
            if (profile != null)
            {
                products = client.GetProductTable().ToList();
                profilesq = client.GetAllUsers().ToList();
                InitializeComponent();
                Loaded += Window_Loaded;
                Onlyforadmin.Visibility = Visibility.Hidden;
                if (profile.AccessRight >= 3)
                {
                     productsmoderation = client.GetModerationProduct(profile.ID).ToList();
                    for(int i=0; i<productsmoderation.Count;i++)
                    {
                        Moderationproduct.Items.Add(productsmoderation[i]);
                        
                       // listuser.Items.Add(i);
                    }
                    Onlyforadmin.Visibility = Visibility.Visible;
                    //if(profile.AccessRight==4)
                    //{
                    //    listuser.ItemsSource = products;
                    //    for(idxg=1;idxg<5;idxg++)
                    //    {
                    //      // listrigth;
                    //    }
                    //}
                    
                    

                    if(profile.AccessRight>=2)
                        {
                            AddGame.Visibility = Visibility.Visible;
                        }
                }
                foreach (Service.Product p in profile.Games.ToList())
                {
                    mylibrary.Items.Add(p);
                }

            }

            else
                throw new Exception("Ошибка подключения");
        }
        public void reqestrefresh(Service.Profile reqestprof)
        {
            fr.Add(reqestprof);
            if (fr.Count != 0)
            {
                Reaestfriends.Visibility = Visibility.Visible;
                howmanynewfriends.Text = "+" + fr.Count;

            }
        }

        public void friendsonline(int id)
        {
            
            Lvfriend.Items.Clear();
            for (int i = 0; i < r.Count; i++)
            {
                if (r[i].ID == id) r[i].status = "Online";
                Lvfriend.Items.Add(r[i]);
            }
        }
        public void friendsonline(Service.Profile id)
        {
            Lvfriend.Items.Clear();
            for (int i = 0; i < r.Count; i++)
            {
                if (r[i].ID == id.ID) r[i].status = "Online";
                Lvfriend.Items.Add(r[i]);
            }
        }
        public void friendsnew(Service.Profile id)
        {
            r.Add(id);
            Lvfriend.Items.Add(id); 
        }
        public void friendsdel(int id)
        {
            for (int i = 0; i < r.Count; i++)
            {
                if (r[i].ID == id)
                {
                    Lvfriend.Items.RemoveAt(i);
                    r.RemoveAt(i);
                }
                
            }
        }


        public void friendsoffline(int id)
        {
            Lvfriend.Items.Clear();

            for (int i = 0; i < r.Count; i++)
            {
                if (r[i].ID == id) r[i].status = "Offline";
                Lvfriend.Items.Add(r[i]);
            }
        }



        public void messagerefresh1()
        {
            if(isender!=-1)
            for (int i = 0; i < frmail.Count; i++)
            {
                if (frmail[i].IDSender == isender)
                {
                    frmail.RemoveAt(i);
                        if(frmail.Count!=0)
                    howmanymail.Text = "+" + frmail.Count;
                        else
                        {
                            MailCount.Visibility = Visibility.Hidden;

                            howmanymail.Text = "";
                        }
                    }
            }
        }
            public void messagerefresh()
        {
            if (frmail.Count != 0)
            {
                MailCount.Visibility = Visibility.Visible;
                howmanymail.Text = "+" + frmail.Count;
            }
            else
            {
                MailCount.Visibility = Visibility.Hidden;
                howmanymail.Text = "+" + frmail.Count;
            }
        }
        private void Window_Initialized(object sender, EventArgs e)
        {
            r = profile.Friends.ToList();
            mail.Text = profile.Mail;
            telephone.Text = profile.Telephone;
            fr = client.GetFriendRequests(profile.ID).ToList();
            frmail = client.GetNewMessages(profile.ID).ToList();

            if (fr.Count != 0)
            {
                Reaestfriends.Visibility = Visibility.Visible;
                howmanynewfriends.Text = "+" + fr.Count;

            }
            else Reaestfriends.Visibility = Visibility.Hidden;
            imMainImage.Source = dp.GetImageFromByte(profile.MainImage);
            lvimMainImage.Items.Add(profile.MainImage);

            tbLogin.Text = profile.Login;
            lbName.Text = profile.Name;
            for (int i = 0; i < r.Count; i++)
            {
                Lvfriend.Items.Add(r[i]);
            }

            Back.IsEnabled = false;
            // ProductOne.DataContext = products[ishop];
            ProductOne.Items.Add(products[ishop]);
            ProductTwo.Items.Add(products[ishop + 1]);
            ProductFree.Items.Add(products[ishop + 2]);
            // Lvmylibrary.ItemsSource = profile.Games;
            btnMoney.Content = Convert.ToString(profile.Money) + " р";
            // lbLogin.Content = profile.AccessRight;
            //DataContext = new ProductViewModel();


        }
        private void refresh(Service.Profile pr)
        {
            // searchproduct.DataContext = new ProductViewModel();
            //Lvm.DataContext = searchproduct.DataContext;
            //  searchprofile.DataContext = new ProfileViewModel();
            // AllUser.DataContext = searchprofile.DataContext;
            fr = client.GetFriendRequests(profile.ID).ToList();
            if (fr.Count != 0)
            {
                Reaestfriends.Visibility = Visibility.Visible;
                howmanynewfriends.Text = "+" + fr.Count;


            }
            else Reaestfriends.Visibility = Visibility.Hidden;

            mylibrary.Items.Clear();
            foreach (Service.Product p in pr.Games.ToList())
            {
                mylibrary.Items.Add(p);
            }


            //   searchlibrary.DataContext = new LibraryViewModel(pr.Games.ToList());
            //  mylibrary.DataContext = searchlibrary.DataContext;
            if (mainfprofile.Count != 0)
            {
                KorzinaCount.Visibility = Visibility.Visible;
                howmanyproduct.Text = Convert.ToString(mainfprofile.Count);
            }
            else
            {
                KorzinaCount.Visibility = Visibility.Hidden;
                howmanyproduct.Text = Convert.ToString(0);

            }
            //   imMainImage.Source = dp.GetImageFromByte(profile.MainImage);
            // tbLogin.Content = pr.Login;
            //  lbName.Content = pr.Name;
            
           // Lvmylibrary.ItemsSource = pr.Games;
            btnMoney.Content = Convert.ToString(pr.Money) + " р";
            profile = pr;

        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            Close();
        }

        private void TbExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnFull_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState != WindowState.Maximized)
                WindowState = WindowState.Maximized;
            else
            {
                WindowState = WindowState.Normal;
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            imagesmaets = Environment.CurrentDirectory + @"\Content\";
            searchproduct.Focus();
            foreach(Service.Product produc in products)
            {
                ModelProduct mp = new ModelProduct();
                mp = mp.MakeModelProduct(produc);
                Lvm.Items.Add(mp);
            }
                foreach (Service.Profile produc in profilesq)
                {
                    ModelProfile mp = new ModelProfile();
                    mp = mp.MakeModelProfile(produc);
                    AllUser.Items.Add(mp);
                }
         
            Lvm.Visibility = Visibility.Hidden;

        }
        private void Buy_Click(object sender, RoutedEventArgs e)
        {
            buyProduct = new Korzina(profile);
            buyProduct.Left = this.Left;
            buyProduct.Top = this.Top;
            buyProduct.Show();
        }
        private void refreshfriends()
        {
            Lvfriend.Items.Clear();
            for (int i = 0; i < r.Count; i++)
            {
                Lvfriend.Items.Add(r[i]);
            }
        }
        private void BtnexitProfile_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MF = new MainWindow();
            MF.Show();
            Close();
        }

        private void leftmouse(object sender, EventArgs e)
        {
            Product Pr;
            int i = Lvm.SelectedIndex;
            if (products.Count == Lvm.Items.Count)
            {



                if (i != -1)
                {
                    //Service.Product d = products.FirstOrDefault(o => o.Id ==i);
                     Pr = new Product(products[i], profile);
                    Pr.Left = this.Left;
                    Pr.Top = this.Top;
                    Pr.Show();
                }
            }
            else
            {
                 Pr = new Product(w[i], profile);
                Pr.Left = this.Left;
                Pr.Top = this.Top;
                Pr.Show();
            }
            
            MainWindow.shopWindows.Visibility = Visibility.Hidden;

        }
        private void leftmouse1(object sender, EventArgs e)
        {
            Product Pr = new Product(products[ishop], profile);
            Pr.Left = this.Left;
            Pr.Top = this.Top;
            Pr.Show();
            MainWindow.shopWindows.Visibility = Visibility.Hidden;
        }
        private void leftmouse2(object sender, EventArgs e)
        {
            Product Pr = new Product(products[ishop + 1], profile);
            Pr.Left = this.Left;
            Pr.Top = this.Top;
            Pr.Show();
            MainWindow.shopWindows.Visibility = Visibility.Hidden;
        }
        private void leftmouse3(object sender, EventArgs e)
        {
            Product Pr = new Product(products[ishop + 2], profile);
            Pr.Left = this.Left;
            Pr.Top = this.Top;
            Pr.Show();
            MainWindow.shopWindows.Visibility = Visibility.Hidden;
        }
        private void leftmousefriend(object sender, EventArgs e)
        {
            List<Service.Profile> fr = ShopWindows.r.ToList();

            int i = Lvfriend.SelectedIndex;
            //Service.Profile d = fr.FirstOrDefault(o => o.ID ==i);
            if (i != -1)
            {
               // client.AddToBlacklist(MainWindow.shopWindows.profile.ID, fr[i].ID);
              //  client.RemoveFromBlacklist(MainWindow.shopWindows.profile.ID, fr[i].ID);
                profilefriend r = new profilefriend(ShopWindows.client.CheckProfile(fr[i].ID));
                r.Left = this.Left;
                r.Top = this.Top;
                r.Show();
                MainWindow.shopWindows.Visibility = Visibility.Hidden;
            }

        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void leftmouse_mylibrary(object sender, EventArgs e)
        {
            Screenshoot.Items.Clear();
            Screenshoot1.Items.Clear();
            Screenshoot2.Items.Clear();
            Screenshoot3.Items.Clear();
            Screenshoot4.Items.Clear();
            Screenshoot5.Items.Clear();
            screns.Clear();
            Service.Product idx = profile.Games[mylibrary.SelectedIndex];
            
            screns.Add(idx.MainImage);
            List<byte[]> d = ShopWindows.client.GetGameImages(idx.Id).ToList();

            for (int h = 0; h < d.Count; h++)
            {
                screns.Add(d.ToList()[h]);
                modelProducts = new List<ModelImage>();
            }
            foreach (byte[] product in screns)
            {
                ModelImage modelProduct = new ModelImage();
                modelProducts.Add(modelProduct.MakeModelImage(product));
            }
            Screenshoot.Items.Add(modelProducts[0]);
            Screenshoot1.Items.Add(modelProducts[0 + 1]);
            Screenshoot2.Items.Add(modelProducts[0 + 2]);
            Screenshoot3.Items.Add(modelProducts[0 + 3]);
            Screenshoot4.Items.Add(modelProducts[0 + 4]);
            Screenshoot5.Items.Add(modelProducts[0 + 5]);
            
            string path = GetPathToGame(idx.Id) + "\\" + idx.Name + ".exe";
            if (path == null || !File.Exists(path) )
            {
                Download.Visibility = Visibility.Visible;
                Play.Visibility = Visibility.Hidden;
            }
            else
            {
                Download.Visibility = Visibility.Hidden;
                Play.Visibility = Visibility.Visible;
            }
            int i = mylibrary.SelectedIndex;
            if (i != -1)
            {
                tbgamename.Visibility = Visibility.Visible;
                tbgamename.Text = profile.Games[i].Name;
            }

        }

        private void Lvm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WinForms.MessageBox.Show("Вы запустили игру " + tbgamename.Text);
        }

        private void TextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Lvm.Visibility = Visibility.Visible;
            btnSearchBack.Visibility = Visibility.Visible;
            grMainShop.Visibility = Visibility.Hidden;

        }

        private void BtnSearchBack_Click(object sender, RoutedEventArgs e)
        {
            Lvm.Visibility = Visibility.Hidden;
            btnSearchBack.Visibility = Visibility.Hidden;
            grMainShop.Visibility = Visibility.Visible;
            FilterProductText.Text = "";
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Next.IsEnabled = true;
            Back.IsEnabled = true;
            if (ishop >= 0)
            {
                ishop--;
                ProductOne.Items.Clear();
                ProductTwo.Items.Clear();
                ProductFree.Items.Clear();
                ProductOne.Items.Add(products[ishop]);
                ProductTwo.Items.Add(products[ishop + 1]);
                ProductFree.Items.Add(products[ishop + 2]);
                if (ishop == 0)
                {
                    Back.IsEnabled = false;
                    //Back.Visibility = Visibility.Hidden;
                    //Next.Visibility = Visibility.Visible;
                }
            }

        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            Next.IsEnabled = true;
            Back.IsEnabled = true;
            if (products.Count > ishop + 3)
            {
                ishop++;
                ProductOne.Items.Clear();
                ProductTwo.Items.Clear();
                ProductFree.Items.Clear();
                ProductOne.Items.Add(products[ishop]);
                ProductTwo.Items.Add(products[ishop + 1]);
                ProductFree.Items.Add(products[ishop + 2]);
                if ((ishop) == products.Count-3)
                {
                    Next.IsEnabled = false;
                    // Next.Visibility = Visibility.Hidden;
                    //Back.Visibility = Visibility.Visible;
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //this.Close();
            client.Disconnect(profile.ID);
            client.Close();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
          refresh(ShopWindows.client.CheckActiveProfile(profile.ID));
        }

     

        private void FilterUser_FocusableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            
        }

        private void FilterUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FilterUser.Text != "")
            {
                AllUser.Items.Clear();
                p = client.GetProfileByFilter(FilterUser.Text).ToList();
                foreach (Service.Profile produc in p)
                {
                    ModelProfile mp = new ModelProfile();
                    mp = mp.MakeModelProfile(produc);
                    AllUser.Items.Add(mp);
                }
            }
            else
            {
                AllUser.Items.Clear();
                foreach (Service.Profile produc in profilesq)
                {
                    ModelProfile mp = new ModelProfile();
                    mp = mp.MakeModelProfile(produc);
                    AllUser.Items.Add(mp);
                }
            }

        }

        private void AllUser_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            profilefriend r;
            int i = AllUser.SelectedIndex;
            if (AllUser.Items.Count == profilesq.Count)

            {
                if (profilesq[i].ID != profile.ID)
                {
                    //Service.Profile d = fr.FirstOrDefault(o => o.ID ==i);
                    if (i != -1)
                    {
                        // client.AddToBlacklist(MainWindow.shopWindows.profile.ID, fr[i].ID);
                        // client.RemoveFromBlacklist(MainWindow.shopWindows.profile.ID, fr[i].ID);
                        r = new profilefriend(ShopWindows.client.CheckProfile(profilesq[i].ID));
                        r.Left = this.Left;
                        r.Top = this.Top;
                        r.Show();
                        MainWindow.shopWindows.Visibility = Visibility.Hidden;
                    }
                }
                else myprofiletab.Focus();
            }
            else
            {
                if (p[i].ID != profile.ID)
                {
                    if (i != -1)
                {
                    // client.AddToBlacklist(MainWindow.shopWindows.profile.ID, fr[i].ID);
                    // client.RemoveFromBlacklist(MainWindow.shopWindows.profile.ID, fr[i].ID);
                    r = new profilefriend(ShopWindows.client.CheckProfile(p[i].ID));
                    r.Left = this.Left;
                    r.Top = this.Top;
                    r.Show();
                    MainWindow.shopWindows.Visibility = Visibility.Hidden;
                }
                }
                else myprofiletab.Focus();
            }
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
           // ShowDialog()
        }

        private void Newfriendswho_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Reqestfriends rf = new Reqestfriends();
            rf.Top = Top;
            rf.Left = Left;
            rf.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Reqestfriends rf = new Reqestfriends();
            rf.Top = Top;
            rf.Left = Left;
            rf.Show();
        }

        private void Download_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog openFileDialog = new FolderBrowserDialog();
            Service.Product idg = profile.Games[mylibrary.SelectedIndex];
            //   openFileDialog.Filter = "All files (*.*)";
            // openFileDialog.CheckFileExists = false;
            openFileDialog.ShowDialog();
            string s = openFileDialog.SelectedPath;
            if (s != "")
            {
                s += @"\" + idg.Name;
                if (!File.Exists(s + "\\" + idg.Name + ".exe"))
                {
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                    worker.WorkerReportsProgress = true;
                    worker.ProgressChanged += worker_ProgressChanged;
                    worker.DoWork += worker_DoWork;
                    DownloadProduct downloadProduct = new DownloadProduct();
                    downloadProduct.path = s;
                    downloadProduct.idUser = profile.ID;
                    downloadProduct.idGame = idg.Id;
                    SetPathToGame(s, idg.Id);
                    worker.RunWorkerAsync(downloadProduct);
                    MainWindow.shopWindows.Play.Visibility = Visibility.Visible;
                    MainWindow.shopWindows.Download.Visibility = Visibility.Hidden;
                    //}
                    // dp.Download();
                }
                else
                {
                    UpdatePathToGame(s, idg.Id);
                    System.Windows.MessageBox.Show("По данному пути мы уже нашли эту игру, скачивать ничего не нужно");
                    Download.Visibility = Visibility.Hidden;
                    Play.Visibility = Visibility.Visible;
                    //переназначить путь
                }
            }
        }

        private void SetPathToGame(string path, int idProduct)
        {
            string pathToJson = $@"{Environment.CurrentDirectory}\Settings\GamesPath.json";
            List<Tuple<int, string>> GamesPath = File.Exists(pathToJson) ? JsonConvert.DeserializeObject<List<Tuple<int, string>>>(File.ReadAllText(pathToJson)) : new List<Tuple<int, string>>();
            GamesPath.Add(Tuple.Create(idProduct, path));
            File.WriteAllText(pathToJson, JsonConvert.SerializeObject(GamesPath));
        }

        private void UpdatePathToGame(string path, int idProduct)
        {
            string pathToJson = $@"{Environment.CurrentDirectory}\Settings\GamesPath.json";
            List<Tuple<int, string>> GamesPath = File.Exists(pathToJson) ? JsonConvert.DeserializeObject<List<Tuple<int, string>>>(File.ReadAllText(pathToJson)) : new List<Tuple<int, string>>();
            Tuple<int, string> tuple = GamesPath.FirstOrDefault(u => u.Item1 == idProduct);
            GamesPath.Remove(tuple);
            GamesPath.Add(Tuple.Create(idProduct, path));
            File.WriteAllText(pathToJson, JsonConvert.SerializeObject(GamesPath));
        }

        private string GetPathToGame(int idProduct)
        {
            string pathToJson = $@"{Environment.CurrentDirectory}\Settings\GamesPath.json";
            List<Tuple<int, string>> messages = File.Exists(pathToJson) ? JsonConvert.DeserializeObject<List<Tuple<int, string>>>(File.ReadAllText(pathToJson)) : new List<Tuple<int, string>>();
            foreach (Tuple<int, string> tuple in messages)
            {
                if (tuple.Item1 == idProduct)
                {
                    return tuple.Item2;
                }
            }
            return null;
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            Service.Product idx = profile.Games[mylibrary.SelectedIndex];
            string file = GetPathToGame(idx.Id) + @"\" + idx.Name + ".exe";
            if (file != null && File.Exists(file))
                System.Diagnostics.Process.Start(file);
            else WinForms.MessageBox.Show("Файл не найден");
        }
        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            dwnload.Value = e.ProgressPercentage;
            dnmtext.Text = (string)e.UserState;
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            DownloadProduct downloadProduct = (DownloadProduct)e.Argument;
            var worker = sender as BackgroundWorker;
            worker.ReportProgress(0, String.Format("Processing Iteration 1"));

            DownloadServiceClient download = new DownloadServiceClient("NetTcpBinding_IDownloadService");
            long DownloadSize;

            using (FileStream outputStream = new FileStream($@"{downloadProduct.path}.zip", FileMode.OpenOrCreate, FileAccess.Write))
            {
               
                if (File.Exists($@"{downloadProduct.path}.zip"))
                {
                    FileInfo file = new FileInfo($@"{downloadProduct.path}.zip");
                    DownloadSize = file.Length;
                    outputStream.Position = DownloadSize;
                }
                else
                    DownloadSize = 0;

                long FileSize = download.GetFileSize(downloadProduct.idGame);
                const int bufferSize = 16 * 1024;

                byte[] buffer = new byte[bufferSize];
                using (Stream stream = download.DownloadProduct(downloadProduct.idGame, downloadProduct.idUser, downloadProduct.path, DownloadSize))
                {

                    int bytesRead = stream.Read(buffer, 0, bufferSize);
                    while (bytesRead > 0)
                    {
                        FileInfo file = new FileInfo($@"{downloadProduct.path}.zip");
                        DownloadSize = file.Length;
                        worker.ReportProgress(Convert.ToInt32((double)DownloadSize / (double)FileSize * (double)100),String.Format("Загружено {0} %", Convert.ToInt32((double)DownloadSize / (double)FileSize * (double)100)));
                        outputStream.Write(buffer, 0, bytesRead);
                        bytesRead = stream.Read(buffer, 0, bufferSize);
                        outputStream.Flush();
                    }
                    outputStream.Close();
                    worker.ReportProgress(100);
                }
            }
            ZipFile.ExtractToDirectory($@"{downloadProduct.path}.zip", downloadProduct.path);
            File.Delete($@"{downloadProduct.path}.zip");

            download.Close();
        }
        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            System.Windows.MessageBox.Show("Загружено");
            dwnload.Value = 0;
            dnmtext.Text = "";
            Download.Visibility = Visibility.Hidden;
            Play.Visibility = Visibility.Visible;
        }

        private void AddGame_Click(object sender, RoutedEventArgs e)
        {
            NewGameAdd ng = new NewGameAdd();
            ng.Left = Left;
            ng.Top = Top;
            Visibility = Visibility.Hidden;
            ng.Show();
        }

        private void Refreshmoderation_Click(object sender, RoutedEventArgs e)
        {

            
        }

        private void BtnYes_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int i = Moderationproduct.SelectedIndex;
            client.ChangeModerationStatus(profile.ID, productsmoderation[i].Id, true);
            Moderationproduct.Items.RemoveAt(i);
        }

        private void BtnNo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int i = Moderationproduct.SelectedIndex;
            client.ChangeModerationStatus(profile.ID, productsmoderation[i].Id, false);
            Moderationproduct.Items.RemoveAt(i);

        }

        private void Changeprofile_Click(object sender, RoutedEventArgs e)
        {
            tbLogin.Visibility = Visibility.Hidden;
            lbName.Visibility = Visibility.Hidden;
            mail.Visibility = Visibility.Hidden;
            telephone.Visibility = Visibility.Hidden;
            tbLoginnew_hint.Text = tbLogin.Text;
            tbtelephonenew_hint.Text = telephone.Text;
            tbmailnew_hint.Text = mail.Text;
            lbNamenew_hint.Text = lbName.Text;
            ChangeprofileSave.Visibility = Visibility.Visible;
            Changeprofile.Visibility = Visibility.Hidden;
            newprofilechange.Visibility = Visibility.Visible;
            btnBack.Visibility = Visibility.Visible;

        }

        private void Tbtelephonenew_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(tbtelephonenew.Text!="")
                {

                tbtelephonenew_hint.Visibility = Visibility.Hidden;
                }
                else tbtelephonenew_hint.Visibility = Visibility.Visible;
        }

        private void Tbmailnew_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbmailnew.Text != "")
            {

                tbmailnew_hint.Visibility = Visibility.Hidden;
            }
            else tbmailnew_hint.Visibility = Visibility.Visible;

        }

        private void LbNamenew_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (lbNamenew.Text != "")
            {

                lbNamenew_hint.Visibility = Visibility.Hidden;
            }
            else lbNamenew_hint.Visibility = Visibility.Visible;
        }

        private void TbLoginnew_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbLoginnew.Text != "")
            {

                tbLoginnew_hint.Visibility = Visibility.Hidden;
            }
            else tbLoginnew_hint.Visibility = Visibility.Visible;
        }

        private void ChangeprofileSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tbLoginnew.Text == "" && tbtelephonenew.Text==""&& tbmailnew.Text=="" && tbmailnew.Text == "" && lbNamenew.Text == "")
                    throw new Exception("Вы не заполнили ни одного поля!");
                Profile newprof = new Profile();

                newprof.ID = profile.ID;
                    newprof.Telephone = tbtelephonenew.Text;
                    newprof.Name = lbNamenew.Text;
                    newprof.Login = tbLoginnew.Text;
                    newprof.Mail = tbmailnew.Text;
                if (tbLoginnew.Text != "")
                    tbLogin.Text = newprof.Login;
                if (lbNamenew.Text != "")
                    lbName.Text = newprof.Name;
                if (tbmailnew.Text != "")
                    mail.Text = newprof.Mail;
                if (tbtelephonenew.Text != "")
                    telephone.Text = newprof.Telephone;
                client.ChangeProfileInformation(newprof);
                newprofilechange.Visibility = Visibility.Hidden;
                btnBack.Visibility = Visibility.Hidden;
                ChangeprofileSave.Visibility = Visibility.Hidden;
                Changeprofile.Visibility = Visibility.Visible;
                tbLogin.Visibility = Visibility.Visible;
                lbName.Visibility = Visibility.Visible;
                mail.Visibility = Visibility.Visible;
                telephone.Visibility = Visibility.Visible;

            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message,"Предупреждение");
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            newprofilechange.Visibility = Visibility.Hidden;
            btnBack.Visibility = Visibility.Hidden;
            ChangeprofileSave.Visibility = Visibility.Hidden;
            Changeprofile.Visibility = Visibility.Visible;
            tbLogin.Visibility = Visibility.Visible;
            lbName.Visibility = Visibility.Visible;
            mail.Visibility = Visibility.Visible;
            telephone.Visibility = Visibility.Visible;
        }
        List<byte[]> gamescreen = new List<byte[]>();
        private ModelImage get_image_from_PC()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            ModelImage modelProduct = new ModelImage();
            string s = openFileDialog.FileName;

            // System.Windows.MessageBox.Show(s);
            if (s != "")
            {
                byte[] buffer = dp.GetByteFromImage(s);
                gamescreen.Add(buffer);


                modelProduct = modelProduct.MakeModelImage(buffer);
                return modelProduct;
            }
            else
            {
                modelProduct.Id = -1;
                return modelProduct;
            }
        }

        private void ImMainImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
            
            ModelImage modelProduct= get_image_from_PC();
            if (modelProduct.Id != -1)
            {
                lvimMainImage.Items.Clear();
                imMainImage.Visibility = Visibility.Hidden;
                lvimMainImage.Items.Add(modelProduct);
                client.changeProfileImage(profile.ID, modelProduct.MainImage);
            }
            else lvimMainImage.SelectedItem = null;
            
        }

        private void Changerigth_Click(object sender, RoutedEventArgs e)
        {
            //ComboBoxItem selectedItem = (ComboBoxItem)listrigth.SelectedItem;
            //int i = listrigth.SelectedIndex;
            //int j = listuser.;
        }

        private void Btnmewmessage_Click(object sender, RoutedEventArgs e)
        {
            messagefriends mf = new messagefriends();
            mf.Top = Top;
            mf.Left = Left;
            mf.Show();
        }

        private void Refreshmail_Click(object sender, RoutedEventArgs e)
        {
            messagerefresh1();
        }

        private void Resetpassword_Click(object sender, RoutedEventArgs e)
        {
            Changepassword cp = new Changepassword(profile.Login);
            cp.Show();
        }

        private void FilterProductText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FilterProductText.Text != "")
            {
                Lvm.Items.Clear();
               w = client.GetProductByFilter(FilterProductText.Text).ToList();
                foreach (Service.Product produc in w )
                {
                    ModelProduct mp = new ModelProduct();
                    mp = mp.MakeModelProduct(produc);
                    Lvm.Items.Add(mp);
                }
            }
            else
            {
                Lvm.Items.Clear();
                foreach (Service.Product produc in products)
                {
                    ModelProduct mp = new ModelProduct();
                    mp = mp.MakeModelProduct(produc);
                    Lvm.Items.Add(mp);
                }
            }
        }

        private void Reefreshf_Click(object sender, RoutedEventArgs e)
        {
            refreshfriends();
        }

        private void Screenshoot1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Screenshoot.Items.Clear();
            Screenshoot.Items.Add(modelProducts[1]);
        }

        private void Screenshoot2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Screenshoot.Items.Clear();
            Screenshoot.Items.Add(modelProducts[2]);
        }

        private void Screenshoot3_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Screenshoot.Items.Clear();
            Screenshoot.Items.Add(modelProducts[3]);
        }

        private void Screenshoot4_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Screenshoot.Items.Clear();
            Screenshoot.Items.Add(modelProducts[4]);
        }

        private void Screenshoot5_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Screenshoot.Items.Clear();
            Screenshoot.Items.Add(modelProducts[5]);
        }

        private void Mylibrary_Selected(object sender, RoutedEventArgs e)
        {
           
        }

        private void Delgame_Click(object sender, RoutedEventArgs e)
        {
            Service.Product idx = profile.Games[mylibrary.SelectedIndex];
            string path = GetPathToGame(idx.Id) + "\\" + idx.Name + ".exe";
            if (path != null && File.Exists(path))
            {
                
                    File.Delete(path);
                Play.Visibility = Visibility.Hidden;
                Download.Visibility = Visibility.Visible;
            }
        }

        private void Mylibrary_Selected_1(object sender, RoutedEventArgs e)
        {
            
        }

        private void Mylibrary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mylibrary.SelectedIndex != -1)
            {
                delgame.Visibility = Visibility.Visible;
                
            }
            else
            {
                delgame.Visibility = Visibility.Hidden;
            }
        }

        private void Mylibrary_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}