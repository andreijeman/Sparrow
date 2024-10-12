using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    public class ConsoleLogger : ILogger
    {
        public void LogError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"[{DateTime.Now}] ");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Info: ");

            Console.ForegroundColor= ConsoleColor.White;
            Console.WriteLine(message);
        }

        public void LogInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"[{DateTime.Now}] ");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Info: ");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
        }

        public void LogWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"[{DateTime.Now}] ");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Info: ");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
        }
    }
}
