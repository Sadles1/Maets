using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Service
{
    public class Product
    {
        public int Id { get; set; }
        public byte[] MainImage { get; set; }
        public List<byte[]> Screenshots { get; set; }
        public string Name { get; set; }
        public double? WholesalePrice { get; set; }
        public double RetailPrice { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }
    }
}
