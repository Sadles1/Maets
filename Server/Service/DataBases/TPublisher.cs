using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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
        public virtual ICollection<TModerateProducts> TModerateProducts { get; set; }
        public virtual ICollection<TProducts> TProducts { get; set; }
    }
}
