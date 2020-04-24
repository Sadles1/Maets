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
        public string Key { get; set; }
        public int IdProduct { get; set; }
        public bool IsSold { get; set; }
        public bool IsActivate { get; set; }

        public virtual TProducts IdProductNavigation { get; set; }
    }
}
