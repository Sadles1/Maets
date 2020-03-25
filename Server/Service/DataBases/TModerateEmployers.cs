using System;
using System.Collections.Generic;

namespace Service
{
    public partial class TModerateEmployers
    {
        public int Idemployee { get; set; }
        public int IdmoderateProduct { get; set; }
        public bool? Result { get; set; }

        public virtual TUsers IdemployeeNavigation { get; set; }
        public virtual TModerateProducts IdmoderateProductNavigation { get; set; }
    }
}
