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

        public static void PrintMyMessage(int left, int top, string text, string senderInfo)
        {
            ConsoleColor boxColor = DrawService.GenerateColorFromText(senderInfo);
            
            var boxSize = DrawService.PrintTextInBox(
                            left, top, 
                            Config.Chat.MessageBoxMinWidth, Config.Chat.MessageBoxMaxWidth,
                            Config.Chat.MessageBoxMinHeight, Config.Chat.MessageBoxMaxHeight,
                            Config.Chat.BoxTemplate, boxColor, 
                            text, Config.Chat.TextColor);

            int margin = 4;
            int diff = (boxSize.BoxWidth - senderInfo.Length) / 2;
            
            var textSize = DrawService.PrintTextInRange(left + margin, top, boxSize.BoxWidth - 2 * margin, 1, senderInfo, boxColor);

            DrawService.PrintPoint(left + margin - 1, top, ' ');
            DrawService.PrintPoint(left + margin + textSize.TextWidth, top, ' ');
        }


    }
}
