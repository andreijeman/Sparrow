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

        public static void PrintSendedMessage(int left, int top, string text, string senderInfo)
        {
            PrintService.Image image = PrintService.GetBorderedTextImage(
                senderInfo.Length, Config.Chat.MessageBoxMaxWidth,
                Config.Chat.MessageBoxMinHeight, Config.Chat.MessageBoxMaxHeight,
                Config.Chat.BorderTemplate, Config.Chat.SendedTextBorderColor,
                text, Config.Chat.SendedTextColor); 


            var fittedText = PrintService.GetFittedText(image.Width  - 6, 1, senderInfo);
            PrintService.PrintLine(image.Width - fittedText.Width - 4, 0, " " + fittedText.Lines[0] + " ", Config.Chat.SendedTextBorderColor, image);

            image.Print(left, top);
        }

        public static void PrintReceivedMessage(int left, int top, string text, string senderInfo)
        {
            PrintService.Image image = PrintService.GetBorderedTextImage(
                senderInfo.Length, Config.Chat.MessageBoxMaxWidth,
                Config.Chat.MessageBoxMinHeight, Config.Chat.MessageBoxMaxHeight,
                Config.Chat.BorderTemplate, 
                Config.Chat.ReceivedTextBorderColor,
                text, Config.Chat.ReceivedTextColor);


            var fittedText = PrintService.GetFittedText(image.Width - 6 , 1, senderInfo);
            PrintService.PrintLine(2, 0, " " + fittedText.Lines[0] + " ", Config.Chat.ReceivedTextBorderColor, image);

            image.Print(left, top);
        }


    }
}
