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
    public partial class Korzina : Window
    {
        Service.Profile profile;
        DataProvider dp = new DataProvider();
        public Korzina(Service.Profile profile)
        { 
            this.profile = profile;
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();   
            
            window.Show();
            this.Close();
        }
        private void TbExit_Click(object sender, RoutedEventArgs e)
        {
            ShopWindows MF = new ShopWindows(profile);
           
            MF.Show();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Buy_Click(object sender, RoutedEventArgs e)
        {
          
        }
    }
}