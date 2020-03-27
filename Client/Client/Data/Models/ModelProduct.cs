using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Client
{
    public class ModelProduct : Service.Product
    {
        public ImageSource Image { get; set; }
        public ModelProduct MakeModelProduct(Service.Product product)
        {
            DataProvider dp = new DataProvider();
            ModelProduct modelProduct = new ModelProduct();
            modelProduct.Name = product.Name;
            modelProduct.Image = dp.GetImageFromByte(product.MainImage);
            modelProduct.Description = product.Description;
            return modelProduct;
        }
    }
}
