using System.Net.Sockets;
using System.Net;
using Logger;
using Network;
using Server.Postman;

namespace Server;

public abstract class BaseServer
{
    protected readonly Socket _serverSocket;
    protected readonly IPEndPoint _serverIPEndPoint;

    public BaseServer(int port)
    {
        _serverIPEndPoint = new IPEndPoint(Dns.GetHostEntry(Dns.GetHostName()).AddressList[1], port);
        _serverSocket = new Socket(_serverIPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
    }

    public abstract void Start();
    public abstract void Stop();
}
