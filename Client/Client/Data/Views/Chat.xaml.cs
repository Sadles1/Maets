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

namespace Client.Data.Views
{
    /// <summary>
    /// Логика взаимодействия для Chat.xaml
    /// </summary>
    public partial class Chat : Window
    {
        static public Chat chatnow;
        public List<Service.UserMessage> chat;
        Service.Profile Senderg;
        Service.Profile Resiverg;

        public Chat(Service.Profile Sender, Service.Profile Resiver)
        {
            Senderg = Sender;
            Resiverg = Resiver;
            InitializeComponent();
            btnBack.Content = " <-Назад";
            Lvfriend.Items.Add( Resiver);
            chatnow = this;
             chat = ShopWindows.client.GetChat(Sender.ID, Resiver.ID).ToList();
            foreach (Service.UserMessage msg in chat)
            {
                
                    tbChat.Text += msg.message;
                    tbChat.Text += "\n";
            }
           
            scrl.ScrollToVerticalOffset(279);
        }
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Btngo_Click(object sender, RoutedEventArgs e)
        {
            Service.UserMessage chat1 = new Service.UserMessage();
            chat1.message = tbChatnew.Text;
            chat1.IDReceiver = Resiverg.ID;
            chat1.IDSender = Senderg.ID;
            ShopWindows.client.SendMsg(chat1);
            chat.Clear();
            chat = ShopWindows.client.GetChat(ShopWindows.client.CheckProfile(MainWindow.shopWindows.profile.ID).ID, Resiverg.ID).ToList();
            foreach (Service.UserMessage msg in chat)
            {
                if (ShopWindows.client.CheckProfile(Senderg.ID).ID == msg.IDSender)
                {
                    tbChat.TextAlignment = TextAlignment.Right;
                    tbChat.Text += msg.message;

                }
                else tbChat.Text += msg.message;
                tbChat.Text += "\n";
            }
           
            tbChat.Text += "\n";
            scrl.ScrollToVerticalOffset(int.MaxValue);
            tbChatnew.Clear();
        }
 
        
        private void Lvfriend_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
            int i = Lvfriend.SelectedIndex;
            //Service.Profile d = fr.FirstOrDefault(o => o.ID ==i);
            if (i != -1)
            {
                Lvfriend.Items.Clear();
                profilefriend r = new profilefriend(ShopWindows.client.CheckProfile(Resiverg.ID));
                r.Left = this.Left;
                r.Top = this.Top;
                r.Visibility = Visibility.Visible;
                r.Focus();
                this.Close();
            }

        }
    }
}
