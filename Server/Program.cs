namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server(11111);
            server.Start();

            Console.ReadLine();
        }
    }
}
