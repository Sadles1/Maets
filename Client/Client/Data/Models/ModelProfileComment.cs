using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Client
{
    public class ModelProfileComment : Service.Profile
    {
        public ImageSource Image { get; set; }
        public string Comment { get; set; }
        public string Star { get; set; }
        public ModelProfileComment MakeModelProfileComment(Tuple<Service.Comment,Service.Profile>  profile)
        {
            DataProvider dp = new DataProvider();
            ModelProfileComment modelProfile = new ModelProfileComment();
            modelProfile.Image = dp.GetImageFromByte(profile.Item2.MainImage);
            modelProfile.ID = profile.Item2.ID;
            modelProfile.Login = profile.Item2.Login;
            modelProfile.Comment = profile.Item1.comment;
            modelProfile.Star = Convert.ToString(profile.Item1.Score) +@"\5";
            return modelProfile;
        }
    }
}
