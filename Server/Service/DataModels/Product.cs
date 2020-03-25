using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class Product
    {
        public int Id { get; set; }
        public Bitmap MainImage { get; set; }
        public List<Bitmap> Screenshots { get; set; }
        public string Name { get; set; }
        public int WholesalePrice { get; set; }
        public int RetailPrice { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }
    }
}
