using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Linq.Expressions;

namespace Client
{
    public class ClientService
    {
        private Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private delegate void OnReceivePacket();

        public void ConnectToServer(IPAddress ip, int port, string username)
        {
            try
            {
                IPEndPoint serverEndPoint = new IPEndPoint(ip, port);
                socket.Connect(serverEndPoint);
                SendMessage(username);
            }
            catch
            {
                throw new Exception("Connection failed");
            }
        }

        public void Start()
        {
            Task.Run(() => ReceiveMessageLoop());
        }


        public void SendMessageLoop()
        {
            string? message;
            while(true)
            {
                message = null; 

                lock(MessageHolder.SendQueue)
                {
                    if (MessageHolder.SendQueue.Count > 0)
                    {
                        message = MessageHolder.SendQueue.Dequeue();
                    }
                }

                if (!string.IsNullOrEmpty(message))
                {
                    try
                    {
                        SendMessage(message);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Exception: " + ex.Message);
                    }
                }
            }

        }

        public void SendMessage(string message)
        {
            try
            {
                socket.Send(Encoding.ASCII.GetBytes(message));
            }
            catch
            {
                throw new Exception("Send message failed");
            }
        }

        public void ReceiveMessageLoop()
        {
            while (socket.Connected)
            {
                try
                {
                    string? message = ReceiveMessage();
                    if (message is { })
                    {
                        lock(MessageHolder.ReceiveQueue)
                        {
                            MessageHolder.ReceiveQueue.Enqueue(message);
                        }  
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                }
            }

        }

        public string? ReceiveMessage()
        {
            string? message = null;
            byte[] buffer = new byte[1024];

            try
            {
                int count = socket.Receive(buffer);
                message = Encoding.ASCII.GetString(buffer, 0, count);
            }
            catch
            {
                throw new Exception("Receive message failed");
            }

            return message;
        }

        public void Stop()
        {
            try
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }
    }
}
