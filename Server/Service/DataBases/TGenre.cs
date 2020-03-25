﻿using System;
using System.Collections.Generic;

namespace Service
{
    public partial class TGenre
    {
        public TGenre()
        {
            TGameGenre = new HashSet<TGameGenre>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<TGameGenre> TGameGenre { get; set; }
    }
}
