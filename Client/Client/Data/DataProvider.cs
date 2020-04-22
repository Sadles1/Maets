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
         public async Task<Service.Profile> GetProfileAsync(int idUser)
        {
          
            Service.Profile pr = ShopWindows.client.CheckProfile(idUser);
            return pr;
        }    
        //public async Task<Service.Profile> GetProfileAsync(int idUser)
        //{
        //    var respounce = await GetProfileAsync(idUser);
        //    return respounce;
        //}
        async public void Download(string path, int idGame, int idUser)
        {
            await Task.Run(() =>
            {
                try
                {
                    DownloadServiceClient download = new DownloadServiceClient("NetTcpBinding_IDownloadService");
                    Stream stream = download.DownloadProduct(idUser,idGame,path);
                    byte[] buffer = new byte[16 * 1024];
                    byte[] data;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int read;
                        while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }
                        data = ms.ToArray();
                    }
                    File.WriteAllBytes($@"{path}.zip", data);
                    //string dbDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DB");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    ZipFile.ExtractToDirectory($@"{path}.zip", path);
                    
                    MessageBox.Show("Загрузка завершена");

                    download.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }

    }
}


