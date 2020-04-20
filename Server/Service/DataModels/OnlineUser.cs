using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class OnlineUser
    {
        public Profile UserProfile { get; set; }
        public OperationContext operationContext { get; set; }
    }
}
