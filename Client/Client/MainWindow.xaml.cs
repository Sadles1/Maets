﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.IO;
using System.Configuration;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Service.WCFServiceClient client = new Service.WCFServiceClient("NetTcpBinding_IWCFService");
        public static Service.Profile profile;
        DataProvider dp = new DataProvider();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {

        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            profile = client.Connect(tbLogin.Text, dp.HashPassword(tbPassword.Text));
            if (profile == null)
            {
                MessageBox.Show("Error");
                return;
            }
            MessageBox.Show("Succes login");

        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindows register = new RegisterWindows();
            register.Show();
        }
    }
}
