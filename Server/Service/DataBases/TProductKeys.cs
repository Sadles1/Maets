using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public partial class TProductKeys
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string LicenseKey { get; set; }
        public int IdProduct { get; set; }
        public bool isSold { get; set; }
        public bool isActivate { get; set; }
        public virtual TProducts IdProductNavigation { get; set; }
    }
}
