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
    /// Логика взаимодействия для MailConfirmation.xaml
    /// </summary>
    public partial class MailConfirmation : Window
    {
        string code,hashpassword;
        Service.Profile profile;
        Service.WCFServiceClient client;

        public MailConfirmation(Service.Profile profile, string hashpassword)
        {
            client = new Service.WCFServiceClient(new System.ServiceModel.InstanceContext(new CallbackClass()), "NetTcpBinding_IWCFService");
            this.profile = profile;
            this.hashpassword = hashpassword; 
            InitializeComponent();
            tb.Text = "Введите код подтвержения, отправленный на почту \n" + profile.Mail;
            code = client.CheckMail(profile.Mail);
            
        }

        private void Go_Click(object sender, RoutedEventArgs e)
        {
            if (Code.Text == code)
            {
                client.Register(profile, hashpassword);
                MessageBox.Show("Success");
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
