using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Client
{
    public class ModelImage:Service.Product
    {

        DataProvider dp = new DataProvider();
        public ModelImage MakeModelImage(byte[] profile)
        {
            DataProvider dp = new DataProvider();
            ModelImage modelProfile = new ModelImage();
            modelProfile.MainImage = profile;
            return modelProfile;
        }
        
    }
}
