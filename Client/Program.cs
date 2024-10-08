using System;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PrintService.GetBorderedTextImage(
                Config.Chat.MessageBoxMinWidth, Config.Chat.MessageBoxMaxWidth,
                Config.Chat.MessageBoxMinHeight, Config.Chat.MessageBoxMaxHeight,
                Config.Chat.BorderTemplate, Config.Chat.MyMessageBoxColor,
                "123456789qwertyuiopdddddddddddddddddddddddddddddddddddddddddddddddddddddd", Config.Chat.MyMessageTextColor
                ).Print(0, 0);


            Console.ReadKey();
        }
    }
}
