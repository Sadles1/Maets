﻿ using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class UserMessage
    {
        public int IDSender { get; set; }
        public int IDReceiver { get; set; }
        public string message { get; set; }
        public DateTime date { get; set; }
        public bool isRead { get; set; }
    }
}
