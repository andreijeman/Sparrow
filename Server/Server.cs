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
        private Dictionary<Socket, string> _sendersDictionary;
        
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
            _sendersDictionary = new Dictionary<Socket, string>();

            _postman = new Postman<Packet>(new Codec(), 1024);
            _semaphore = new SemaphoreSlim(1, 1);
        }

        public void Start()
        {
                IPEndPoint endPoint = new IPEndPoint(_serverIp, _serverPort);

                _serverSocket.Bind(endPoint);
                _serverSocket.Listen(_serverMaxConn);

                _ = Task.Run(async() => await AcceptClientsAsync());
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

            if(packet.Data == _serverPassword && !_sendersDictionary.ContainsValue(packet.Sender))
            {
                _ = Task.Run(async () =>
                {
                    _logger.LogInfo($"Socket({SocketUtils.GetIp(socket)}) authenticated.");
                    await AddClient(socket, packet.Sender);
                    await _postman.SendPacketAsync(new Packet("Server", Label.Data, Status.Ok), socket);
                    await SendBroadcastAsync(new Packet("packet.Sender", Label.Connected, ""));
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
            string sender = _sendersDictionary[socket];

            while (socket.Connected)
            {
                try
                {
                    Packet packet = await _postman.ReceivePacketAsync(socket);
                    packet = new Packet(sender, Label.Message, packet.Data);
                    _ = Task.Run(async () => await SendBroadcastAsync(packet));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }

            }
            
            await RemoveClient(socket);

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();

            _logger.LogInfo($"Socket({SocketUtils.GetIp(socket)}) disconnected.");
            await SendBroadcastAsync(new Packet(sender, Label.Disconnected, ""));
        }

        public async Task SendBroadcastAsync(Packet packet)
        {
            await _semaphore.WaitAsync();
            try
            {
                await _postman.SendPacketAsync(new Packet("Server", Label.Message, packet.Sender), _clientSockets);
            }
            finally
            {
                _semaphore.Release(); 
            }
        }

        public async Task AddClient(Socket socket, string sender)
        {
            await _semaphore.WaitAsync();
            try
            {
                _clientSockets.Add(socket);
                _sendersDictionary.Add(socket, sender);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task RemoveClient(Socket socket)
        {
            await _semaphore.WaitAsync();
            try
            {
                _clientSockets.Remove(socket);
                _sendersDictionary.Remove(socket);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
