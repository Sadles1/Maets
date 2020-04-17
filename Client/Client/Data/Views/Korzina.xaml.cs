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

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Buy_Click(object sender, RoutedEventArgs e)
        {
            if (lvProduct.Items.Count == 0) MessageBox.Show("Сначала добавте товар в корзину!");
            else
            {
                if (profile.Money >= Convert.ToDouble(tbSumm.Text))
                {
                    
                    profile.Money -= Convert.ToDouble(tbSumm.Text);
                    
                    MainWindow.client.BuyProduct(ShopWindows.mainfprofile.ToArray(), profile.ID);
                   
                    //Тутт будет добавление самой игры на аккаунт
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

        private void LvProduct_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            double sum = 0;
            for (int i=0;i< ShopWindows.mainfprofile.Count;i++)
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
    }
}