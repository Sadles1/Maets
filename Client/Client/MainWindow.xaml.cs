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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            Service.WCFServiceClient client = new Service.WCFServiceClient("NetTcpBinding_IWCFService");

            List<Service.Product> products = client.GetProductTable().ToList();
            DGProducts.ItemsSource = products;
            var product = products.First();
            TestImage.Source = DataProvider.GetImageFromByte(product.MainImage);
        }

    }
}
