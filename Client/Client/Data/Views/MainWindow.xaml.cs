using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.IO;
using System.Configuration;
using System.ServiceModel;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static public ShopWindows shopWindows;
        static public RegisterWindows register;
        DataProvider dp = new DataProvider();
        public MainWindow()
        {
            
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
           // tbLogin.Text = "p4shark";
          //  tbPassword.Password = "qwerty90";
            tbLogin.Text = "admin";
            tbPassword.Password = "admin";
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                shopWindows = new ShopWindows(tbLogin.Text, dp.HashPassword(tbPassword.Password));
                shopWindows.Show();
                Close();
            }
            catch (FaultException ex)
            {
                MessageBox.Show(string.Format("{0} - {1}", ex.Code.Name, ex.Message), "ERROR", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK);
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            register = new RegisterWindows();

            register.Show();
            this.Close();
        }

        private void TbExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TbSver_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();

        }


    }
}
