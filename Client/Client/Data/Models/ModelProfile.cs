using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Client.Data.Models
{
    public class ModelProfile: Service.Profile
    {
        public ImageSource Image { get; set; }
        public ModelProfile MakeModelProfile(Service.Profile profile)
        {
            DataProvider dp = new DataProvider();
            ModelProfile modelProfile = new ModelProfile();
            modelProfile.Name = profile.Name;
            modelProfile.Image = dp.GetImageFromByte(profile.MainImage);
            modelProfile.Mail = profile.Mail;
            modelProfile.ID = profile.ID;
            modelProfile.Login = profile.Login;
            modelProfile.Friends = profile.Friends;
            modelProfile.Games = profile.Games;
            modelProfile.AccessRight = profile.AccessRight;
            modelProfile.Telephone = profile.Telephone;
            modelProfile.status = profile.status;
            modelProfile.Money = profile.Money;
            modelProfile.Discount = profile.Discount;
            return modelProfile;
        }
    }
}
