using System.Net.Sockets;
using System.Net;

using NetworkSocket;
using Logger;

namespace Server
{
    public class Server
    {
        private readonly Socket _serverSocket;
        private readonly IPAddress _serverIp;
        private readonly int _serverPort;

        private readonly int _serverMaxConn;
        private string _serverPassword;

        private List<Socket> _clientSockets;
        private Dictionary<Socket, string> _usernamesDictionary;
        
        Postman<Packet> _postman;
        private ILogger _logger;

        private readonly SemaphoreSlim _semaphore;

        public Server(IPAddress ip, int port, int serverMaxConn, string serverPassword, ILogger logger)
        {
            _serverIp = ip;
            _serverPort = port;
            _serverMaxConn = serverMaxConn;
            _serverPassword = serverPassword;
            _logger = logger;

            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _clientSockets = new List<Socket>();
            _usernamesDictionary = new Dictionary<Socket, string>();

            _postman = new Postman<Packet>(new Codec(), 1024);
            _semaphore = new SemaphoreSlim(1, 1);
        }

        public async Task StartAsync()
        {
                IPEndPoint endPoint = new IPEndPoint(_serverIp, _serverPort);

                _serverSocket.Bind(endPoint);
                _serverSocket.Listen(_serverMaxConn);

                await AcceptClientsAsync();
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
                        _logger.LogInfo($"New socket({SocketUtils.GetIp(clientSocket)}) connectected.");
                        await AuthClientAsync(clientSocket);
                        await HandleClientAsync(clientSocket);
                    });
                }
                catch(Exception ex) 
                {
                    _logger.LogError(ex.Message);
                }
            }
        }

        private async Task AuthClientAsync(Socket socket)
        {
            Packet packet = await _postman.ReceivePacketAsync(socket);

            if(packet.Data == _serverPassword && !_usernamesDictionary.ContainsValue(packet.Sender))
            {
                _ = Task.Run(async () =>
                {
                await _semaphore.WaitAsync();
                try
                {
                    _clientSockets.Add(socket);
                    _usernamesDictionary.Add(socket, packet.Sender);
                }
                finally
                {
                    _semaphore.Release();
                }

                _logger.LogInfo($"Socket({SocketUtils.GetIp(socket)}) authenticated.");
                await _postman.SendPacketAsync(new Packet("Server", PacketType.Data, Status.Ok), socket);
                await SendBroadcastAsync(new Packet("packet.Sender", PacketType.Connected, ""));
                });
            }
            else
            {
                _logger.LogInfo($"Socket({SocketUtils.GetIp(socket)}) authentication rejected. Client: {packet.Sender}.");
                throw new Exception("Socket authentication rejected");
            }
            
        }

        private async Task HandleClientAsync(Socket socket)
        {
            try
            {
                while (socket.Connected)
                {
                    Packet packet = await _postman.ReceivePacketAsync(socket);

                    _ = Task.Run(async () =>
                    {
                        await _semaphore.WaitAsync();
                        try
                        {
                            
                            SendBroadcastAsync(packet);
                        }
                        finally
                        {
                            _semaphore.Release();
                        }

                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                await _semaphore.WaitAsync();
                try
                {
                    _clientSockets.Remove(socket);
                }
                finally
                {
                    _semaphore.Release();
                }


                socket.Shutdown(SocketShutdown.Both);
                socket.Close();

                _logger.LogInfo($"Socket({SocketUtils.GetIp(socket)}) disconnected.")
                SendBroadcastAsync(new Packet("Server", PacketType.Disconnected))

            }
        }

        public async Task SendBroadcastAsync(Packet packet)
        {
            await _semaphore.WaitAsync();
            try
            {
                await _postman.SendPacketAsync(new Packet("Server", Command.NewConnection, packet.Sender), _clientSockets);
            }
            finally
            {
                _semaphore.Release(); 
            }
        }


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
