namespace ConsoleUI.Utils
{
    public static class PrintUtils
    {
        static PrintUtils()
        {
            Console.CursorVisible = false;
        }

        public static void PrintText(int left, int top, List<string> lines, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                PrintString(left, top + i, lines[i], foregroundColor, backgroundColor);
            }
        }

        public static void PrintBorder(int left, int top, int width, int height, char[] template, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            //Left Line
            PrintRect(left + width - 1, top + 1, 1, height - 2, template[0], foregroundColor, backgroundColor);

            //Left Up Corner
            PrintChar(left, top, template[1], foregroundColor, backgroundColor);

            //Up Line
            PrintRect(left + 1, top, width - 2, 1, template[2], foregroundColor, backgroundColor);

            //Right Up Corner
            PrintChar(left + width - 1, top, template[3], foregroundColor, backgroundColor);

            //Left Down Corner
            PrintChar(left, top + height - 1, template[4], foregroundColor, backgroundColor);

            //Down Line
            PrintRect(left + 1, top + height - 1, width - 2, 1, template[5], foregroundColor, backgroundColor);

            //Right Down Corner
            PrintChar(left + width - 1, top + height - 1, template[6], foregroundColor, backgroundColor);

            //Right Line
            PrintRect(left, top + 1, 1, height - 2, template[7], foregroundColor, backgroundColor);
        }

        public static void PrintString(int left, int top, string str, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            int length = left + str.Length > Console.BufferWidth ? Console.BufferWidth - left : str.Length;

            if(length > 0)
            {
                for (int i = 0; i < length; i++)
                {
                    PrintChar(left + i, top, str[i], foregroundColor, backgroundColor);
                }
            }
        }

        public static void PrintRect(int left, int top, int width, int height, char texture, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {

            for(int i = 0; i < height; i++)
            {
                for(int j = 0; j < width; j++)
                {
                    PrintChar(left + j, top + i, texture, foregroundColor, backgroundColor);
                }
            }
        }

        public static void PrintChar(int left, int top, char texture, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            if(left < Console.BufferWidth)
            {
                if(Console.ForegroundColor != foregroundColor) Console.ForegroundColor = foregroundColor;
                if(Console.BackgroundColor != backgroundColor) Console.BackgroundColor = backgroundColor;

                Console.SetCursorPosition(left, top);
                Console.Write(texture);
            }
        }
    }
}
