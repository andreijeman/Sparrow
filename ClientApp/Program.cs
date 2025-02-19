using Logger;
using Client;
using System.Net;
using Server.Postman;
using System.Text;

public class Program
{
    static object _consoleLocker = new object();
    static StringBuilder _input = new StringBuilder();

    static MyConsoleLogger _logger = new MyConsoleLogger(ClearInput, WriteInput, _consoleLocker);
    static ChatClient _client = new ChatClient(_logger);

    static async Task Main(string[] args)
    {
        _client.PacketReceivedEvent += ProcessReceivedMessage;

        bool isConnected = false;
        do
        {
            ReadServerData(out var ip, out var port, out var password, out var username);
            isConnected = await _client.ConnectToServerAsync(ip, port, password, username);
        } while (!isConnected);


        _client.StartReceivePackets();

        WriteInput();
        ProcessInput();
    }

    static void ReadServerData(out IPAddress ip, out int port, out string password, out string username)
    {
        while (true)
        {
            Console.Write("IP: ");
            if (IPAddress.TryParse(Console.ReadLine(), out var res))
            {
                ip = res;
                break;
            }
        }

        while (true)
        {
            Console.Write("Port: ");
            if (int.TryParse(Console.ReadLine(), out var res))
            {
                port = res;
                break;
            }
        }

        while (true)
        {
            Console.Write("Password: ");
            string? res = Console.ReadLine();
            if (res != null)
            {
                password = res;
                break;
            }
        }

        while (true)
        {
            Console.Write("Username: ");
            string? res = Console.ReadLine();
            if (res != null)
            {
                username = res;
                break;
            }
        }
    }

    static void ProcessReceivedMessage(Packet packet)
    {
        switch(packet.Label)
        {
            case Label.UserMessage:
                ClearInput();
                WriteUserMessage(packet);
                WriteInput();
                break;

            case Label.UserConnected:
                _logger.LogInfo($"New user connected - {packet.Sender}");
                break;

            case Label.UserDiconnected:
                _logger.LogInfo($"User disconnected - {packet.Sender}");
                break;
        }
    }

    static void WriteUserMessage(Packet packet)
    {
        lock(_consoleLocker)
        {
            Console.ForegroundColor = GetColorFromString(packet.Sender);
            Console.Write(packet.Sender + ": ");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(packet.Data);
        }
    }

    static ConsoleColor GetColorFromString(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return ConsoleColor.White;
        }

        int hash = text.GetHashCode();
        return (ConsoleColor)(Math.Abs(hash) % 16);
    }

    static async void ProcessInput()
    {
        while(true)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            if(keyInfo.Key == ConsoleKey.Enter)
            {
                await _client.SendPacketAsync(
                    new Packet
                    {
                        Label = Label.UserMessage,
                        Data = _input.ToString()
                    });

                _input.Clear();
                ClearInput();
            }
            else if(keyInfo.Key == ConsoleKey.Backspace)
            {
                if(_input.Length > 0)
                {
                    ClearInput();
                    _input.Remove(_input.Length - 1, 1);
                }
            }
            else
            {
                _input.Append(keyInfo.KeyChar);
            }
                
            WriteInput();
        }
    }

    static void WriteInput()
    {
        lock(_consoleLocker)
        {
            int top = Console.GetCursorPosition().Top;

            Console.Write("> ");
            Console.Write(_input.ToString()); 

            Console.SetCursorPosition(0, top);
        }
    }

    static void ClearInput()
    {
        lock(_consoleLocker)
        {
            int top = Console.GetCursorPosition().Top;
            
            Console.Write(new string(' ', _input.Length + 2));
            
            Console.SetCursorPosition(0, top);
        }
    }
}

public class MyConsoleLogger : ILogger
{
    private ConsoleLogger _logger = new ConsoleLogger();
    private object _locker;

    public Action ClearInput;
    public Action WriteInput;

    public MyConsoleLogger(Action clearInput, Action writeInput, object locker)
    {
        ClearInput = clearInput;
        WriteInput = writeInput;
        _locker = locker;
    }

    public void LogError(string message)
    {
        ClearInput.Invoke();
        lock(_locker)
        {
            _logger.LogError(message);
        }
        WriteInput.Invoke();
    }

    public void LogInfo(string message)
    {
        ClearInput.Invoke();
        lock (_locker)
        {
            _logger.LogInfo(message);
        }
        WriteInput.Invoke();
    }

    public void LogWarning(string message)
    {
        ClearInput.Invoke();
        lock ( _locker)
        {
            _logger.LogWarning(message);
        }
        WriteInput.Invoke();
    }
}
