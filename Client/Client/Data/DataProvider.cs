using Client.Service;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        public byte[] GetByteFromImage(string file)
        {
                byte[] data;
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                BitmapFrame image = BitmapFrame.Create(new Uri(file));//В изображение записываем массив байтов
                encoder.Frames.Add(image);
                using (MemoryStream ms = new MemoryStream())
                {
                    encoder.Save(ms);
                    data = ms.ToArray();
                }

            return data; 
            
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


