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
        int curreLine;
       
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

        public static void PrintMessageEvent(string text, string senderInfo)
        {

        }

        public static void PrintSendedMessage(int left, int top, string text, string senderInfo)
        {
            Image image = ImageUtil.GetBorderedTextImage(
                senderInfo.Length, Config.Chat.MessageBoxMaxWidth,
                Config.Chat.MessageBoxMinHeight, Config.Chat.MessageBoxMaxHeight,
                Config.Chat.BorderTemplate, Config.Chat.SendedTextBorderColor,
                text, Config.Chat.SendedTextColor); 


            var fittedText = ImageUtil.GetFittedText(image.Width  - 6, 1, senderInfo);
            ImageUtil.PrintLine(image.Width - fittedText.Width - 4, 0, " " + fittedText.Lines[0] + " ", Config.Chat.SendedTextBorderColor, image);

            image.Print(left, top);
        }

        public static void PrintReceivedMessage(int left, int top, string text, string senderInfo)
        {
            Image image = ImageUtil.GetBorderedTextImage(
                senderInfo.Length, Config.Chat.MessageBoxMaxWidth,
                Config.Chat.MessageBoxMinHeight, Config.Chat.MessageBoxMaxHeight,
                Config.Chat.BorderTemplate, 
                Config.Chat.ReceivedTextBorderColor,
                text, Config.Chat.ReceivedTextColor);


            var fittedText = ImageUtil.GetFittedText(image.Width - 6 , 1, senderInfo);
            ImageUtil.PrintLine(2, 0, " " + fittedText.Lines[0] + " ", Config.Chat.ReceivedTextBorderColor, image);

            image.Print(left, top);
        }


    }
}
