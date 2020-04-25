using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Client
{
    public class ModelProfileMessage : Service.Profile
    {
        public string message { get; set; }
        public ImageSource Image { get; set; }
        public ModelProfileMessage MakeModelProfileMessage(Service.Profile profile, string mes)
        {
            DataProvider dp = new DataProvider();
            ModelProfileMessage modelProfile = new ModelProfileMessage();
            modelProfile.Name = profile.Name;
            modelProfile.Image = dp.GetImageFromByte(profile.MainImage);
            modelProfile.ID = profile.ID;
            modelProfile.Login = profile.Login;
            modelProfile.status = profile.status;
            modelProfile.message = mes;
            return modelProfile;
        }
    }
}
