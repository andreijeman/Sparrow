using System.Net;
using System.Net.Sockets;


namespace Network
{
    public static class SocketUtils
    {
        public static string GetIp(Socket socket)
        {
            if (socket?.RemoteEndPoint != null)
            {
                return ((IPEndPoint)socket.RemoteEndPoint).Address.ToString();
            }
            else throw new NullReferenceException();
        }
    }
}
