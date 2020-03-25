using System;
using System.Collections.Generic;

namespace Service
{
    public partial class TGameGenre
    {
        public int Idgame { get; set; }
        public int Idgenre { get; set; }

        public virtual TProducts IdgameNavigation { get; set; }
        public virtual TGenre IdgenreNavigation { get; set; }
    }
}
