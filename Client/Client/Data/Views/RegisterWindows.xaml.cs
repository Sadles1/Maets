using Client.Service;
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
    /// Логика взаимодействия для RegisterWindows.xaml
    /// </summary>
    public partial class RegisterWindows : Window
    {
        DataProvider dp = new DataProvider();
        public RegisterWindows()
        {
            InitializeComponent();
        }

        //async private void Register(string login, string name, string password, string mail, string telephone)//асинхронный метод для регистрации
        //{
        //    bool result = false;
        //    btnRegister.IsEnabled = false;
        //    await Task.Run(() =>
        //    {
        //        try
        //        {
                   

                    
        //            result = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);                  
        //        }
        //    });
        //    btnRegister.IsEnabled = true;
        //    if (result)
        //        Close();
        //}
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tbLogin.Text == "")
                    throw new Exception("Поле логин не может быть пустым");
                if (tbMail.Text == "")
                    throw new Exception("Поле почта не может быть пустым");
                if (tbPassword.Password == "")
                    throw new Exception("Поле пароль не может быть пустым");
                WCFServiceClient client = new WCFServiceClient(new System.ServiceModel.InstanceContext(new CallbackClass()), "NetTcpBinding_IWCFService");
                Service.Profile profile = new Service.Profile();
                profile.Login = tbLogin.Text;
                profile.Mail = tbMail.Text;
                string hashpassword = dp.HashPassword(tbPassword.Password);
                profile.AccessRight = 1;
                profile.Telephone = tbTelephone.Text;
                profile.Name = tbName.Text;
                MailConfirmation q = new MailConfirmation(profile, hashpassword);
                q.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TbExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TbSver_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow MF = new MainWindow();
            MF.Show();
        }
    }
}