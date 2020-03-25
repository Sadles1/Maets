using System;
using System.Collections.Generic;

namespace Service
{
    public partial class TProducts
    {
        public TProducts()
        {
            TComments = new HashSet<TComments>();
            TDeals = new HashSet<TDeals>();
            TGameGenre = new HashSet<TGameGenre>();
            TMinGameSysReq = new HashSet<TMinGameSysReq>();
            TRecGameSysReq = new HashSet<TRecGameSysReq>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public double? Rate { get; set; }
        public int? WholesalePrice { get; set; }
        public int RetailPrice { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int? Quantity { get; set; }
        public int Iddeveloper { get; set; }
        public int Idpublisher { get; set; }

        public virtual TDeveloper IddeveloperNavigation { get; set; }
        public virtual TPublisher IdpublisherNavigation { get; set; }
        public virtual ICollection<TComments> TComments { get; set; }
        public virtual ICollection<TDeals> TDeals { get; set; }
        public virtual ICollection<TGameGenre> TGameGenre { get; set; }
        public virtual ICollection<TMinGameSysReq> TMinGameSysReq { get; set; }
        public virtual ICollection<TRecGameSysReq> TRecGameSysReq { get; set; }
    }
}
