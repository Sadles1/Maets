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
        int ishop = 0;
        public Service.Profile prof;
        public static Korzina buyProduct;
        public Service.Product tv1 = new Service.Product();
        List<byte[]> screns;
        List<ModelImage> modelProducts;
        DataProvider dp = new DataProvider();
        public bool check(Service.Product pr)
        {
            this.Title = "Maets";

            List<Service.Product> listpr = prof.Games.ToList();
            int q=0;
            for( int i=0; i< listpr.Count;i++)
            {
                if (listpr[i].Id == pr.Id) q++;
            }
            if (q != 0) return true;
            else return false;
        }
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
            if (check(tv))
            {
                BuyAlready.Visibility = Visibility.Visible;
                BuyAlready.ToolTip = "Этот товар уже есть в вашей библиотеке, вы можете купить его только оптом(2+ штук))";
                
            }
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void Inicialize(Service.Product productnow)
        {
            screns = new List<byte[]>();
            screns.Add(productnow.MainImage);
            for (int i = 0; i < ShopWindows.client.GetGameImages(productnow.Id).ToList().Count; i++)
            {
                screns.Add(ShopWindows.client.GetGameImages(productnow.Id).ToList()[i]);
            }
            ;
             modelProducts = new List<ModelImage>();
            foreach (byte[] product in screns)
            {
                ModelImage modelProduct = new ModelImage();
                modelProducts.Add(modelProduct.MakeModelImage(product));
            }
            Back.IsEnabled = false;

            Screenshoot.Items.Add(modelProducts[ishop]);
            Screenshoot1.Items.Add(modelProducts[ishop + 1]);
            Screenshoot2.Items.Add(modelProducts[ishop + 2]);
            Screenshoot3.Items.Add(modelProducts[ishop + 3]);
            Screenshoot4.Items.Add(modelProducts[ishop + 4]);
            Screenshoot5.Items.Add(modelProducts[ishop + 5]);
            tbgameName.Text = productnow.Name;
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
            MainWindow.shopWindows.Visibility = Visibility.Visible;
            MainWindow.shopWindows.Refresh.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            this.Close();
        }

        private void BtnBuy_Click(object sender, RoutedEventArgs e)
        {
            ModelProductCart tvc = new ModelProductCart();
            tvc = tvc.MakeModelProductCart(tv1);
            tvc.Price = tv1.RetailPrice;
            ShopWindows.mainfprofile.Add(tvc);
            btnBuy.Visibility = Visibility.Hidden;
            btnkorzina.Visibility = Visibility.Visible;
        }

        private void Btnkorzina_Click(object sender, RoutedEventArgs e)
        {
            buyProduct = new Korzina(prof);
            buyProduct.Left = this.Left;
            buyProduct.Top = this.Top;
            buyProduct.Show();
            this.Close();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Next.IsEnabled = true;
            Back.IsEnabled = true;
            if (ishop >= 0)
            {
                ishop--;
                Screenshoot.Items.Clear();
                Screenshoot.Items.Add(modelProducts[ishop]);
                if (ishop == 0)
                {

                    Back.IsEnabled = false;
                    Screenshoot.Items.Clear();
                    Screenshoot.Items.Add(modelProducts[0]);
                    //Back.Visibility = Visibility.Hidden;
                    //Next.Visibility = Visibility.Visible;
                }
            }

        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {

            Next.IsEnabled = true;
            Back.IsEnabled = true;
            if (5 >= ishop + 1)
            {
                ishop++;

                Screenshoot.Items.Clear();

                Screenshoot.Items.Add(modelProducts[ishop]);
                if ((ishop) == 5)
                {
                    Next.IsEnabled = false;
                    // Next.Visibility = Visibility.Hidden;
                    //Back.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
