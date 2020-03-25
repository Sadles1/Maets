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

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            Service.Profile profile = new Service.Profile();
            profile.Login = tbLogin.Text;
            profile.Mail = tbMail.Text;
            profile.Name = tbName.Text;
            profile.Telephone = tbTelephone.Text;
            string password = dp.HashPassword(tbPassword.Text);
            MainWindow.client.Register(profile,password);
        }
    }
}
