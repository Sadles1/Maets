using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class Comment
    {
        public int id { get; set; }
        public int idUser { get; set; }
        public int idProduct { get; set; }
        public string comment { get; set;}
        public int Score { get; set; }
    }
}
