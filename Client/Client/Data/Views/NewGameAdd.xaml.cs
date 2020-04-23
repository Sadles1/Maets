using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class NewGameAdd : Window
    {
  
        List<ModelImage> modelProducts;
        List<byte[]> gamescreen = new List<byte[]>();
        DataProvider dp = new DataProvider();
      
        public NewGameAdd()
        {
            
            InitializeComponent();
            modelProducts = new List<ModelImage>();
            btnAdd.Content = "Отправить заявку\n на добаление";
            ModelImage modelProduct = new ModelImage();
            modelProducts.Add(modelProduct.MakeModelImage(File.ReadAllBytes($@"{Environment.CurrentDirectory}\Content\Images\1.encr")));
            Screenshoot.Items.Add(modelProducts[0]);
            Screenshoot1.Items.Add(modelProducts[0]);
            Screenshoot2.Items.Add(modelProducts[0]);
            Screenshoot3.Items.Add(modelProducts[0]);
            Screenshoot4.Items.Add(modelProducts[0]);
            Screenshoot5.Items.Add(modelProducts[0]);
            Screenshoot1.IsEnabled = false;
            Screenshoot2.IsEnabled = false;
            Screenshoot3.IsEnabled = false;
            Screenshoot4.IsEnabled = false;
            Screenshoot5.IsEnabled = false;
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
  
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.shopWindows.Visibility = Visibility.Visible;
            MainWindow.shopWindows.Refresh.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
            this.Close();
        }

        private ModelImage get_image_from_PC()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            
            string s = openFileDialog.FileName;
            // System.Windows.MessageBox.Show(s);
            byte[] buffer = dp.GetByteFromImage(s);
            gamescreen.Add(buffer);

            ModelImage modelProduct = new ModelImage();
            modelProduct = modelProduct.MakeModelImage(buffer);
            return modelProduct;
        }
        private void Screenshoot_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Screenshoot.Items.Clear();
            Screenshoot.Items.Add(get_image_from_PC());
            Screenshoot1.IsEnabled = true;
            Screenshoot2.IsEnabled = true;
            Screenshoot3.IsEnabled = true;
            Screenshoot4.IsEnabled = true;
            Screenshoot5.IsEnabled = true;
        }

        private void Screenshoot1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Screenshoot1.Items.Clear();
            Screenshoot1.Items.Add(get_image_from_PC());
        }

        private void Screenshoot2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Screenshoot2.Items.Clear();
            Screenshoot2.Items.Add(get_image_from_PC());
        }
       
        private void Screenshoot3_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Screenshoot3.Items.Clear();
            Screenshoot3.Items.Add(get_image_from_PC());
        }


        private void Screenshoot5_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Screenshoot5.Items.Clear();
            Screenshoot5.Items.Add(get_image_from_PC());
        }

        private void TbgameName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbgameName.Text != "") tbgameName_hint.Visibility = Visibility.Hidden;
            else tbgameName_hint.Visibility = Visibility.Visible;
        }

        private void TbDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbDescription.Text != "") tbDescription_hint.Visibility = Visibility.Hidden;
            else tbDescription_hint.Visibility = Visibility.Visible;
        }

        private void TbDeveloper_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbDeveloper.Text != "") tbDeveloper_hint.Visibility = Visibility.Hidden;
            else tbDeveloper_hint.Visibility = Visibility.Visible;
        }

        private void TbPublisher_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbPublisher.Text != "")
            {

                tbPublisher_hint.Visibility = Visibility.Hidden; }
            else tbPublisher_hint.Visibility = Visibility.Visible;
        }

        private void Screenshoot4_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Screenshoot4.Items.Clear();
            Screenshoot4.Items.Add(get_image_from_PC());
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if(tbDescription.Text =="" || tbDeveloper.Text == "" || tbPublisher.Text == "" || tbgameName.Text == "" || gamescreen.Count<6 || price.Text=="" )
            {
                System.Windows.MessageBox.Show("Не все поля заполнены!");
            }
            else
            {
                Service.Product yourgame = new Service.Product();
                yourgame.Name = tbgameName.Text;
                yourgame.Description = tbDescription.Text;
                yourgame.Developer = tbDeveloper.Text;
                yourgame.Publisher = tbPublisher.Text;
                yourgame.MainImage = gamescreen[0];
                yourgame.RetailPrice = Convert.ToDouble(price.Text);
                yourgame.WholesalePrice = Convert.ToDouble(price.Text);
                List<byte[]> scrs = new List<byte[]>();
                for(int i=1;i<gamescreen.Count; i++)
                {
                    scrs.Add(gamescreen[i]);
                }
                ShopWindows.client.AddModerationProduct(yourgame, scrs.ToArray());
                System.Windows.MessageBox.Show("Успешно отправлено на модерацию");
                this.Close();
            }

        }
    }
}
