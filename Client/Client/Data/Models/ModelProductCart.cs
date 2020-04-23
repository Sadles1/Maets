using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Client
{
    public class ModelProductCart:Service.Product
    {
        public ImageSource Image { get; set; }
        public int How {get;set;}
        public double Price {get;set;}
        public ModelProductCart MakeModelProductCart(Service.Product product)
        {
            DataProvider dp = new DataProvider();
            ModelProductCart modelProduct = new ModelProductCart();
            modelProduct.Id = product.Id;
            modelProduct.Name = product.Name;
            modelProduct.Image = dp.GetImageFromByte(product.MainImage);
            modelProduct.How = 1;
            modelProduct.Price = 0;
            modelProduct.RetailPrice = product.RetailPrice;
            modelProduct.WholesalePrice = product.WholesalePrice;
            return modelProduct;
        }
    }
}
