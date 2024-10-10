using NetworkSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Codec : ICodec<Packet>
    {
        public byte[] Pack(Packet packet)
        {
            throw new NotImplementedException();
        }

        public Packet Unpack(byte[] data, int index, int count)
        {
            throw new NotImplementedException();
        }
    }
}
