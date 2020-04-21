using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Service
{
    public partial class TDeals
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int IdBuyers { get; set; }
        public int IdProduct { get; set; }
        public int Count { get; set; }
        public double BuyingPrice { get; set; }
        public bool Wholesale { get; set; }
        public virtual TUsers IdBuyersNavigation { get; set; }
        public virtual TProducts IdProductNavigation { get; set; }
    }
}
