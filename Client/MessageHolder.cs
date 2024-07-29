using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public static class MessageHolder
    {
        public static Queue<string> SendQueue = new Queue<string>(); 
        public static Queue<string> ReceiveQueue  = new Queue<string>(); 
    }
}
