using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public partial class TUsersGames
    {
        public int IdUser { get; set; }
        public int IdProduct { get; set; }

        public virtual TProducts IdProductNavigation { get; set; }
        public virtual TUsers IdUsersNavigation { get; set; }

    }
}
