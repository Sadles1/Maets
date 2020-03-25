using System;
using System.Collections.Generic;

namespace Service
{
    public partial class TPublisher
    {
        public TPublisher()
        {
            TModerateProducts = new HashSet<TModerateProducts>();
            TProducts = new HashSet<TProducts>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Iduser { get; set; }

        public virtual TUsers IduserNavigation { get; set; }
        public virtual ICollection<TModerateProducts> TModerateProducts { get; set; }
        public virtual ICollection<TProducts> TProducts { get; set; }
    }
}
