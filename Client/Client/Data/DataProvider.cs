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

        public string HashPassword(string Password)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(Password);
            bytes = DigestUtilities.CalculateDigest("GOST3411-2012-512", bytes);
            string password = "";
            foreach (byte b in bytes)
                password += b;
            return password;
        }
        async public void Download(string path, int idGame)
        {
            await Task.Run(() =>
            {
                try
                {
                    DownloadServiceClient download = new DownloadServiceClient("NetTcpBinding_IDownloadService");
                    Stream stream = download.DownloadProduct(idGame);
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
                   // ZipFile.ExtractToDirectory($@"{path}.zip", path);

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
