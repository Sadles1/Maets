using System;
using System.Collections.Generic;
using System.Drawing;
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
    /// Логика взаимодействия для MailConfirmation.xaml
    /// </summary>
    public partial class MailConfirmation : Window
    {
        string code,hashpassword;
        Service.Profile profile;
        Service.WCFServiceClient client;

        public MailConfirmation(Service.Profile profile, string hashpassword)
        {
            string data1 = Environment.CurrentDirectory + "\\Content\\maets.cur";
            var cursor = new Cursor(data1);
            this.Cursor = cursor;
            client = new Service.WCFServiceClient(new System.ServiceModel.InstanceContext(new CallbackClass()), "NetTcpBinding_IWCFService");

                this.Title = "Maets";
                List<Service.Profile> profiles = client.GetAllUsers().ToList();

                for (int i = 0; i < profiles.Count; i++)
                {
                    if (profiles[i].Login == profile.Login)
                    {
                        throw new Exception("Этот логин занят!");
                    }
                    
                }

                this.profile = profile;
                this.hashpassword = hashpassword;
                InitializeComponent();
                tb.Text = "Введите код подтвержения, отправленный на почту " + profile.Mail;
                code = client.CheckMailRegister(profile.Mail);
            
          
            
        }
        private void TbExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        private void Codee_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Codee.Text != "") { Codee.Background = new SolidColorBrush(Colors.White); Codeef.Visibility = Visibility.Hidden; }
            else Codeef.Visibility = Visibility.Visible;

        }

        private void Go_Click(object sender, RoutedEventArgs e)
        {
            if (Codee.Text == code)
            {
                client.Register(profile, hashpassword);
                MessageBox.Show("Регистрация прошла успешно");
                this.Close();
                MainWindow.register.Close();
            }
            else
            {
                MessageBox.Show("Код неверен, повторите попытку");
            }
        }
    }
}
