using System.Net.Sockets;

namespace Server
{
    public record ServerData(Socket Socket, string Ip, int Port, int MaxClients);
}
