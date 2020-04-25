using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
    /// Логика взаимодействия для MailConfirmation.xaml
    /// </summary>
    public partial class Changepassword : Window
    {
        DataProvider dp = new DataProvider();
        public Changepassword(string Login)
        {
            this.Title = "Maets";
            InitializeComponent();
        }

        private void TbExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Changepassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tbPassword1.Password == "")
                    throw new Exception("Поле пароль не может быть пустым");
                if (tbPassword2.Password != tbPassword1.Password)
                    throw new Exception("Пароли не совпадают");
                ShopWindows.client.changePassword(MainWindow.shopWindows.profile.ID, dp.HashPassword(tbPasswordold.Password), dp.HashPassword(tbPassword1.Password));
                MessageBox.Show("Пароль успешно изменён");
                this.Close();
            }
            catch (FaultException exs)
            {
                MessageBox.Show(exs.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           

        }
    }
}
