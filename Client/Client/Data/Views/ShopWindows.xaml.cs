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
using System.Windows.Media;
using WinForms = System.Windows.Forms;
namespace Client
{
    /// <summary>
    /// Логика взаимодействия для ShopWindows.xaml
    /// </summary>

    public partial class ShopWindows : Window
    {
        static public List<Service.Profile> fr;

        public static List<ModelProductCart> mainfprofile = new List<ModelProductCart>();
        public static WCFServiceClient client;
        static public Korzina buyProduct;
        public Profile profile;
        List<Service.Product> products;
        List<Service.Product> productsmoderation;
        int ishop = 0;

        static public string imagesmaets;
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
                if (profile.AccessRight >= 3)
                {
                     productsmoderation = client.GetModerationProduct(profile.ID).ToList();
                    for(int i=0; i<productsmoderation.Count;i++)
                    {
                        Moderationproduct.Items.Add(productsmoderation[i]);
                    }
                    Onlyforadmin.Visibility = Visibility.Visible;
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
        private void Window_Initialized(object sender, EventArgs e)
        {
            mail.Text = profile.Mail;
            telephone.Text = profile.Telephone;
            fr = client.GetFriendRequests(profile.ID).ToList();
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
         //   imMainImage.Source = dp.GetImageFromByte(profile.MainImage);
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
            imagesmaets = Environment.CurrentDirectory + @"\Content\";
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
            if (s != "")
            {
                s += @"\" + idg.Name;

                //if (openFileDialog.ShowDialog() == true)
                //{
                // profile.Games[0].Path = s;
                //    FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                // System.Windows.MessageBox.Show(s);
                DownloadGame(s, idg.Id, profile.ID);
                MainWindow.shopWindows.Play.Visibility = Visibility.Visible;
                MainWindow.shopWindows.Download.Visibility = Visibility.Hidden;
                //}
                // dp.Download();
            }
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            Service.Product idx = profile.Games[mylibrary.SelectedIndex];
            string file = client.GetWayToGame(profile.ID, idx.Id) + @"\" + idx.Name + ".exe";
            if (file!= null && File.Exists(file))
                System.Diagnostics.Process.Start(file);
            else WinForms.MessageBox.Show("Файл не найден");
        }

        async public void DownloadGame(string path, int idGame, int idUser)
        {
            Play.IsEnabled = false;
            await Task.Run(() =>
            {
                try
                {
                    DownloadServiceClient download = new DownloadServiceClient("NetTcpBinding_IDownloadService");
                    long DownloadSize;

                    using (FileStream outputStream = new FileStream($@"{path}.zip", FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        if (File.Exists($@"{path}.zip"))
                        {
                            FileInfo file = new FileInfo($@"{path}.zip");
                            DownloadSize = file.Length;
                            outputStream.Position = DownloadSize;
                        }
                        else
                            DownloadSize = 0;




                        long FileSize = download.GetFileSize(idGame);
                        const int bufferSize = 16 * 1024;

                        byte[] buffer = new byte[bufferSize];
                        using (Stream stream = download.DownloadProduct(idGame, idUser, path, DownloadSize))
                        {

                            int bytesRead = stream.Read(buffer, 0, bufferSize);
                            while (bytesRead > 0)
                            {
                                outputStream.Write(buffer, 0, bytesRead);
                                bytesRead = stream.Read(buffer, 0, bufferSize);
                                DownloadSize += buffer.Length;
                                outputStream.Flush();
                            }
                            outputStream.Close();

                        }
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
                if (tbLoginnew.Text == "" && tbmailnew.Text=="" && tbmailnew.Text == "" && lbNamenew.Text == "")
                    throw new Exception("Вы не заполнили ни одного поля!");
                Profile newprof = new Profile();
                newprof.ID = profile.ID;
                newprof.Telephone = tbtelephonenew.Text;
                newprof.Name = lbNamenew.Text;
                newprof.Login = tbLoginnew.Text;
                newprof.Mail = tbmailnew.Text;
                client.ChangeProfileInformation(newprof);
                tbLogin.Text = newprof.Login;
                lbName.Text = newprof.Name;
                mail.Text = newprof.Mail;
                telephone.Text = newprof.Telephone;
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
    }
}