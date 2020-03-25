using System;
using System.Collections.Generic;

namespace Service
{
    public partial class TComments
    {
        public int Id { get; set; }
        public int Iduser { get; set; }
        public int Score { get; set; }
        public string Comment { get; set; }
        public int Idproduct { get; set; }

        public virtual TProducts IdproductNavigation { get; set; }
        public virtual TUsers IduserNavigation { get; set; }
    }
}
