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
            newminimum();
            allprice();
           

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

                        List<int> Cartone = new List<int>();
                        List<Tuple<int, int>> CartWho = new List<Tuple<int, int>>();
                        int i = 0;
                        foreach (ModelProductCart pr in ShopWindows.mainfprofile)
                        {
                            if (pr.How == 1) Cartone.Add(pr.Id);
                            else if (pr.How > 1)
                            {
                                CartWho.Add(new Tuple<int, int> (pr.Id, pr.How));
                            }
                        }

                        ShopWindows.client.BuyProduct(Cartone.ToArray(), profile.ID); ShopWindows.client.CheckProfile(profile.ID);
                        ShopWindows.client.BuyProductWholesale(CartWho.ToArray(), profile.ID); ShopWindows.client.CheckProfile(profile.ID);
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
        public bool inmylibrary(int j)
        {
            int no = 0;
            for (int i = 0; i < profile.Games.ToList().Count; i++)
            {
                if (ShopWindows.mainfprofile[j].Id == profile.Games.ToList()[i].Id) no++;
            }
            if (no != 0) return true;
            else return false;
            }
        public void newminimum()
        {

            for (int i = 0; i < ShopWindows.mainfprofile.Count; i++)
            {
                if (inmylibrary(i))
                {
                    ShopWindows.mainfprofile[i].How = 2;
                    ShopWindows.mainfprofile[i].Price = ShopWindows.mainfprofile[i].WholesalePrice;
                }
            }
        }
            public void allprice()
        {

            for (int i = 0; i < ShopWindows.mainfprofile.Count; i++)
            {
                lvProduct.Items.Add(ShopWindows.mainfprofile[i]);
            }

            double sum = 0;
            for (int i = 0; i < ShopWindows.mainfprofile.Count; i++)
            {
                sum += ShopWindows.mainfprofile[i].Price * ShopWindows.mainfprofile[i].How;
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
            int i = lvProduct.SelectedIndex;
            if (i != -1)
            {
                //Service.Product d = products.FirstOrDefault(o => o.Id ==i);
                lvProduct.Items.RemoveAt(i);
                ShopWindows.mainfprofile.RemoveAt(i);
                deleteproduct();
            }
        }

        private void Viewbox_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            ShopWindows.mainfprofile[lvProduct.SelectedIndex].How++;
            ShopWindows.mainfprofile[lvProduct.SelectedIndex].Price = ShopWindows.mainfprofile[lvProduct.SelectedIndex].WholesalePrice;
            lvProduct.Items.Clear();
            allprice();
            
        }

        private void Viewbox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ShopWindows.mainfprofile[lvProduct.SelectedIndex].How > 1)
            {
                if (ShopWindows.mainfprofile[lvProduct.SelectedIndex].How == 2 && inmylibrary(lvProduct.SelectedIndex)) ShopWindows.mainfprofile[lvProduct.SelectedIndex].Price = ShopWindows.mainfprofile[lvProduct.SelectedIndex].WholesalePrice;
                else
                {
                    ShopWindows.mainfprofile[lvProduct.SelectedIndex].How--;
                    if (ShopWindows.mainfprofile[lvProduct.SelectedIndex].How == 1) ShopWindows.mainfprofile[lvProduct.SelectedIndex].Price = ShopWindows.mainfprofile[lvProduct.SelectedIndex].RetailPrice;

                    else ShopWindows.mainfprofile[lvProduct.SelectedIndex].Price = ShopWindows.mainfprofile[lvProduct.SelectedIndex].WholesalePrice;
                    lvProduct.Items.Clear();
                    allprice();
                }
                
            }
            
        }
    }
}