using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Server
{
    public class Server
    {
        private Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private readonly int _port;
        private readonly List<Socket> _clients = new List<Socket>();

        public Server(int port)
        {
            _port = port;
        }

        public void Start()
        {
            try
            {
                IPEndPoint endPoint = new IPEndPoint(Dns.GetHostEntry(Dns.GetHostName()).AddressList[1], _port);
                Console.WriteLine(Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString());
                _serverSocket.Bind(endPoint);
                _serverSocket.Listen(10);

                Task.Run(() => AcceptClientsAsync());
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
                        null
                    );

                    lock (_clients)
                    {
                        _clients.Add(clientSocket);
                        Console.WriteLine("Conected: " + ((IPEndPoint)clientSocket.RemoteEndPoint).Address.ToString());
                    }
                    _ = Task.Run(() => HandleClient(clientSocket));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                }
            }
        }

        private void HandleClient(Socket clientSocket)
        {
            string? name = ReceiveMessage(clientSocket);
            string id = _clients.Count.ToString();
            SendMessage(clientSocket, id);
            
            try
            {
                byte[] buffer = new byte[1024];
                string? message;

                while (clientSocket.Connected)
                {
                    message = ReceiveMessage(clientSocket);
                    if(message is { }) BroadcastMessage(id +"?" + name + "?" + message);
                    message = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                lock (_clients)
                {
                    _clients.Remove(clientSocket);
                }

                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
        }

        public string? ReceiveMessage(Socket clientSocket)
        {
            string? message = null;
            try
            {
                byte[] buffer = new byte[1024];
                int count = clientSocket.Receive(buffer);
                message = Encoding.ASCII.GetString(buffer, 0, count);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }

            return message;
        }
        private void BroadcastMessage(string message)
        {
            lock (_clients)
            {
                foreach (Socket client in _clients)
                {
                    SendMessage(client, message); 
                }
            }
        }

        private void SendMessage(Socket clientSocket, string message)
        {
            try
            {
                clientSocket.Send(Encoding.ASCII.GetBytes(message));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:" + ex.Message);
            }
        }

        public void Stop()
        {
            try
            {
                lock (_clients)
                {
                    foreach (Socket client in _clients)
                    {
                        client.Shutdown(SocketShutdown.Both);
                        client.Close();
                    }
                    _clients.Clear();
                }
                _serverSocket.Shutdown(SocketShutdown.Both);
                _serverSocket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }
    }
}
