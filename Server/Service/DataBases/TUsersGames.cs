using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public partial class TUsersGames
    {
        public int Iduser { get; set; }
        public int Idproduct { get; set; }

        public virtual TProducts IdproductNavigation { get; set; }
        public virtual TUsers IduserNavigation { get; set; }

    }
}
