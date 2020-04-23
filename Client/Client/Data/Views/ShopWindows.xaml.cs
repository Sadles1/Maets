using Client.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using WinForms = System.Windows.Forms;
namespace Client
{
    /// <summary>
    /// Логика взаимодействия для ShopWindows.xaml
    /// </summary>

    public partial class ShopWindows : Window
    {
        static public List<Service.Profile> fr;

        public static List<Service.Product> mainfprofile = new List<Service.Product>();
        public static WCFServiceClient client;
        static public Korzina buyProduct;
        public Profile profile;
        List<Service.Product> products;
        int ishop = 0;
        DataProvider dp = new DataProvider();
        int idxg;



        public ShopWindows(string Login, string Password)
        {
            client = new WCFServiceClient(new System.ServiceModel.InstanceContext(new CallbackClass(this)), "NetTcpBinding_IWCFService");
            
            profile = client.Connect(Login, Password);

            if (profile != null)
            {
                products = client.GetProductTable().ToList();

                InitializeComponent();
                Loaded += Window_Loaded;
                Onlyforadmin.Visibility = Visibility.Hidden;
                if (profile.AccessRight >= 2) Onlyforadmin.Visibility = Visibility.Visible;

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
        private void Window_Initialized(object sender, EventArgs e)
        {
            fr = client.GetFriendRequests(profile.ID).ToList();
            if (fr.Count != 0)
            {
                Reaestfriends.Visibility = Visibility.Visible;
                howmanynewfriends.Text = "+" + fr.Count;
               
            }
            else Reaestfriends.Visibility = Visibility.Hidden;
            imMainImage.Source = dp.GetImageFromByte(profile.MainImage);
            tbLogin.Content = profile.Login;
            lbName.Content = profile.Name;
            Lvfriend.ItemsSource = profile.Friends;
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
            searchlibrary.DataContext = new LibraryViewModel(pr.Games.ToList());
            mylibrary.DataContext = searchlibrary.DataContext;
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
           // imMainImage.Source = dp.GetImageFromByte(profile.MainImage);
           // tbLogin.Content = pr.Login;
          //  lbName.Content = pr.Name;
            Lvfriend.ItemsSource = pr.Friends;
            Back.IsEnabled = false;
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
            searchproduct.Focus();
            searchproduct.DataContext = new ProductViewModel();
            Lvm.DataContext = searchproduct.DataContext;
            searchprofile.DataContext = new ProfileViewModel();
            AllUser.DataContext = searchprofile.DataContext;
            searchlibrary.DataContext = new LibraryViewModel(profile.Games.ToList());
            mylibrary.DataContext = searchlibrary.DataContext;
            //searchprofile.DataContext = new ProfileViewModel();
            //searchproduct.DataContext = new ProductViewModel();
            //AllUser.DataContext = new ProfileViewModel();
            //Lvm.DataContext = new ProductViewModel();
            // DataContext = new ProfileViewModel();

            //tbLogin.DataContext = new MyProfileVieModel(profile);
            // Lvmylibrary.DataContext = new MyProfileViewModel();

            Lvm.Visibility = Visibility.Hidden;

        }
        private void Buy_Click(object sender, RoutedEventArgs e)
        {
            buyProduct = new Korzina(profile);
            buyProduct.Left = this.Left;
            buyProduct.Top = this.Top;
            buyProduct.Show();
        }
        private void minirefresh()
        {
            searchproduct.DataContext = new ProductViewModel();
            Lvm.DataContext = searchproduct.DataContext;
            searchprofile.DataContext = new ProfileViewModel();
            AllUser.DataContext = searchprofile.DataContext;
        }
        private void BtnexitProfile_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MF = new MainWindow();
            MF.Show();
            Close();
        }

        private void BtnFakeProduct_Click(object sender, RoutedEventArgs e)
        {
            Service.Product id = new Service.Product();
            // Product Pr = new Product(id);
            // Pr.Show();
        }
        private void leftmouse(object sender, EventArgs e)
        {
            List<Service.Product> products = ShopWindows.client.GetProductTable().ToList();
            int i = Lvm.SelectedIndex;
            if (i != -1)
            {
                //Service.Product d = products.FirstOrDefault(o => o.Id ==i);
                Product Pr = new Product(products[i], profile);
                Pr.Left = this.Left;
                Pr.Top = this.Top;
                Pr.Show();
                MainWindow.shopWindows.Visibility = Visibility.Hidden;
            }

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
            List<Service.Profile> fr = profile.Friends.ToList();

            int i = Lvfriend.SelectedIndex;
            //Service.Profile d = fr.FirstOrDefault(o => o.ID ==i);
            if (i != -1)
            {
               // client.AddToBlacklist(MainWindow.shopWindows.profile.ID, fr[i].ID);
                client.RemoveFromBlacklist(MainWindow.shopWindows.profile.ID, fr[i].ID);
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
            Service.Product idx = profile.Games[mylibrary.SelectedIndex];
            string path = client.GetWayToGame(client.CheckProfile(profile.ID).ID, idx.Id) + "\\" + idx.Name + ".exe";
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
            idxg = Lvm.SelectedIndex;
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
                    Next.IsEnabled = true;
                    Back.IsEnabled = false;
                    //Back.Visibility = Visibility.Hidden;
                    //Next.Visibility = Visibility.Visible;
                }
            }

        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (products.Count > ishop + 3)
            {
                ishop++;
                ProductOne.Items.Clear();
                ProductTwo.Items.Clear();
                ProductFree.Items.Clear();
                ProductOne.Items.Add(products[ishop]);
                ProductTwo.Items.Add(products[ishop + 1]);
                ProductFree.Items.Add(products[ishop + 2]);
                if ((ishop + 2) == products.Count - 1)
                {
                    Next.IsEnabled = false;
                    Back.IsEnabled = true;
                    // Next.Visibility = Visibility.Hidden;
                    //Back.Visibility = Visibility.Visible;
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            client.Disconnect(profile.ID);
            client.Close();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
          refresh(ShopWindows.client.CheckActiveProfile(profile.ID));
          //  MyProfileVieModel phone = (MyProfileVieModel)this.Resources["nexusPhone"];
           // phone.Company = "LG"; // Меняем с Google на LG
        }

     

        private void FilterUser_FocusableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            
        }

        private void FilterUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FilterUser.Text.Count() !=0) AllUser.Visibility = Visibility.Visible;
          
        }

        private void AllUser_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            List<Service.Profile> fr = client.GetAllUsers().ToList() ;

            int i = AllUser.SelectedIndex;
            //Service.Profile d = fr.FirstOrDefault(o => o.ID ==i);
            if (i != -1)
            { 
                // client.AddToBlacklist(MainWindow.shopWindows.profile.ID, fr[i].ID);
               // client.RemoveFromBlacklist(MainWindow.shopWindows.profile.ID, fr[i].ID);
                profilefriend r = new profilefriend(ShopWindows.client.CheckProfile(fr[i].ID));
                r.Left = this.Left;
                r.Top = this.Top;
                r.Show();
                MainWindow.shopWindows.Visibility = Visibility.Hidden;
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
            s += @"\" + idg.Name;

            DownloadGame(s, idg.Id, profile.ID);
            Play.Visibility = Visibility.Visible;
            Download.Visibility = Visibility.Hidden;

        }

        async public void DownloadGame(string path, int idGame, int idUser)
        {
            Play.IsEnabled = false;
            await Task.Run(() =>
            {
                try
                {
                    DownloadServiceClient download = new DownloadServiceClient("NetTcpBinding_IDownloadService");
                    using (Stream stream = download.DownloadProduct(idGame, idUser, path))
                    {
                        byte[] buffer = new byte[16 * 1024];
                        byte[] data;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            int read;
                            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                double a = ((FileStream)stream).Position / ((FileStream)stream).Length;
                                File.WriteAllText(@"D:\test.txt",Convert.ToString(a));
                                ms.Write(buffer, 0, read);
                            }
                            data = ms.ToArray();
                        }
                        File.WriteAllBytes($@"{path}.zip", data);
                    }
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    ZipFile.ExtractToDirectory($@"{path}.zip", path);
                    File.Delete($@"{path}.zip");
                    System.Windows.MessageBox.Show("Загрузка завершена");

                    download.Close();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            });
            Play.IsEnabled = true;
        }


        private void Play_Click(object sender, RoutedEventArgs e)
        {
            Service.Product idx = profile.Games[mylibrary.SelectedIndex];
            string file = client.GetWayToGame(profile.ID, idx.Id) + @"\" + idx.Name + ".exe";
            if (file!= null && File.Exists(file))
                System.Diagnostics.Process.Start(file);
            else WinForms.MessageBox.Show("Файл не найден");
        }

        private void AddGame_Click(object sender, RoutedEventArgs e)
        {
            NewGameAdd ng = new NewGameAdd();
            ng.Left = Left;
            ng.Top = Top;
            Visibility = Visibility.Hidden;
            ng.Show();
        }
    }
}