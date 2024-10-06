using System.Net.Sockets;

namespace Server
{
    public record ClientData(Socket Socket, string Ip, string Username);
    
}
