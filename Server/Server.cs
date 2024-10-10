using System.Net.Sockets;
using System.Net;

using NetworkSocket;

namespace Server
{
    public class Server
    {
        private Socket _serverSocket;
        private IPAddress _serverIp;
        private int _serverPort;


        private Dictionary<string id, Socket socket> _clients;
        private int _maxClients;

        Postman<Packet> _postman;

        public Server(IPAddress ip, int port, int maxClients)
        {
            _serverIp = ip;
            _serverPort = port;
            _maxClients = maxClients;

            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _clients = new List<ClientData>();
            _postman = new Postman<Packet>(new Codec(), 1024);
        }

        public async Task StartAsync()
        {
            try
            {
                IPEndPoint endPoint = new IPEndPoint(_serverIp, _serverPort);

                _serverSocket.Bind(endPoint);
                _serverSocket.Listen(_maxClients);

                await AcceptClientsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        private async Task AcceptClientsAsync()
        {

            while (true)
            {
                try
                {
                    Socket clientSocket = await Task.Factory.FromAsync(
                        _serverSocket.BeginAccept,
                        _serverSocket.EndAccept,
                        null);

                    _ = Task.Run(async () =>
                    {
                        ClientData client = await AuthSocketAsync(clientSocket);
                        await HandleClientAsync(client);
                    });
                }
                catch(Exception ex) 
                {
                    Console.WriteLine("Exception: " + ex.Message);
                }
            }
        }

        private async Task<ClientData> AuthSocketAsync(Socket socket)
        {

             Packet packet = await _postman.ReceivePacketAsync(socket);


            ClientData client = new ClientData(socket, ip, username);

            lock (_clients) _clients.Add(client);

            Console.WriteLine($"{client.Ip}:{client.Username} connected");
            SendBroadcastPacket(new ServerPacket(ip, username, $"User {client.Ip}:{client.Username} has connected."));

            return client;
        }

        //private async Task HandleClientAsync(ClientData client)
        //{
        //    try
        //    {
        //        while (client.Socket.Connected)
        //        {
        //            ServerPacket packet = ReceivePacket(client);
        //            SendBroadcastPacket(packet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception: " + ex.Message);
        //    }
        //    finally
        //    {
        //        lock (_clients)
        //        {
        //            _clients.Remove(client);
        //        }

        //        client.Socket.Shutdown(SocketShutdown.Both);
        //        client.Socket.Close();

        //        Console.WriteLine($"{client.Ip}:{client.Username} has disconnected.");
        //        SendBroadcastPacket(new ServerPacket(_server.Ip, "Server", $"User {client.Ip}:{client.Username} has disconnected."));

        //    }
        //}


        //public void Stop()
        //{
        //    try
        //    {
        //        lock (_clients)
        //        {
        //            foreach (ClientData client in _clients)
        //            {
        //                client.Socket.Shutdown(SocketShutdown.Both);
        //                client.Socket.Close();
        //            }
        //            _clients.Clear();
        //        }
        //        _server.Socket.Shutdown(SocketShutdown.Both);
        //        _server.Socket.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception: " + ex.Message);
        //    }
        //}
    }
}
