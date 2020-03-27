using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public partial class TOnlineUsers
    {
        public int Id { get; set; }
        public virtual TUsers IdUsersNavigation { get; set; }
    }
}
