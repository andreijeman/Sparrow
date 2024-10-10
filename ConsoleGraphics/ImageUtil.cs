using System.Text;

namespace ConsoleGraphics
{
    public static class ImageUtil
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

        public static ConsoleColor GetColorFromText(string text)
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
