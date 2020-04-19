﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Service
{
    public partial class TUsers
    {
        public TUsers()
        {
            TComments = new HashSet<TComments>();
            TDeals = new HashSet<TDeals>();
            TLogin = new HashSet<TLogin>();
            TModerateEmployers = new HashSet<TModerateEmployers>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string Mail { get; set; }
        public int AccessRight { get; set; }
        public double Money { get; set; }
        public double TotalSpentMoney { get; set; }
        public double PersonalDiscount { get; set; }

        public virtual ICollection<TComments> TComments { get; set; }
        public virtual ICollection<TDeals> TDeals { get; set; }
        public virtual ICollection<TLogin> TLogin { get; set; }
        public virtual ICollection<TModerateEmployers> TModerateEmployers { get; set; }
    }
}
