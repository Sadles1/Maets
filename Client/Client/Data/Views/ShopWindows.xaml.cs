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
        public static List<Service.Product> mainfprofile = new List<Service.Product>();
        static public Korzina buyProduct;
        Service.Profile profile;
        List<Service.Product> products = MainWindow.client.GetProductTable().ToList();
        int ishop = 0;
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
            Back.IsEnabled = false;

            ProductOne.Items.Add(products[ishop]);      
            ProductTwo.Items.Add(products[ishop+1]);
            ProductFree.Items.Add(products[ishop+2]);
            //.ItemsSource = profile.Games;
            btnMoney.Content = Convert.ToString(profile.Money) + " р";
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
            MainWindow.client.Close();
            Close();
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
            Lvm.Visibility = Visibility.Hidden;
            
        }
        private void Buy_Click(object sender, RoutedEventArgs e)
        {
            buyProduct = new Korzina(profile);
            buyProduct.Show();
        }

        private void BtnexitProfile_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MF = new MainWindow();
            MainWindow.client.Close();
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
            List<Service.Product> products = MainWindow.client.GetProductTable().ToList();
             int i = Lvm.SelectedIndex;
            if (i != -1)
            {
                //Service.Product d = products.FirstOrDefault(o => o.Id ==i);
                Product Pr = new Product(products[i], profile);
                Pr.Show();
            }

        }
        private void leftmouse1(object sender, EventArgs e)
        {
                Product Pr = new Product(products[ishop], profile);
                Pr.Show();
        }
        private void leftmouse2(object sender, EventArgs e)
        {
                Product Pr = new Product(products[ishop + 1], profile);
                Pr.Show();
        }
        private void leftmouse3(object sender, EventArgs e)
        {
                Product Pr = new Product(products[ishop + 2], profile);
                Pr.Show();

        }
        private void leftmousefriend(object sender, EventArgs e)
        {
           List<Service.Profile> fr = profile.Friends.ToList();
           
            int i = Lv.SelectedIndex;
            //Service.Profile d = fr.FirstOrDefault(o => o.ID ==i);
            if (i != -1)
            {
                
                profilefriend r = new profilefriend(MainWindow.client.CheckFriend(fr[i].ID));
                r.Show();
             }

        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void leftmouse_mylibrary(object sender, EventArgs e)
        {
            List<Service.Product> products = MainWindow.client.GetProductTable().ToList();

            int i = Lvm.SelectedIndex;
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
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (ishop>=0)
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
    }
}