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
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class profilefriend : Window
    {
        Service.Profile tv = new Service.Profile();
        DataProvider dp = new DataProvider();
        public profilefriend(Service.Profile tv1)
        {

            tv = tv1;
            Service.Profile productnow = tv1;
            InitializeComponent();
            
            // Lv.ItemsSource = tv.Friends.ToList() ;
            Inicialize(productnow);

            Loaded += Window_Loaded;
            MainWindow.client.CheckFriend(tv1.ID);
        }
        private void Inicialize(Service.Profile productnow)
        {
            tbFriendUser.Text = "Друзья пользователя " + productnow.Login;
          //Service.WCFServiceClient client1 = new Service.WCFServiceClient("NetTcpBinding_IWCFService");
            
            btnBack.Content = " <-Назад";
           // tbgame.Text = productnow.Name;
           // tbgame1.Text = productnow.Login;
            imMainImage.Source = dp.GetImageFromByte(productnow.MainImage);
            lbLogin.Content = productnow.Login;
            tbmail.Text = productnow.Mail;
            tbName.Text = productnow.Name;
            tbPhone.Text = productnow.Telephone;
            Lv.ItemsSource = productnow.Friends;
           // Screenshoot.Source = dp.GetImageFromByte(tv.MainImage);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new ProductViewModel();

        }
        private void leftmousefriend(object sender, EventArgs e)
        {

            List<Service.Profile> fr = tv.Friends.ToList();

            int i = Lv.SelectedIndex;
            //Service.Profile d = fr.FirstOrDefault(o => o.ID ==i);
            if (i != -1)
            {

                profilefriend r = new profilefriend(MainWindow.client.CheckFriend(fr[i].ID));
                r.Show();
            }


        }
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
