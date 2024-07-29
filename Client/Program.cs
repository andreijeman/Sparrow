using System;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(ChatConfig.Width, ChatConfig.Height);
            Console.SetBufferSize(ChatConfig.Width, Console.BufferHeight);

            Client client = new Client();
            client.Connect();
            Chat chat = new Chat(client.SignIn());
            client.Start();
            chat.Start();
        }
    }
}
