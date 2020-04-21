using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Client
{
    class LibraryViewModel : DependencyObject
    {
        DataProvider dp = new DataProvider();
        public string FilterText
        {
            get { return (string)GetValue(FilterTextProperty); }
            set { SetValue(FilterTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FilterProductLibrary.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterTextProperty =
            DependencyProperty.Register("FilterProductLibrary", typeof(string), typeof(LibraryViewModel), new PropertyMetadata("", FilterText_Changed));

        private static void FilterText_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var current = d as LibraryViewModel;
            if (current != null)
            {
                current.LibraryItems.Filter = null;
                current.LibraryItems.Filter = current.FilterProductLibrary;
            }
        }

        public ICollectionView LibraryItems
        {
            get { return (ICollectionView)GetValue(LibraryItemsProperty); }
            set { SetValue(LibraryItemsProperty, value); }
        }


        // Using a DependencyProperty as the backing store for LibraryItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LibraryItemsProperty =
            DependencyProperty.Register("LibraryItems", typeof(ICollectionView), typeof(LibraryViewModel), new PropertyMetadata(null));

        public LibraryViewModel(List<Service.Product> products)
        {
            List<ModelProduct> modelProducts = new List<ModelProduct>();
            foreach(Service.Product product in products)
            {
                ModelProduct modelProduct = new ModelProduct();
                modelProducts.Add(modelProduct.MakeModelProduct(product));
            }
            LibraryItems = CollectionViewSource.GetDefaultView(modelProducts);
            LibraryItems.Filter = FilterProductLibrary;
        }

        private bool FilterProductLibrary(object obj)
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
