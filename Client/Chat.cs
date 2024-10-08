using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Chat
    {
       
        public void Start()
        {
            Console.Clear();
            Console.Write(Config.Chat.Logo);


            while (true)
            {

                
            }
        }

        public void Draw()
        {

        }

/*        public static void PrintMessage(int left, int top, string text, string senderInfo, ConsoleColor textColor, ConsoleColor boxColor)
        {
            var boxSize = PrintService.PrintTextInBox(
                            left, top, 
                            Config.Chat.MessageBoxMinWidth, Config.Chat.MessageBoxMaxWidth,
                            Config.Chat.MessageBoxMinHeight, Config.Chat.MessageBoxMaxHeight,
                            Config.Chat.BoxTemplate, boxColor, 
                            text, textColor);

            int margin = 3;
            int diff = boxSize.BoxWidth - 2 * margin - senderInfo.Length;
            int dist = diff > 0 ? diff/2 + margin: margin;
            
            var textSize = PrintService.PrintTextInRange(left + dist, top, boxSize.BoxWidth - 2 * dist, 1, senderInfo, boxColor);

            PrintService.PrintPoint(left + dist - 1, top, ' ');
            PrintService.PrintPoint(left + dist + textSize.TextWidth, top, ' ');
        }*/


    }
}
