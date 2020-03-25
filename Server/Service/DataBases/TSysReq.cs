using System;
using System.Collections.Generic;

namespace Service
{
    public partial class TSysReq
    {
        public TSysReq()
        {
            TMinGameSysReq = new HashSet<TMinGameSysReq>();
            TRecGameSysReq = new HashSet<TRecGameSysReq>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<TMinGameSysReq> TMinGameSysReq { get; set; }
        public virtual ICollection<TRecGameSysReq> TRecGameSysReq { get; set; }
    }
}
