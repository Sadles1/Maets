using Client.Data.Models;
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
    class ProfileViewModel : DependencyObject
    {
        DataProvider dp = new DataProvider();
        public string FilterText
        {
            get { return (string)GetValue(FilterTextProperty); }
            set { SetValue(FilterTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FilterProduct.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterTextProperty =
            DependencyProperty.Register("FilterProfile", typeof(string), typeof(ProfileViewModel), new PropertyMetadata("", FilterText_Changed));

        private static void FilterText_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var current = d as ProfileViewModel;
            if (current != null)
            {
                current.Items1.Filter = null;
                current.Items1.Filter = current.FilterProfile;
            }
        }

        public ICollectionView Items1
        {
            get { return (ICollectionView)GetValue(Items1Property); }
            set { SetValue(Items1Property, value); }
        }


        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Items1Property =
            DependencyProperty.Register("Items1", typeof(ICollectionView), typeof(ProfileViewModel), new PropertyMetadata(null));

        public ProfileViewModel()
        {
            List<ModelProfile> modelProfiles = new List<ModelProfile>();
            //List<Service.Profile> profiles = ShopWindows.client.GetAllUsers().ToList();
            //foreach (Service.Profile profile in profiles)
            //{
            //    ModelProfile modelProfile = new ModelProfile();
            //    modelProfiles.Add(modelProfile.MakeModelProfile(profile));
            //}
            //Items1 = CollectionViewSource.GetDefaultView(modelProfiles);
            //Items1.Filter = FilterProfile;
        }

        private bool FilterProfile(object obj)
        {
            bool result = true;
            Service.Profile profile = obj as Service.Profile;
            if (!string.IsNullOrWhiteSpace(FilterText) && profile != null && !profile.Name.Contains(FilterText) && !profile.Login.Contains(FilterText))
            {
                result = false;
            }
            return result;
        }

    }
}


