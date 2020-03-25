using System;
using System.Collections.Generic;

namespace Service
{
    public partial class TModerateEmployers
    {
        public int IdEmployee { get; set; }
        public int IdModerateProduct { get; set; }
        public bool? Result { get; set; }

        public virtual TUsers IdEmployeeNavigation { get; set; }
        public virtual TModerateProducts IdModerateProductNavigation { get; set; }
    }
}
