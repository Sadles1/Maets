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

        public static Service.WCFServiceClient client = new Service.WCFServiceClient("NetTcpBinding_IWCFService");
        DataProvider dp = new DataProvider();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            tbLogin.Text = "admin";
            tbPassword.Text = "admin";
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(client.State ==  CommunicationState.Closed)
                    client = new Service.WCFServiceClient("NetTcpBinding_IWCFService");
                Service.Profile profile = client.Connect(tbLogin.Text, dp.HashPassword(tbPassword.Text));
                if (profile == null)
                {
                    MessageBox.Show("Error");
                    return;
                }
                ShopWindows shopWindows = new ShopWindows(profile);
                shopWindows.Show();
                this.Close();
            }
            catch (FaultException ex)
            {
                MessageBox.Show(string.Format("{0} - {1}", ex.Code.Name, ex.Message),"ERROR",MessageBoxButton.OK);
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindows register = new RegisterWindows();

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
