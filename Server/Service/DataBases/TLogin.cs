using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Service
{
    public partial class TLogin
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }      
        public virtual TUsers IdNavigation { get; set; }
    }
}
