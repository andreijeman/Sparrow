using System;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Chat.PrintSendedMessage(12, 0, "Test\nsfadfddsfdsdsfdsfdsfdsfdsfdsfdsfsdfsdfsdfdsfsagfad\nedfsafxdfdf", "123456789");
            Chat.PrintReceivedMessage(12, 10, "Testd", "123456789");

            Console.ReadKey();
        }
    }
}
