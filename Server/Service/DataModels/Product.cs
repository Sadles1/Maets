using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Service
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double? Rate { get; set; }
        public byte[] MainImage { get; set; }
        public List<string> GameGenre { get; set; }
        public List<string> MinGameSysReq { get; set; }
        public List<string> RecGameSysReq { get; set; }
        public double WholesalePrice { get; set; }
        public double RetailPrice { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }
    }
}
