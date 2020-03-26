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
    public partial class ShopWindows : Window
    {
        Service.Profile profile;
        DataProvider dp = new DataProvider();
        public ShopWindows(Service.Profile profile)
        { 
            this.profile = profile;
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            imMainImage.Source = dp.GetImageFromByte(profile.MainImage);
            lbLogin.Content = profile.Login;
            lbName.Content = profile.Name;
        }
    }
}
