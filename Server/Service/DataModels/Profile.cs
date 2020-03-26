using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class Profile
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public byte[] MainImage { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Telephone { get; set; }
        public double? Money { get; set; }
        public double? Discount { get; set; }
    }
}
