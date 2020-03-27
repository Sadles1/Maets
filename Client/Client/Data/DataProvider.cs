using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Client
{
    class DataProvider
    {
        public ImageSource GetImageFromByte(byte[] buffer)
        {
            using (var ms = new MemoryStream(buffer))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        public string HashPassword(string Password)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(Password);
            bytes = DigestUtilities.CalculateDigest("GOST3411-2012-512", bytes);
            string password = "";
            foreach (byte b in bytes)
                password += b;
            return password;
        }
    }
}
