using NetworkSocket;
using System.Text;

namespace Server
{
    public class Codec : ICodec<Packet>
    {
        public byte[] Pack(Packet packet)
        {
            return Encoding.ASCII.GetBytes($"{packet.Sender}:{packet.Label}:{packet.Data}");
        }

        public Packet Unpack(byte[] data, int index, int count)
        {
            string str = Encoding.ASCII.GetString(data, index, count);
            string[] strs = str.Split(new char[] { ':' }, 2);
            
            if (strs.Length == 3) return new Packet(strs[0], strs[1], strs[2]);
            else throw new Exception("Invalid packet");
        }
    }
}
