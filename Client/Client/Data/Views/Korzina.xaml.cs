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
        int idx = -1;
        DataProvider dp = new DataProvider();

        public Korzina(Service.Profile profile)
        {

            this.profile = profile;
            InitializeComponent();
            allprice();
            for (int i = 0; i < ShopWindows.mainfprofile.Count; i++)
            {
                lvProduct.Items.Add(ShopWindows.mainfprofile[i]);
            }

        }

        private void Window_Initialized(object sender, EventArgs e)
        {

        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.shopWindows.Refresh.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            this.Close();
            MainWindow.shopWindows.Visibility = Visibility.Visible;
        }

        private void Buy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lvProduct.Items.Count == 0)
                    MessageBox.Show("Сначала добавте товар в корзину!");
                else
                {
                    if (profile.Money >= Convert.ToDouble(tbSumm.Text))
                    {

                        profile.Money -= Convert.ToDouble(tbSumm.Text);

                        List<int> Cart = new List<int>();
                        foreach (Service.Product pr in ShopWindows.mainfprofile)
                        {
                            Cart.Add(pr.Id);
                        }

                        ShopWindows.client.BuyProduct(Cart.ToArray(), profile.ID); ShopWindows.client.CheckProfile(profile.ID);
                        //Тут будет добавление самой игры на аккаунт
                        MessageBox.Show("Покупка успешно совершена! \n C вашего счета списано " + tbSumm.Text + "\n Остаток: " + profile.Money);
                        ShopWindows.mainfprofile.Clear();
                        lvProduct.Items.Clear();
                        tbSumm.Text = "";

                    }
                    else
                    {
                        MessageBox.Show("Недостаточно средств! \n Нужно ещё " + (Convert.ToDouble(tbSumm.Text) - profile.Money));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("{0}", ex.Message), "ERROR", MessageBoxButton.OK);
            }
        }

        private void LvProduct_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            double sum = 0;
            idx = lvProduct.SelectedIndex;
            for (int i = 0; i < ShopWindows.mainfprofile.Count; i++)
            {
                sum += ShopWindows.mainfprofile[i].RetailPrice;
            }
            tbSumm.Text = Convert.ToString(sum);
        }
        public void allprice()
        {
            double sum = 0;
            for (int i = 0; i < ShopWindows.mainfprofile.Count; i++)
            {
                sum += ShopWindows.mainfprofile[i].RetailPrice;
            }
            tbSumm.Text = Convert.ToString(sum);
        }
        public void deleteproduct()
        {
            if (ShopWindows.mainfprofile.Count != 0)
            {
                double sum = 0;
                for (int i = 0; i < ShopWindows.mainfprofile.Count; i++)
                {
                    sum += ShopWindows.mainfprofile[i].RetailPrice;
                }
                tbSumm.Text = Convert.ToString(sum);
            }
            else tbSumm.Text = "0";
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            List<Service.Product> products = ShopWindows.client.GetProductTable().ToList();
            int i = lvProduct.SelectedIndex;
            if (i != -1)
            {
                //Service.Product d = products.FirstOrDefault(o => o.Id ==i);
                lvProduct.Items.RemoveAt(i);
                ShopWindows.mainfprofile.RemoveAt(i);
                deleteproduct();
            }
        }
    }
}