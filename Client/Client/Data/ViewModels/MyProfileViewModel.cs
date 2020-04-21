
using Client.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Client.Data.ViewModels
{
   
        class MyProfileViewModel : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            private string name;

            public string Name
            {
                get => name;
                set
                {
                    name = value; OnPropertyChanget(nameof(Name));
                }
            }

            public MyProfileViewModel()
            {
            ModelProfile modelProfile = new ModelProfile();
            }

            private void OnPropertyChanget(string name)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

    }

