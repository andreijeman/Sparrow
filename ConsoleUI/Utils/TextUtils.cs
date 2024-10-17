using System.Text;

namespace ConsoleUI.Utils
{
    public static class TextUtils
    {
        public static List<string> GetFittedText(int maxWidth, int maxHeight, string text)
        {
            List<string> lines = new List<string>();
            StringBuilder line = new StringBuilder();
            int index = 0;

            while (lines.Count < maxHeight && index < text.Length)
            {
                line.Clear();
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

                lines.Add(line.ToString());
            }

            return lines;
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
