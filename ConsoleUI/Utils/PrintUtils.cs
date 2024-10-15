using System.Text;

namespace ConsoleUI
{
    public static class PrintUtils
    {
        public static void PrintText(int left, int top, List<string> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                PrintString(left, top + i, lines[i]);
            }
        }

        public static void PrintBorder(int left, int top, int width, int height, char[] template)
        {
            //Up Line
            PrintRect(left + 1, top, width - 2, 1, template[0]);

            //Left Line
            PrintRect(left + width - 1, top + 1, 1, height - 2, template[1]);

            //Down Line
            PrintRect(left + 1, top + height - 1, width - 2, 1, template[0]);

            //Righ Line
            PrintRect(left, top + 1, 1, height - 2, template[1]);

            //Left Up Corner
            PrintPoint(left, top, template[2]);

            //Right Up Corner
            PrintPoint(left + width - 1, top, template[3]);

            //Right Down Corner
            PrintPoint(left + width - 1, top + height - 1, template[4]);

            //Left Down Corner
            PrintPoint(left, top + height - 1, template[5]);
        }

        public static void PrintString(int left, int top, string str)
        {
            int length = left + str.Length > Console.BufferWidth ? Console.BufferWidth - left : str.Length;

            if(length > 0)
            {
                Console.SetCursorPosition(left, top);

                for (int i = 0; i < length; i++)
                {
                    Console.Write(str[i]);
                }
            }
        }

        public static void PrintRect(int left, int top, int width, int height, char texture)
        {
            width = left + width > Console.BufferWidth ? Console.BufferWidth - left : width;

            if(width > 0)
            {
                for(int i = 0; i < height; i++)
                {
                    Console.SetCursorPosition(left, top + i);
                    for(int j = 0; j < width; j++)
                    {
                        Console.Write(texture);
                    }
                }
            }
        }

        public static void PrintPoint(int left, int top, char texture)
        {
            if(left < Console.BufferWidth)
            {
                Console.SetCursorPosition(left, top);
                Console.Write(texture);
            }
        }
    }
}
