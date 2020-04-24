using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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
            TUsersGames = new HashSet<TUsersGames>();
            TProductKeys = new HashSet<TProductKeys>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public double? Rate { get; set; }
        public double WholesalePrice { get; set; }
        public double RetailPrice { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Quantity { get; set; }
        public int IdDeveloper { get; set; }
        public int IdPublisher { get; set; }

        public virtual TDeveloper IdDeveloperNavigation { get; set; }
        public virtual TPublisher IdPublisherNavigation { get; set; }
        public virtual ICollection<TUsersGames> TUsersGames { get; set; }
        public virtual ICollection<TProductKeys> TProductKeys { get; set; }
        public virtual ICollection<TComments> TComments { get; set; }
        public virtual ICollection<TDeals> TDeals { get; set; }
        public virtual ICollection<TGameGenre> TGameGenre { get; set; }
        public virtual ICollection<TMinGameSysReq> TMinGameSysReq { get; set; }
        public virtual ICollection<TRecGameSysReq> TRecGameSysReq { get; set; }
    }
}
