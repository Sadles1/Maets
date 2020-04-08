using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для ShopWindows.xaml
    /// </summary>
    //public class ModelFriends: Service.Product
    //{
    //    public ImageSource Image { get; set; }

    //    public ModelFriends MakeModelFriends(Service.Product product)
    //    {
    //        DataProvider dp = new DataProvider();
    //        ModelFriends modelFriends = new ModelFriends();
    //        modelFriends.Name = product.Name;
    //        modelFriends.Image = dp.GetImageFromByte(product.MainImage);
    //        modelFriends.Description = product.Description;
    //        return modelFriends;
    //    }
    //}
    public partial class ShopWindows : Window
    {
        public Service.Product fake = new Service.Product();
        static public List<Service.Product> tvkrz = new List<Service.Product>();
        static public Korzina buyProduct;
        Service.Profile profile;
        DataProvider dp = new DataProvider();
        int idxg;
        public ShopWindows(Service.Profile profile)
        {
          
            this.profile = profile;
            InitializeComponent();
            Loaded += Window_Loaded;
            
            if (profile.AccessRight == 3) Onlyforadmin.Visibility = Visibility.Visible;

        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            imMainImage.Source = dp.GetImageFromByte(profile.MainImage);
            lbLogin.Content = profile.Login;
            lbName.Content = profile.Name;
            Lv.ItemsSource = profile.Friends;
           // lbLogin.Content = profile.AccessRight;


        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();   
            
            window.Show();
            this.Close();
        }
        private void TbExit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.client.Disconnect(profile.ID);
            this.Close();
        }

        private void BtnFull_Click(object sender, RoutedEventArgs e)
        {
            if(WindowState != WindowState.Maximized) 
            WindowState = WindowState.Maximized;
            else
            {
                WindowState= WindowState.Normal;
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
          DataContext = new ProductViewModel();
            
        }
        private void Buy_Click(object sender, RoutedEventArgs e)
        {
            buyProduct = new Korzina(profile, fake,tvkrz);
            buyProduct.Show();
        }

        private void BtnexitProfile_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MF = new MainWindow();
            MainWindow.client.Disconnect(profile.ID);
            MF.Show();
            this.Close();
        }

        private void BtnFakeProduct_Click(object sender, RoutedEventArgs e)
        {
            Service.Product id = new Service.Product();
           // Product Pr = new Product(id);
           // Pr.Show();
        }
        private void leftmouse(object sender, EventArgs e)
        {
            List<Service.Product> products = MainWindow.client.GetProductTable().ToList();

            int i = Lvm.SelectedIndex;
            if(i!=-1)
            { 
            //Service.Product d = products.FirstOrDefault(o => o.Id ==i);
            Product Pr = new Product(products[i],profile,tvkrz);
            Pr.Show();
                }

        }
        private void leftmousefriend(object sender, EventArgs e)
        {
            List<Service.Profile> fr = profile.Friends.ToList();
            int i = Lv.SelectedIndex;
           // Service.Profile d = fr.FirstOrDefault(o => o.ID ==i);
            profilefriend r = new profilefriend(fr[i]);
            r.Show();

        }

        private void leftmouse_2(object sender, EventArgs e)
        {
            List<Service.Product> products = MainWindow.client.GetProductTable().ToList();

            int i = Lvmylibrary.SelectedIndex;
            if (i != -1)
            {
                tbgamename.Text = products[i].Name;
            }

        }

        private void Lvm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             idxg = Lvm.SelectedIndex;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Вы запустили игру " + tbgamename.Text);
        }
    }
}