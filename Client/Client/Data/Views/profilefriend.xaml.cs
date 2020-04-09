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
        public profilefriend(Service.Profile tv)
        {
           
            Service.Profile productnow = tv;
            InitializeComponent();
           // Lv.ItemsSource = tv.Friends.ToList() ;
            Inicialize(productnow);
            Loaded += Window_Loaded;
        }
        private void Inicialize(Service.Profile productnow)
        {
            tbFriendUser.Text = "Друзья пользователя " + productnow.Login;
            
            btnBack.Content = " <-Назад";
           // tbgame.Text = productnow.Name;
           // tbgame1.Text = productnow.Login;
            imMainImage.Source = dp.GetImageFromByte(productnow.MainImage);
            lbLogin.Content = productnow.Login;
            tbmail.Text = productnow.Mail;
            tbName.Text = productnow.Name;
            tbPhone.Text = productnow.Telephone;
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
            Service.Profile d = fr.FirstOrDefault(o => o.ID == i);
            profilefriend r = new profilefriend(d);
            r.Show();

        }
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
