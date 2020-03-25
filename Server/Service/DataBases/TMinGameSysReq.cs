using System;
using System.Collections.Generic;

namespace Service
{
    public partial class TMinGameSysReq
    {
        public int Idgame { get; set; }
        public int IdsysReq { get; set; }
        public string Description { get; set; }

        public virtual TProducts IdgameNavigation { get; set; }
        public virtual TSysReq IdsysReqNavigation { get; set; }
    }
}
