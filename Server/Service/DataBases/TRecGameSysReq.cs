using System;
using System.Collections.Generic;

namespace Service
{
    public partial class TRecGameSysReq
    {
        public int IdGame { get; set; }
        public int IdSysReq { get; set; }
        public string Description { get; set; }

        public virtual TProducts IdGameNavigation { get; set; }
        public virtual TSysReq IdSysReqNavigation { get; set; }
    }
}
