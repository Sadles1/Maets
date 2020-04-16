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
    /// Логика взаимодействия для ShopWindows.xaml
    /// </summary> 
    public partial class Korzina : Window
    {
        Service.Profile profile;
        DataProvider dp = new DataProvider();
        static public List<Service.Product> tvkrz = new List<Service.Product>();
        public bool check(Service.Product b)
        {
            int p=0;
            for(int i=0;i< tvkrz.Count; i++)
            {
                if (tvkrz[i].Id == b.Id) p++;
            }
            if (p != 0) return true;
            else return false ;
        }

        public Korzina(Service.Profile profile, Service.Product tv,List<Service.Product> korz)
        {
            tvkrz = korz;
            this.profile = profile;
            InitializeComponent();
            if (!check(tv) && tv.Id != -1)
            {
                tvkrz.Add(tv);
                 
            }
            for (int i = 0; i < tvkrz.Count; i++)
            {
                lvProduct.Items.Add(tvkrz[i]);
            }
            allprice();

        }

        private void Window_Initialized(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();   
            
            window.Show();
            this.Close();
        }
        private void TbExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnFull_Click(object sender, RoutedEventArgs e)
        {
            if(WindowState != WindowState.Maximized) 
            WindowState = WindowState.Maximized;
            else
            {
                WindowState= WindowState.Normal;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Buy_Click(object sender, RoutedEventArgs e)
        {
            if (lvProduct.Items.Count == 0) MessageBox.Show("Сначала добавте товар в корзину!");
            else
            {
                if (profile.Money >= Convert.ToDouble(tbSumm.Text))
                {
                    lvProduct.Items.Clear();
                    profile.Money -= Convert.ToDouble(tbSumm.Text);
                    tbSumm.Text = "";
                    MainWindow.client.BuyProduct(tvkrz.ToArray(), profile.ID);
                    tvkrz.Clear();
                    MessageBox.Show("Покупка успешно совершена, с вашего счета списано" + tbSumm.Text + "\n Остаток: " + profile.Money);
                }
                else
                {
                    MessageBox.Show("Недостаточно средств! \n Нужно ещё " + (Convert.ToDouble(tbSumm.Text) - profile.Money));
                }
            }
        }

        private void LvProduct_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            double sum = 0;
            for (int i=0;i<tvkrz.Count;i++)
            {
                sum += tvkrz[i].RetailPrice;
            }
            tbSumm.Text = Convert.ToString(sum);
        }
        public void allprice()
        {
            double sum = 0;
            for (int i = 0; i < tvkrz.Count; i++)
            {
                sum += tvkrz[i].RetailPrice;
            }
            tbSumm.Text = Convert.ToString(sum);
        }
    }
}