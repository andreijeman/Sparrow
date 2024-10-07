using System;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Chat chat = new Chat();
            //chat.Start();
           Chat.PrintMyMessage(10, 10, "dsfd\nfhgi\nbsulfg", "1f");
           Chat.PrintMyMessage(20, 10, "dsfd\nfhgi\nbsulfg", "1f");
           Chat.PrintMyMessage(30, 10, "dsfd\nfhgi\nbsulfg", "1f");

            Console.ReadKey();
        }
    }
}
