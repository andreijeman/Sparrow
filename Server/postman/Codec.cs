using System.Text;
using Network;

namespace Server.Postman;

public class Codec : ICodec<Packet>
{
    public byte[] Pack(Packet packet)
    {
        return Encoding.UTF8.GetBytes($"{packet.Sender}:{(int)packet.Label}:{packet.Data}");
    }

    public Packet Unpack(byte[] data, int index, int count)
    {
        string str = Encoding.UTF8.GetString(data, index, count);
        string[] strs = str.Split(new char[] { ':' }, 3);

        if (strs.Length > 2 && Enum.TryParse(typeof(Label), strs[1], out var label))
        {
            return new Packet { Sender = strs[0], Label = (Label)label, Data = strs[2] };
        }
        else throw new Exception("Invalid packet");
    }
}
