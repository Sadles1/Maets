﻿using Client.Data.ViewModels;
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
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class profilefriend : Window
    {
        public List<Service.UserMessage> chat = new List<Service.UserMessage>();

        Service.Profile tv = new Service.Profile();
        Service.Profile Resiver = new Service.Profile();
        Service.Profile Sender = new Service.Profile();
        DataProvider dp = new DataProvider();
        
        public profilefriend(Service.Profile tv1)
        {
            Sender = MainWindow.shopWindows.profile;
            Resiver = tv1;
            tv = tv1;
            Service.Profile productnow = tv1;
            InitializeComponent();
            if (ShopWindows.client.CheckBlacklist(Sender.ID, Resiver.ID))
            {
                imMainImage.Source = dp.GetImageFromByte(productnow.MainImage);
                lbLogin.Content = productnow.Login;
                if (tv.status)
                    lbStatus.Content = "Online";
                else lbStatus.Content = "Offline";
                lbName.Content = "Этот пользователь добавил вас в Чёрный список";
            }
            else
            {
                // Lv.ItemsSource = tv.Friends.ToList() ;
                Inicialize(productnow);
                blacklistgo.Content = "Дoбавить пользователя \n в Чёрный список";
                Loaded += Window_Loaded;
                ShopWindows.client.CheckProfile(tv1.ID);
            }
            
        }
        public bool checkfriends()
        {  
            List<Service.Profile> myfriends = MainWindow.shopWindows.profile.Friends.ToList();
            int i = 0;
            int cnt = 0;
            for(i=0;i<myfriends.Count;i++)
            {
                if (tv.ID == myfriends[i].ID) cnt++;
            }
            if (cnt != 0) return true;
            else return false;
        }
      
        public bool checkfriendsrequest()
        {
            List<Service.Profile> myfriends = ShopWindows.client.GetFriendRequests(Sender.ID).ToList() ;
            int i = 0;
            int cnt = 0;
            for(i=0;i<myfriends.Count;i++)
            {
                if (tv.ID == myfriends[i].ID) cnt++;
            }
            if (cnt != 0) return true;
            else return false;
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
            if (tv.status)
                lbStatus.Content = "Online";
            else lbStatus.Content = "Offline";
            tbName.Text = productnow.Name;
            tbPhone.Text = productnow.Telephone;
            Lv.ItemsSource = productnow.Friends;

            if (MainWindow.shopWindows.profile.ID != tv.ID)
            {
                if (checkfriendsrequest())
                {
                    FriendYN.Visibility = Visibility.Visible;
                }
                if (checkfriends())
                {
                    btndelfriend.Visibility = Visibility.Visible;
                    btnnewfriend.Visibility = Visibility.Hidden;
                }
                else
                {
                    btnnewfriend.Visibility = Visibility.Visible;
                    btndelfriend.Visibility = Visibility.Hidden;

                }
            }
            else
            {
                btnnewfriend.Visibility = Visibility.Hidden;
                btndelfriend.Visibility = Visibility.Hidden;


            }

            //  tbChat.Text = chat.ToList()[0].message;
            // Screenshoot.Source = dp.GetImageFromByte(tv.MainImage);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           // DataContext = new ProductViewModel();

        }
        private void leftmousefriend(object sender, EventArgs e)
        {

            List<Service.Profile> fr = tv.Friends.ToList();

            int i = Lv.SelectedIndex;
            //Service.Profile d = fr.FirstOrDefault(o => o.ID ==i);
            if (i != -1)
            {

                profilefriend r = new profilefriend(ShopWindows.client.CheckProfile(fr[i].ID));
                r.Left = this.Left;
                r.Top = this.Top;
                r.Show();
                this.Close();
            }


        }
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.shopWindows.Refresh.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

            MainWindow.shopWindows.Visibility = Visibility.Visible;
            this.Close();
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void BtnChat_Click(object sender, RoutedEventArgs e)
        {
            Chat chat = new Chat(Sender, Resiver);
            chat.Left = Left;
            chat.Top = Top;
            chat.Show();
        }

        private void Btnnewfriend_Click(object sender, RoutedEventArgs e)
        {
            ShopWindows.client.SendFriendRequest(Sender.ID,Resiver.ID);

        }

        private void Btndelfriend_Click(object sender, RoutedEventArgs e)
        {
            ShopWindows.client.DeleteFriend(Sender.ID, Resiver.ID);
        }

        private void Btnnewfriendyes_Click(object sender, RoutedEventArgs e)
        {
            btndelfriend.Visibility = Visibility.Visible;
            FriendYN.Visibility = Visibility.Hidden;
            ShopWindows.client.DeleteFriendReqest(Sender.ID, Resiver.ID);
            ShopWindows.client.AddFriend(Sender.ID, Resiver.ID);
        }


        private void Btnnewlfriendno_Click(object sender, RoutedEventArgs e)
        {
            btnnewfriend.Visibility = Visibility.Visible;
            FriendYN.Visibility = Visibility.Hidden;
            ShopWindows.client.DeleteFriendReqest(Sender.ID, Resiver.ID);

            //ShopWindows.client.AddFriend(Sender.ID, Resiver.ID);
        }


        private void Blacklistgo_Click(object sender, RoutedEventArgs e)
        {
            ShopWindows.client.AddToBlacklist(Sender.ID, Resiver.ID);
            blacklistgo.Visibility = Visibility.Hidden;
            blacklistout.Visibility = Visibility.Visible;
            blacklistout.Content = "Удалить из \n из Чёрного списка";
        }

        private void Blacklistout_Click(object sender, RoutedEventArgs e)
        {
            ShopWindows.client.AddToBlacklist(Sender.ID, Resiver.ID);
            blacklistgo.Visibility = Visibility.Visible;
            blacklistout.Visibility = Visibility.Hidden;
           // blacklistout.Content = "Удалить из \n из Чёрного списка";
        }
    }
}
