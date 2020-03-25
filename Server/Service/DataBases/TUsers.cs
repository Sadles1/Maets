using System;
using System.Collections.Generic;

namespace Service
{
    public partial class TUsers
    {
        public TUsers()
        {
            TComments = new HashSet<TComments>();
            TDeals = new HashSet<TDeals>();
            TDeveloper = new HashSet<TDeveloper>();
            TLogin = new HashSet<TLogin>();
            TModerateEmployers = new HashSet<TModerateEmployers>();
            TPublisher = new HashSet<TPublisher>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string Mail { get; set; }
        public int AccessRight { get; set; }

        public virtual ICollection<TComments> TComments { get; set; }
        public virtual ICollection<TDeals> TDeals { get; set; }
        public virtual ICollection<TDeveloper> TDeveloper { get; set; }
        public virtual ICollection<TLogin> TLogin { get; set; }
        public virtual ICollection<TModerateEmployers> TModerateEmployers { get; set; }
        public virtual ICollection<TPublisher> TPublisher { get; set; }
    }
}
