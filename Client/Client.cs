using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Client
{
    public class Client
    {
        private Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public void Start()
        {
            Task.Run(() => ReceiveMessageLoop());
            Task.Run(() => SendMessageLoop());
        }

        public void Connect()
        {
            IPAddress? ip;
            int port;

            while(!_clientSocket.Connected)
            {

                do
                {
                    Console.Write("Enter server Ip: ");
                } while (!IPAddress.TryParse(Console.ReadLine(), out ip));

                do
                {
                    Console.Write("Enter server port: ");
                } while (!int.TryParse(Console.ReadLine(), out port));

                try
                {
                    IPEndPoint serverEndPoint = new IPEndPoint(ip, port);
                    _clientSocket.Connect(serverEndPoint);

                }
                catch
                {
                    Console.WriteLine("Connection failed");
                }
            }
        }

        public void SignIn()
        {
            string? name = null;
            do
            {
                Console.Write("Enter your name: ");
                name = Console.ReadLine();
            } while (string.IsNullOrEmpty(name));


            SendMessage(name);
        }

        public void SendMessageLoop()
        {
            string? message;
            while(_clientSocket.Connected)
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
                    SendMessage(message);
                }
            }
        }

        public void SendMessage(string message)
        {
            try
            {
                _clientSocket.Send(Encoding.ASCII.GetBytes(message));
            }
            catch
            {
                Console.WriteLine("Send message failed");
            }
        }

        public void ReceiveMessageLoop()
        {
            while (true)
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

        }

        public string? ReceiveMessage()
        {
            string? message = null;
            byte[] buffer = new byte[1024];

            try
            {
                int count = _clientSocket.Receive(buffer);
                message = Encoding.ASCII.GetString(buffer, 0, count);
            }
            catch
            {
                Console.WriteLine("Receive message failed");
            }

            return message;
        }

        public void Stop()
        {
            try
            {
                _clientSocket.Shutdown(SocketShutdown.Both);
                _clientSocket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }
    }
}
