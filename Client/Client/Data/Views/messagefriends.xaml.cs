using Client.Data.Views;
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
    /// Логика взаимодействия для Reqestfriends.xaml
    /// </summary>
    public partial class messagefriends : Window
    {
        List<ModelProfileMessage> mail = new List<ModelProfileMessage>();
        Service.Profile who;
        ModelProfileMessage whom = new ModelProfileMessage();
        public messagefriends()
        {
            this.Title = "Заявки в друзья";
            string data1 = Environment.CurrentDirectory + "\\Content\\maets.cur";
            var cursor = new Cursor(data1);
            this.Cursor = cursor;
            InitializeComponent();
            foreach (Service.UserMessage person in ShopWindows.frmail)
            {
                who = ShopWindows.client.CheckProfile(person.IDSender);
                whom = whom.MakeModelProfileMessage(who, person.message);
                mail.Add(whom);
            }
            Lvfriendnew.ItemsSource = mail;


        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }


        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void leftmousefriend(object sender, EventArgs e)
        {
           // List<Service.Profile> fr = MainWindow.shopWindows.profile.FriendReqests.ToList();

            int i = Lvfriendnew.SelectedIndex;
            //Service.Profile d = fr.FirstOrDefault(o => o.ID ==i);
            if (i != -1)
            {
                // client.AddToBlacklist(MainWindow.shopWindows.profile.ID, fr[i].ID);
               // ShopWindows.client.RemoveFromBlacklist(MainWindow.shopWindows.profile.ID, fr[i].ID);
                profilefriend r = new profilefriend(ShopWindows.client.CheckProfile(ShopWindows.frmail[i].IDSender));

                r.Left = this.Left;

                r.Top = this.Top;
                
                r.Show();
                r.btnChat.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                this.Close();
                MainWindow.shopWindows.Visibility = Visibility.Hidden;
            }

        }
    }
}
