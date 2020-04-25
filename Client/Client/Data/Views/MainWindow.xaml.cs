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
using Client.Data;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string data;
        int cnt = 0;
        static public ShopWindows shopWindows;
        static public RegisterWindows register;
        DataProvider dp = new DataProvider();
        public MainWindow()
        {
            
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            this.Title = "Maets";
           
            data = Environment.CurrentDirectory + @"\options.txt";
            FileInfo file = new FileInfo(data);
            if (!File.Exists(data))
            {
                RememberPassword.IsChecked = false;
            }
            else if (file.Length!=0)
            {
                RememberPassword.IsChecked = true;
                tbLogin.Text = File.ReadLines(data).Skip(0).First();
                tbPassword.Password = File.ReadLines(data).Skip(1).First(); ;
            }


        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                remember();
                cnt++;
                shopWindows = new ShopWindows(tbLogin.Text, dp.HashPassword(tbPassword.Password));
                shopWindows.Show();
                Close();
            }
            catch (FaultException ex)
            {
                if (cnt != 0) resetpassword.Visibility = Visibility.Visible;
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

        private void remember()
        {
            if (RememberPassword.IsChecked == true)
            {
                File.WriteAllText(data, "");
                StreamWriter f = new StreamWriter(data);
              
                f.WriteLine(tbLogin.Text);
                f.WriteLine(tbPassword.Password);
                f.Close();
            }

        }
        private void RememberPassword_Click(object sender, RoutedEventArgs e)
        {
        
        }

        private void TbLogin_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
           
        }

        private void TbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            
        }

        private void Resetpassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ResetPassword rp = new ResetPassword(tbLogin.Text);
                rp.Show();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
