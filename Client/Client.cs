using System.Net.Sockets;
using System.Net;

using Network;
using Server.Postman;
using Logger;

namespace Client
{
    public class Client
    {
        private Socket _socket;
        private Postman<Packet> _postman;
        private ILogger _logger;

        public delegate void PacketReceivedEventHandler(Packet packet);
        public event PacketReceivedEventHandler? PacketReceivedEvent;

        public delegate void ConnectionEventHandler(bool isConnected);
        public event ConnectionEventHandler? ConnectionEvent;

        public Client(ILogger logger)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _postman = new Postman<Packet>(new Codec(), 1024);
            _logger = logger;
        }

        public async Task ConnectToServerAsync(string ip, string port, string password, string username)
        {
            try
            {
                _socket.Connect(new IPEndPoint(IPAddress.Parse(ip), int.Parse(port)));
                await _postman.SendPacketAsync(new Packet(username, Label.Data, password), _socket);
                Packet packet = await _postman.ReceivePacketAsync(_socket);

                ConnectionEvent?.Invoke(packet.Data == Status.Ok);

                await ReceivePacketsAsync();
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task ReceivePacketsAsync()
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
                await _postman.SendPacketAsync(packet, _socket);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
