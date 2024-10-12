using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public static class PacketType
    {
        public const string Data = "Data";
        public const string Connected = "Connected";
        public const string Disconnected = "Diconnected";
        public const string Message = "Message";
        public const string MyMessage = "MyMessage";
    }
}
