using Logger;
using Network;
using Server.Postman;
using System.Net.Sockets;
using System.Net;

namespace Client;

public class ChatClient
{
    private readonly Socket _socket;

    private ILogger _logger;

    private readonly Postman<Packet> _postman = new Postman<Packet>(new Codec(), 1024);

    public event Action<Packet>? PacketReceivedEvent;

    public ChatClient(ILogger logger)
    {
        _logger = logger;
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }

    public async Task<bool> ConnectToServerAsync(IPAddress ip, int port, string password, string username)
    {
        try
        {
            _socket.Connect(new IPEndPoint(ip, port));

            await _postman.SendPacketAsync(_socket, new Packet { Sender = username, Data = password});

            Packet packet = await _postman.ReceivePacketAsync(_socket);

            if (packet.Label == Label.AuthSucceded)
            {
                _logger.LogInfo("Auth succeded");
                return true;
            }

            _logger.LogInfo($"Auth failed -> {packet.Data}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
        return false;
    }

    public async Task StartReceivePacketsAsync()
    {
        while (_socket.Connected)
        {
            try
            {
                Packet packet = await _postman.ReceivePacketAsync(_socket);
                PacketReceivedEvent?.Invoke(packet);
            }
            catch (Exception ex)
            {
                {
                    _logger.LogError(ex.Message);
                }
            }
        }
    }

    public async Task SendPacketAsync(Packet packet)
    {
        try
        {
            await _postman.SendPacketAsync(_socket, packet);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }
}
