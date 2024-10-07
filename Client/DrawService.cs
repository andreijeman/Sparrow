using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public static class DrawService
    {
        public static (int BoxWidth, int BoxHeight) PrintTextInBox(int left, int top, int boxMinWidth, int boxMaxWidth, int boxMinHeight, int boxMaxHeight, char[] boxTemplate, ConsoleColor boxColor, string text, ConsoleColor textColor)
        {
            var size = PrintTextInRange(left + 2, top + 1, boxMaxWidth - 3, boxMaxHeight - 2, text, textColor);

            int boxWidth = size.TextWidth + 4 > boxMinWidth ? size.TextWidth + 4 : boxMinWidth;
            int boxHeight = size.TextHeight + 2 > boxMinHeight ? size.TextHeight + 2 : boxMinHeight;

            PrintBox(left, top, boxWidth, boxHeight, boxTemplate, boxColor);

            return (boxWidth, boxHeight);
        }

        public static (int TextWidth, int TextHeight) PrintTextInRange(int left, int top, int width, int height, string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(left, top);

            int maxLineLength = 0;
            int currentLine = 0;
            int index = 0;
            int currentLineLength = 0;

            while(index < text.Length && currentLine < height)
            {             
                if(currentLineLength < width && text[index] != '\n')
                {
                    Console.Write(text[index]);
                    index++;
                    currentLineLength++;
                }
                else
                {
                    if (maxLineLength < currentLineLength) maxLineLength = currentLineLength;

                    if (text[index] == '\n') index++;

                    currentLine++;
                    currentLineLength = 0;
                    Console.SetCursorPosition(left, top + currentLine);
                }
            }

            if (maxLineLength < currentLineLength) maxLineLength = currentLineLength;
            return (maxLineLength, currentLine + 1);
        }

        public static void ClearRect(int left, int top, int width, int height)
        {
            for(int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(left, top + i);
                for (int j = 0; j < width; j++)
                {
                    Console.Write(" ");
                }
            }
        }

        public static void PrintBox(int left, int top, int width, int height, char[] template, ConsoleColor color)
        {
            Console.ForegroundColor = color;

            //Up Line
            PrintHorizontalLine(left + 1, top, width - 2, template[0]);

            //Left Line
            PrintVerticalLine(left + width - 1, top + 1, height - 2, template[1]);

            //Down Line
            PrintHorizontalLine(left + 1, top + height - 1, width - 2, template[0]);

            //Righ Line
            PrintVerticalLine(left, top + 1, height - 2, template[1]);


            //Left Up Corner
            PrintPoint(left, top, template[2]);

            //Right Up Corner
            PrintPoint(left + width - 1, top, template[3]);

            //Right Down Corner
            PrintPoint(left + width - 1, top + height - 1, template[4]);

            //Left Down Corner
            PrintPoint(left, top + height - 1, template[5]);

        }

        public static void PrintPoint(int left, int top, char character)
        {
            if(IsInConsoleBufferRange(left, top))
            {
                Console.SetCursorPosition(left, top);
                Console.Write(character);
            }
        }

        public static void PrintVerticalLine(int left, int top, int lenght, char character)
        {
            for (int i = 0; i < lenght; i++)
            {
                Console.SetCursorPosition(left, top + i);
                Console.Write(character);
            }
        }

        public static void PrintHorizontalLine(int left, int top, int lenght, char character)
        {
            Console.SetCursorPosition(left, top);
            for (int i = 0; i < lenght; i++)
            {
                Console.Write(character);
            } 
        }

        public static bool IsInConsoleBufferRange(int left, int top)
        {
            return left >= 0 && left < Console.BufferWidth &&
                    top >= 0 && top < Console.BufferHeight;
        }

        public static ConsoleColor GenerateColorFromText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return ConsoleColor.White;
            }

            int hash = text.GetHashCode();
            return (ConsoleColor)(Math.Abs(hash) % 16);
        }
    }
}
