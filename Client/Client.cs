using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;


using NetworkSocket;
using Server;
using Logger;
using System.Globalization;

namespace Client
{
    public class Client
    {
        private Socket socket;
        private Postman<Packet> _postman;
        private ILogger _logger;

        public delegate void PacketReceivedEventHandler(Packet packet);
        public event PacketReceivedEventHandler? PacketReceivedEvent;

        public Client(ILogger logger)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _postman = new Postman<Packet>(new Codec(), 1024);
            _logger = logger;
        }

        public async Task<bool> ConnectToServerAsync(IPAddress ip, int port, string password, string username)
        {
            try
            {
                socket.Connect(new IPEndPoint(ip, port));

                await _postman.SendPacketAsync(new Packet(username, Label.Data, password), socket);
                Packet packet = await _postman.ReceivePacketAsync(socket);
                
                return packet.Data == Status.Ok;
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public void Start()
        {
            Task.Run(async() => await ReceivePacketsAsync());
        }

        public async Task ReceivePacketsAsync()
        {
            while (true)
            {
                try
                {
                    Packet packet = await _postman.ReceivePacketAsync(socket);
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
            await _postman.SendPacketAsync(packet, socket);
        }
    }
}
