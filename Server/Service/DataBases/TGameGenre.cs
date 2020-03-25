using System;
using System.Collections.Generic;

namespace Service
{
    public partial class TGameGenre
    {
        public int IdGame { get; set; }
        public int IdGenre { get; set; }

        public virtual TProducts IdGameNavigation { get; set; }
        public virtual TGenre IdGenreNavigation { get; set; }
    }
}
