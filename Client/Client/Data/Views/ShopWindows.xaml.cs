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
    public partial class ShopWindows : Window
    {
        Service.Profile profile;
        DataProvider dp = new DataProvider();
        public ShopWindows(Service.Profile profile)
        { 
            this.profile = profile;
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            imMainImage.Source = dp.GetImageFromByte(profile.MainImage);
            lbLogin.Content = profile.Login;
            lbName.Content = profile.Name;
            Lv.ItemsSource = profile.Friends;
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

        private void Buy_Click(object sender, RoutedEventArgs e)
        {
            Korzina buyProduct = new Korzina(profile);
            buyProduct.Show();
            this.Close();
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
            Product Pr = new Product();
            Pr.Show();
        }
    }
}