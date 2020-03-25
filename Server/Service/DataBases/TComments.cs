using System;
using System.Collections.Generic;

namespace Service
{
    public partial class TComments
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public int Score { get; set; }
        public string Comment { get; set; }
        public int IdProduct { get; set; }

        public virtual TProducts IdProductNavigation { get; set; }
        public virtual TUsers IdUserNavigation { get; set; }
    }
}
