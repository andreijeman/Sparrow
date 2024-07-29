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

                while (true)
                {
                    Console.Write("Enter server Ip: ");
                    if (IPAddress.TryParse(Console.ReadLine(), out ip)) break;

                }

                while(true)
                {
                    Console.Write("Enter server port: ");
                    if (int.TryParse(Console.ReadLine(), out port)) break;
                }

                try
                {
                    IPEndPoint serverEndPoint = new IPEndPoint(ip, port);
                    _clientSocket.Connect(serverEndPoint);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                }
            }
        }

        public string SignIn()
        {
            string? name = null;
            string? id = null;
            while(name is null)
            {
                Console.Write("Enter your name: ");
                name = Console.ReadLine();
            }

            do
            {
                SendMessage(name);
                id = ReceiveMessage();

            } while ( id is null);
            Console.WriteLine(id);
            return id;
        }

        public void SendMessageLoop()
        {
            string? message = null;
            bool send = false;
            while(_clientSocket.Connected)
            {
                lock(MessageHolder.SendQueue)
                {
                    if (MessageHolder.SendQueue.Count > 0)
                    {
                        message = MessageHolder.SendQueue.Dequeue();
                        send = true;
                    }
                }

                if (send)
                {
                    SendMessage(message!);
                    send = false;
                }
            }
        }

        public void SendMessage(string message)
        {
            try
            {
                _clientSocket.Send(Encoding.ASCII.GetBytes(message));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
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
            try
            {
                byte[] buffer = new byte[1024];
                int count = _clientSocket.Receive(buffer);
                message =  Encoding.ASCII.GetString(buffer, 0, count);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
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
