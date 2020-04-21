using Client.Data.ViewModels;
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
    /// Логика взаимодействия для Test.xaml
    /// </summary>
    public partial class Test : Window
    {
        DataProvider dp = new DataProvider();
        public Test()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
            ShopWindows shopWindows = new ShopWindows("admin", dp.HashPassword("admin"));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            qwe.DataContext = new ProfileViewModel();
            ewq.DataContext = new ProfileViewModel();
        }
        private void BtnexitProfile_Click(object sender, RoutedEventArgs e)
        {
            ShopWindows.client.Disconnect(1);
            ShopWindows.client.Close();
        }
    }
}
