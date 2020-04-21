using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Client.Data.ViewModels
{
    class ProductViewModel : DependencyObject
    {
        DataProvider dp = new DataProvider();
        public string FilterText
        {
            get { return (string)GetValue(FilterTextProperty); }
            set { SetValue(FilterTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FilterProduct.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterTextProperty =
            DependencyProperty.Register("FilterProduct", typeof(string), typeof(ProductViewModel), new PropertyMetadata("", FilterText_Changed));

        private static void FilterText_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var current = d as ProductViewModel;
            if (current != null)
            {
                current.Items.Filter = null;
                current.Items.Filter = current.FilterProduct;
            }
        }

        public ICollectionView Items
        {
            get { return (ICollectionView)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }


        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ICollectionView), typeof(ProductViewModel), new PropertyMetadata(null));

        public ProductViewModel()
        {
            List<ModelProduct> modelProducts = new List<ModelProduct>();
            List<Service.Product> products = ShopWindows.client.GetProductTable().ToList();
            foreach(Service.Product product in products)
            {
                ModelProduct modelProduct = new ModelProduct();
                modelProducts.Add(modelProduct.MakeModelProduct(product));
            }
            Items = CollectionViewSource.GetDefaultView(modelProducts);
            Items.Filter = FilterProduct;
        }

        private bool FilterProduct(object obj)
        {
            bool result = true;
            Service.Product product = obj as Service.Product;
            if (!string.IsNullOrWhiteSpace(FilterText) && product != null && !product.Name.Contains(FilterText) && !product.Description.Contains(FilterText))
            {
                result = false;
            }
            return result;
        }


    }
}
