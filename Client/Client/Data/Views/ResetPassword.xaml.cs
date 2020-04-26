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
    public partial class ResetPassword : Window
    {
        string code;
        Service.Profile profile;
        Service.WCFServiceClient client;
        DataProvider dp = new DataProvider();
        public ResetPassword(string Login)
        {
            string data1 = Environment.CurrentDirectory + "\\Content\\maets.cur";
            var cursor = new Cursor(data1);
            this.Cursor = cursor;
            this.Title = "Maets";

            client = new Service.WCFServiceClient(new System.ServiceModel.InstanceContext(new CallbackClass()), "NetTcpBinding_IWCFService");
            List<Service.Profile> profiles = client.GetAllUsers().ToList();
            for (int i = 0; i < profiles.Count; i++)
            {
                if (profiles[i].Login == Login)
                {
                    profile = profiles[i];
                    break;
                }
                ;
            }
            if (profile.Login != Login)
                throw new Exception("Такого пользовaтеля не существует");
            InitializeComponent();
            profile = client.CheckProfile(profile.ID);

            tb.Text = "Введите код подтвержения, отправленный на почту " + profile.Mail;
            code = client.CheckMailResetPassword(profile.Mail);
            
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
                client.resetPassword(profile.ID,dp.HashPassword(tbPassword1.Password));
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

        private void Codee_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Code.Text != "") Codee.Visibility = Visibility.Visible;
            else Codee.Visibility = Visibility.Hidden;
        }
    }
}
