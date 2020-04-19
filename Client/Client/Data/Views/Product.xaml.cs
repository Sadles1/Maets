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
    public partial class Product : Window
    {
        public Service.Profile prof;
        public static Korzina buyProduct;
        public Service.Product tv1 = new Service.Product();
        DataProvider dp = new DataProvider();
        public Product(Service.Product tv, Service.Profile profile)
        {
            tv1 = tv;
            InitializeComponent();
            prof = profile;
            Inicialize(tv1);
            int p = 0;

            if (ShopWindows.mainfprofile.Count != 0)
            {
                for (int i = 0; i < ShopWindows.mainfprofile.Count; i++)
                {
                    if (ShopWindows.mainfprofile[i].Id == tv1.Id) p++;
                }
                if (p != 0)
                {
                    btnBuy.Visibility = Visibility.Hidden;
                    btnkorzina.Visibility = Visibility.Visible;
                }
            }
        }
        private void Inicialize(Service.Product productnow)
        {
            tbgameName.Text = productnow.Name;
            Screenshoot.Source = dp.GetImageFromByte(productnow.MainImage);
            tbDescription.Text = productnow.Description;
            tbDeveloper.Text = productnow.Developer;
            tbPublisher.Text = productnow.Publisher;
            //tbgame1.Text = productnow.Description + "\n" + productnow.Developer + "\n";
            price.Text = Convert.ToString(productnow.RetailPrice);
            price.ToolTip = "Тут должна быть цена другая, но она пока не рабоатет";
            // Screenshoot.Source = dp.GetImageFromByte(tv.MainImage);
        }
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnBuy_Click(object sender, RoutedEventArgs e)
        {
            ShopWindows.mainfprofile.Add(tv1);
            btnBuy.Visibility = Visibility.Hidden;
            btnkorzina.Visibility = Visibility.Visible;
        }

        private void Btnkorzina_Click(object sender, RoutedEventArgs e)
        {
            buyProduct = new Korzina(prof);
            buyProduct.Show();
            this.Close();
        }

    }
}
