using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public static class PrintService
    {
        public static Image GetBorderedTextImage(int borderMinWidth, int borderMaxWidth, 
                                                int borderMinHeight, int borderMaxHeight, 
                                                char[] borderTemplate, ConsoleColor borderColor, 
                                                string text, ConsoleColor textColor)
        {
            var fittedText = GetFittedText(borderMaxWidth, borderMaxHeight, text);

            int imageWidth = fittedText.Width + 4 > borderMinWidth ? fittedText.Width + 4 : borderMinWidth;
            int imageHeight = fittedText.Height + 2 > borderMinHeight ? fittedText.Height + 2 : borderMinHeight;
            
            Image image = new Image(imageWidth, imageHeight);
            PrintBorder(0, 0, imageWidth, imageHeight, borderTemplate, borderColor, image);
            PrintLines(2, 1, fittedText.Lines, textColor, image);

            return image;
        }

        public static Image GetTextImage(int maxWidth, int maxHeight, string text, ConsoleColor color)
        {
            var fittedText = GetFittedText(maxWidth, maxHeight, text);

            Image image = new Image(fittedText.Width, fittedText.Height);

            PrintLines(0, 0, fittedText.Lines, color, image);

            return image;
        }

        public static (int Width, int Height, List<string> Lines) GetFittedText(int maxWidth, int maxHeight, string text)
        {
            List<string> lines = new List<string>();

            int index = 0;
            int maxLineLength = 0;

            while (lines.Count < maxHeight && index < text.Length)
            {
                StringBuilder line = new StringBuilder("");
                for (int i = 0; i < maxWidth && index < text.Length; i++)
                {
                    if (text[index] == '\n')
                    {
                        index++;
                        break;
                    }
                    else
                    {
                        line.Append(text[index]);
                        index++;
                    }
                }

                if (maxLineLength < line.Length) maxLineLength = line.Length;
                lines.Add(line.ToString());
            }

            return (maxLineLength, lines.Count, lines);
        }

        public static void PrintBorder(int left, int top, int width, int height, char[] template, ConsoleColor color, Image image)
        {
            //Up Line
            PrintHorizontalLine(left + 1, top, width - 2, template[0], color, image);

            //Left Line
            PrintVerticalLine(left + width - 1, top + 1, height - 2, template[1], color, image);

            //Down Line
            PrintHorizontalLine(left + 1, top + height - 1, width - 2, template[0], color, image);

            //Righ Line
            PrintVerticalLine(left, top + 1, height - 2, template[1], color, image);

            //Left Up Corner
            image.SetCell(left, top, template[2], color);

            //Right Up Corner
            image.SetCell(left + width - 1, top, template[3], color);

            //Right Down Corner
            image.SetCell(left + width - 1, top + height - 1, template[4], color);

            //Left Down Corner
            image.SetCell(left, top + height - 1, template[5], color);
        }

        public static void PrintLines(int left, int top, List<string> lines, ConsoleColor color, Image image)
        {
            for(int i = 0; i < lines.Count; i++) 
            {
                PrintLine(left, top + i, lines[i], color, image);
            }
        }

        public static void PrintLine(int left, int top, string line, ConsoleColor color, Image image)
        {
            for (int i = 0; i < line.Length; i++)
            {
                image.SetCell(left + i, top, line[i], color);
            }
        }

        public static void PrintVerticalLine(int left, int top, int length, char texture, ConsoleColor color, Image image)
        {
            for (int i = 0; i < length; i++)
            {
                image.SetCell(left, top + i, texture, color);
            }
        }

        public static void PrintHorizontalLine(int left, int top, int lenght, char texture, ConsoleColor color, Image image)
        {
            for (int i = 0; i < lenght; i++)
            {
                image.SetCell(left + i, top, texture, color);
            } 
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

        public class Image
        {
            public int Width { get; }
            public int Height { get; }

            public Cell[,] Grid { get; }

            public void Print(int left, int top)
            {
                if (left >= 0 && left + Width < Console.BufferWidth &&
                    top >= 0 && top + Height < Console.BufferHeight)
                {
                    ConsoleColor predColor = ConsoleColor.White;
                    for (int i = 0; i < Height; i++)
                    {
                        Console.SetCursorPosition(left, top + i);
                        for (int j = 0; j < Width; j++)
                        {
                            if (Grid[i, j].Color != predColor)
                            {
                                predColor = Grid[i, j].Color;
                                Console.ForegroundColor = predColor;
                            }
                            if(Grid[i, j].Texture == '\0') Console.SetWindowSize(10, 10);
                            else Console.Write(Grid[i, j].Texture );
                        }
                    }
                }
            }

            public void SetCell(int left, int top, char texture, ConsoleColor color)
            {
                if (left >= 0 && left < Width && top >= 0 && top < Height)
                {
                    Grid[top, left].Texture = char.IsControl(texture) ? ' ' : texture;
                    Grid[top, left].Color = color;
                }
            }

            public void Clear()
            {
                for(int i = 0; i < Height; i++)
                {
                    for (int j = 0;j < Width; j++)
                    {
                        Grid[i, j].Texture = ' ';
                    }
                }
            }

            public Image(int width, int height)
            {
                Width = width;
                Height = height;
                Grid = new Cell[Height, Width];
                Clear();
            }

            public struct Cell
            { 
                public char Texture;
                public ConsoleColor Color;

                public Cell(char texture, ConsoleColor color)
                {
                    Texture = texture;
                    Color = color;
                }
            }
        }
    }
}
