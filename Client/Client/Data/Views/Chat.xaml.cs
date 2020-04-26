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
        DataProvider dp = new DataProvider();
        public Chat(Service.Profile Sender, Service.Profile Resiver)
        {
            this.Title = "Maets";
            string data1 = Environment.CurrentDirectory + "\\Content\\maets.cur";
            var cursor = new Cursor(data1);
            this.Cursor = cursor;
            Senderg = Sender;
            Resiverg = Resiver;
            InitializeComponent();
            btnBack.Content = " <-Назад";
            Lvfriend.Items.Add( Resiver);
            chatnow = this;
             chat = ShopWindows.client.GetChat(Sender.ID, Resiver.ID).ToList();
            scrl.ScrollToVerticalOffset(int.MaxValue);
            foreach (Service.UserMessage msg in chat)
            {
                if (msg.IDSender == Senderg.ID)
                {
                   // msg.message.PadRight(100);
                    tbChat.Text += Senderg.Login + " " + msg.date + ": ";
                }
                else if (msg.IDSender == Resiverg.ID)
                {
                   // msg.message.PadLeft(100);
                    tbChat.Text += Resiverg.Login + " " + msg.date + ": ";
                }

                tbChat.Text += msg.message;
                tbChat.Text += "\n";
            }

            scrl.ScrollToVerticalOffset(int.MaxValue);
        }
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            ShopWindows.client.SetMessageRead(Senderg.ID, Resiverg.ID);

            MainWindow.shopWindows.Refreshmail.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        public void get_user(int id, DateTime msg)
        { 
            if (id == Senderg.ID)
                tbChat.Text += Senderg.Login + " " + msg + ": ";
            else 
                 tbChat.Text += Resiverg.Login + " " + msg + ": ";

        }

        private void Btngo_Click(object sender, RoutedEventArgs e)
        {
            Service.UserMessage chat1 = new Service.UserMessage();
            chat1.message = tbChatnew.Text;
            chat1.IDReceiver = Resiverg.ID;
            chat1.IDSender = Senderg.ID;
            ShopWindows.client.SendMsg(chat1);

            tbChatnew.Clear();


            tbChat.Text += Senderg.Login + " " + DateTime.Now + ": ";
             
                tbChat.Text += chat1.message;
                
                tbChat.Text += "\n";
            


            scrl.ScrollToVerticalOffset(int.MaxValue);

        }


        private void Lvfriend_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
            int i = Lvfriend.SelectedIndex;
            //Service.Profile d = fr.FirstOrDefault(o => o.ID ==i);
            if (i != -1)
            {
                
                this.Close();
            }

        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Btngo_Click(btngo, null);



        }
    }
}
