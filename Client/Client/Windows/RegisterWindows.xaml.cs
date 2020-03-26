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

        async private void Register(string login, string name, string password, string mail, string telephone)
        {
            bool result = false;
            btnRegister.IsEnabled = false;
            await Task.Run(() =>
            {
                try
                {
                    Service.Profile profile = new Service.Profile();
                    profile.Login = login;
                    profile.Mail = mail;
                    string hashpassword = dp.HashPassword(password);
                    profile.AccessRight = 1;
                    profile.Telephone = telephone;
                    profile.Name = name;

                    MainWindow.client.Register(profile, hashpassword);
                    MessageBox.Show("Success");
                    result = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);                  
                }
            });
            btnRegister.IsEnabled = true;
            if (result)
                Close();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tbLogin.Text == "")
                    throw new Exception("Поле логин не может быть пустым");
                if (tbMail.Text == "")
                    throw new Exception("Поле почта не может быть пустым");
                if (tbPassword.Text == "")
                    throw new Exception("Поле пароль не может быть пустым");
                Register(tbLogin.Text, tbName.Text, tbPassword.Text, tbMail.Text, tbTelephone.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
