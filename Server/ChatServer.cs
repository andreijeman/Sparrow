using Network;
using Server.Postman;
using System.Net.Sockets;
using System.Net;
using Logger;

namespace Server;

public class ChatServer : BaseServer
{
    private Dictionary<Socket, string> _clientDict;

    private readonly int _serverMaxClients;
    private readonly string _serverPassword;

    private readonly ILogger _logger;

    private readonly Postman<Packet> _postman = new Postman<Packet>(new Codec(), 1024);
    private readonly SemaphoreSlim _clientSemaphore = new SemaphoreSlim(1, 1);

    public IPAddress IP { get => _serverIPEndPoint.Address; }
    public int Port { get => _serverIPEndPoint.Port; }

    public ChatServer(ILogger logger, int port, int maxClients = 8, string password = "") : base(port)
    {
        _clientDict = new Dictionary<Socket, string>();
        _serverMaxClients = maxClients;
        _serverPassword = password;
        _logger = logger;
    }

    public override void Start()
    {
        _serverSocket.Bind(_serverIPEndPoint);
        _serverSocket.Listen(_serverMaxClients);

        Task.Run(AcceptSocketsAsync);
    }

    public override void Stop()
    {

        _clientSemaphore.Wait();

        try
        {
            foreach(Socket socket in _clientDict.Keys)
            {
                socket.ShutdownAndClose();
                socket.Dispose();
            }
            
            _clientDict.Clear();
        }
        finally
        {
            _clientSemaphore.Release();
        }
    }

    private async Task AcceptSocketsAsync()
    {
        while (true)
        {
            try
            {
                Socket clientSocket = await _serverSocket.AcceptAsync();
                _ = Task.Run(async () => await AuthSocketAsync(clientSocket));

                _logger.LogInfo($"New client connected -> {clientSocket.GetIp()}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }

    private async Task AuthSocketAsync(Socket clientSocket)
    {
        try
        {
            Packet packet = await _postman.ReceivePacketAsync(clientSocket);

            if (packet.Data != _serverPassword)
            {
                await _postman.SendPacketAsync(clientSocket, new Packet { Label = Label.AuthFailed, Data = "Incorrect password" });

                _logger.LogInfo($"Client failed auth (incorrect password) -> {clientSocket.GetIp()}");
            }
            else if (_clientDict.ContainsValue(packet.Sender))
            {
                await _postman.SendPacketAsync(clientSocket, new Packet { Label = Label.AuthSucceded, Data = "Reserved username" });

                _logger.LogInfo($"Client failed auth (reserved name) -> {clientSocket.GetIp()}");
            }
            else
            {
                await _postman.SendPacketAsync(clientSocket, new Packet { Label = Label.AuthSucceded });
                await SendBroadcastAsync(new Packet { Label = Label.UserConnected, Sender = packet.Sender });
                await AddClientAsync(clientSocket, packet.Sender);

                _logger.LogInfo($"Client authenticated -> {clientSocket.GetIp()} - {packet.Sender}");

                _ = Task.Run(async () => await HandleClientAsync(clientSocket, packet.Sender));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            clientSocket.ShutdownAndClose();
        }
    }

    private async Task HandleClientAsync(Socket clientSocket, string clientUsername)
    {
        while (clientSocket.Connected)
        {
            try
            {
                Packet packet = await _postman.ReceivePacketAsync(clientSocket);
                packet.Sender = clientUsername;

                await SendBroadcastAsync(packet);

                if (packet.Label == Label.UserMessage)
                {
                    _logger.LogInfo($"Message from user - {packet.Sender}: {packet.Data}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

        }

        await RemoveClientAsync(clientSocket);
        await SendBroadcastAsync(new Packet { Label = Label.UserDiconnected, Sender = clientUsername });
        _logger.LogInfo($"Client disconnected -> {clientSocket.GetIp()} - {clientUsername}");
    }

    public async Task SendBroadcastAsync(Packet packet)
    {
        await _clientSemaphore.WaitAsync();

        try
        {
            List<Task> tasks = new List<Task>();

            foreach (Socket socket in _clientDict.Keys)
            {
                tasks.Add(_postman.SendPacketAsync(socket, packet));
            }

            await Task.WhenAll(tasks);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
        finally
        {
            _clientSemaphore.Release();
        }
    }

    private async Task AddClientAsync(Socket socket, string Username)
    {
        await _clientSemaphore.WaitAsync();

        try
        {
            _clientDict.Add(socket, Username);
        }
        finally
        {
            _clientSemaphore.Release();
        }
    }

    private async Task RemoveClientAsync(Socket socket)
    {
        await _clientSemaphore.WaitAsync();

        try
        {
            _clientDict.Remove(socket);
        }
        finally
        {
            _clientSemaphore.Release();
        }
    }
}
