using ConsoleUI;


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
            Image image = ImageUtils.GetBorderedTextImage(
                senderInfo.Length, Config.Chat.MessageBoxMaxWidth,
                Config.Chat.MessageBoxMinHeight, Config.Chat.MessageBoxMaxHeight,
                Config.Chat.BorderTemplate, Config.Chat.SendedTextBorderColor,
                text, Config.Chat.SendedTextColor); 


            var fittedText = ImageUtils.GetFittedText(image.Width  - 6, 1, senderInfo);
            ImageUtils.PrintLine(image.Width - fittedText.Width - 4, 0, " " + fittedText.Lines[0] + " ", Config.Chat.SendedTextBorderColor, image);

            ImageUtils.ConsolePrint(left, top, image);
        }

        public static void PrintReceivedMessage(int left, int top, string text, string senderInfo)
        {
            Image image = ImageUtils.GetBorderedTextImage(
                senderInfo.Length, Config.Chat.MessageBoxMaxWidth,
                Config.Chat.MessageBoxMinHeight, Config.Chat.MessageBoxMaxHeight,
                Config.Chat.BorderTemplate, 
                Config.Chat.ReceivedTextBorderColor,
                text, Config.Chat.ReceivedTextColor);


            var fittedText = ImageUtils.GetFittedText(image.Width - 6 , 1, senderInfo);
            ImageUtils.PrintLine(2, 0, " " + fittedText.Lines[0] + " ", Config.Chat.ReceivedTextBorderColor, image);

            ImageUtils.ConsolePrint(left, top, image);
        }


    }
}
