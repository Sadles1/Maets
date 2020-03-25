using System;
using System.Collections.Generic;

namespace Service
{
    public partial class TModerateProducts
    {
        public TModerateProducts()
        {
            TModerateEmployers = new HashSet<TModerateEmployers>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? WholesalePrice { get; set; }
        public int RetailPrice { get; set; }
        public string Description { get; set; }
        public DateTime RequestDate { get; set; }
        public int Iddeveloper { get; set; }
        public int Idpublisher { get; set; }
        public bool? ResultModeration { get; set; }

        public virtual TDeveloper IddeveloperNavigation { get; set; }
        public virtual TPublisher IdpublisherNavigation { get; set; }
        public virtual ICollection<TModerateEmployers> TModerateEmployers { get; set; }
    }
}
