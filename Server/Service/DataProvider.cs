using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Service
{
    class DataProvider
    {
        public static Product FormProduct(TProducts TProduct)
        {
            Product product = new Product();
            product.Id = TProduct.Id;
            product.Name = TProduct.Name;
            product.Publisher = TProduct.IdpublisherNavigation.Name;
            product.ReleaseDate = TProduct.ReleaseDate.Date;
            product.RetailPrice = TProduct.RetailPrice;
            product.MainImage = GetByteImage(BitmapFrame.Create(new Uri($@"C:\Users\snayp\Documents\GitHub\MTPProject\Server\Source\Products\1\Screenshots\MainImage.jpg")));
            product.Description = TProduct.Description;
            product.Developer = TProduct.IddeveloperNavigation.Name;
            return product;
        }
        public static byte[] GetByteImage(BitmapFrame image)
        {
            byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(image);
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }
            return data;
        }
    }
}
