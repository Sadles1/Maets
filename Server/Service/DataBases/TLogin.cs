using System;
using System.Collections.Generic;

namespace Service
{
    public partial class TLogin
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int Idowner { get; set; }

        public virtual TUsers IdownerNavigation { get; set; }
    }
}
