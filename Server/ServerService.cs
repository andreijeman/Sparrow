using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO.Pipes;

namespace Server
{
    public class ServerService
    {
        private ServerData _server;
        private List<ClientData> _clients = new List<ClientData>();

        public ServerService(ServerData server)
        {
            _server = server;
        }

        public void Start()
        {
            try
            {
                IPEndPoint endPoint = new IPEndPoint(Dns.GetHostEntry(Dns.GetHostName()).AddressList[1],
                                                    _server.Port);

                _server.Socket.Bind(endPoint);
                _server.Socket.Listen(_server.MaxClients);

                Task.Run(() => AcceptClientsAsync());
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        private async Task AcceptClientsAsync()
        {
            try
            {
                while (true)
                {
                    Socket clientSocket = await Task.Factory.FromAsync(
                        _server.Socket.BeginAccept,
                        _server.Socket.EndAccept,
                        null);

                    _ = Task.Run(() =>
                    {
                        ClientData client = ReceiveClientData(clientSocket);
                        
                        Console.WriteLine($"{client.Ip}:{client.Username} connected");
                        
                        
                        lock (_clients) _clients.Add(client);
                        
                        SendBroadcastMessage(FormatMessage(client.Ip, client.Username, $"User {client.Ip}:{client.Username} has connected."));
                        
                        HandleClient(client);

                    });
                }
            }
            catch(Exception ex) 
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        private void HandleClient(ClientData client)
        {
            try
            {
                while (client.Socket.Connected)
                {
                    string message = ReceiveMessage(client.Socket);
                    if(message is { }) SendBroadcastMessage(FormatMessage(client.Ip, client.Username, message));
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
                    _clients.Remove(client);
                }

                client.Socket.Shutdown(SocketShutdown.Both);
                client.Socket.Close();

                Console.WriteLine($"{client.Ip}:{client.Username} has disconnected.");
                SendBroadcastMessage(FormatMessage(_server.Ip, "Server", $"User {client.Ip}:{client.Username} has disconnected."));

            }
        }

        private ClientData ReceiveClientData(Socket socket)
        {
            try
            {
                string? name = ReceiveMessage(socket);
 
                string ip = ((IPEndPoint)socket.RemoteEndPoint!).Address.ToString();
            
                return new ClientData(socket, ip, name!);
            }
            catch
            {
                Console.WriteLine($"An incoming  connection from {((IPEndPoint)socket.RemoteEndPoint!).Address} failed.");
                throw;
            }
        }

        public string ReceiveMessage(Socket socket)
        { 
            byte[] buffer = new byte[1024];

            try
            {
                int count = socket.Receive(buffer);
                return  Encoding.ASCII.GetString(buffer, 0, count);
            }
            catch
            {
                Console.WriteLine($"Receive message from {((IPEndPoint)socket.RemoteEndPoint!).Address} failed.");
                throw;
            }
        }

        private void SendMessage(Socket socket, string message)
        {
            try
            {
                socket.Send(Encoding.ASCII.GetBytes(message));
            }
            catch
            {
                Console.WriteLine($"Send message to {((IPEndPoint)socket.RemoteEndPoint!).Address} failed.");
                throw;
            }
        }

        private void SendBroadcastMessage(string message)
        {
            lock (_clients)
            {
                foreach (ClientData client in _clients)
                {
                    SendMessage(client.Socket, message);
                }
            }
        }

        public void Stop()
        {
            try
            {
                lock (_clients)
                {
                    foreach (ClientData client in _clients)
                    {
                        client.Socket.Shutdown(SocketShutdown.Both);
                        client.Socket.Close();
                    }
                    _clients.Clear();
                }
                _server.Socket.Shutdown(SocketShutdown.Both);
                _server.Socket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        private static string FormatMessage(string ip, string username, string text)
        {
            return string.Format(Config.MessagePattern, ip, username, text);
        }
    }
}
