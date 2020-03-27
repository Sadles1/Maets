using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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
        public double? WholesalePrice { get; set; }
        public double RetailPrice { get; set; }
        public string Description { get; set; }
        public DateTime RequestDate { get; set; }
        public int IdDeveloper { get; set; }
        public int IdPublisher { get; set; }
        public bool? ResultModeration { get; set; }

        public virtual TDeveloper IdDeveloperNavigation { get; set; }
        public virtual TPublisher IdPublisherNavigation { get; set; }
        public virtual ICollection<TModerateEmployers> TModerateEmployers { get; set; }
    }
}
