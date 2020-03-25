using System;
using System.Collections.Generic;

namespace Service
{
    public partial class TDeals
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Idbuyers { get; set; }
        public int Idproduct { get; set; }
        public string Count { get; set; }
        public bool Wholesale { get; set; }

        public virtual TUsers IdbuyersNavigation { get; set; }
        public virtual TProducts IdproductNavigation { get; set; }
    }
}
