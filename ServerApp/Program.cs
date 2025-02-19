using Logger;
using Server;

ChatServer server = new ChatServer(port: 7777, logger: new ConsoleLogger());

Console.WriteLine($"Server: {server.IP} / {server.Port}");

server.Start();

Console.ReadKey();
server.Stop();

