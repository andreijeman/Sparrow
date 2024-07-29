using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Client
{
    public class Chat
    {
        private int _currentLine;
        private string _userId { get; init; }
        private int _inputLine = ChatConfig.Height - ChatConfig.InputMaxHeight;
        private int _inputHeight;
        private StringBuilder _message = new StringBuilder();
        private bool _printInput = false;
        public Chat(string userId)
        {
            _userId = userId;
        }

        public void ReadInput()
        {
            if (Console.KeyAvailable)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if(keyInfo.Key == ConsoleKey.Enter)
                {
                    MessageHolder.SendQueue.Enqueue(_message.ToString());
                    _message.Clear();
                    _printInput = true;
                }
                else if(keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (_message.Length > 0)
                    {
                        _message.Length--;
                        _printInput = true;
                    }
                }
                else
                {
                    if (_message.Length / ChatConfig.InputWidth < ChatConfig.InputMaxHeight)
                    {
                        _message.Append(keyInfo.KeyChar);
                        _printInput = true;
                    }
                }

            }

        }

        public void PrintInput()
        {
            _printInput = false;
            ClearInput();

            _inputHeight = _message.Length > ChatConfig.InputWidth ? 
                _inputHeight = (int)Math.Ceiling(_message.Length / (double)ChatConfig.InputWidth) : 1;

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            PrintTextBox(ChatConfig.LineLeftBorder, _inputLine - _inputHeight, 
                ChatConfig.InputWidth + ChatConfig.BorderThickness * 2 + ChatConfig.MessageConfig.TextLeftPadding * 2, _inputHeight + ChatConfig.BorderThickness * 2);

            Console.ForegroundColor = ConsoleColor.Gray;
            PrintText(_message.ToString(), ChatConfig.InputWidth, ChatConfig.LineLeftBorder + 2, _inputLine - _inputHeight + 1);
        }

        private void ClearInput()
        {
            for (int i = -1; i <= _inputHeight; i++)
            {
                Console.SetCursorPosition(ChatConfig.LineLeftBorder, _inputLine - i);
                for (int j = 0; j < ChatConfig.LineRightBorder - ChatConfig.LineLeftBorder; j++)
                {
                    Console.Write(" ");
                }
            }
        }

        private void PrintMessage(string id, string userName, string text)
        {
            int boxHeight = (int)Math.Ceiling(text.Length / (double)ChatConfig.MessageConfig.TextWidth) + 2;
            if (_inputLine - _currentLine < ChatConfig.InputMaxHeight + ChatConfig.BorderThickness*2)
            {
                ClearInput();
                _inputLine += boxHeight;
            }

            if (id != _userId)
            {
                int left = ChatConfig.LineLeftBorder + ChatConfig.MessageConfig.TextBoxPadding;

                Console.ForegroundColor = GetColorFromUserId(int.Parse(id));
                PrintTextBox(left, _currentLine, 
                    (text.Length < ChatConfig.MessageConfig.TextWidth ? text.Length : ChatConfig.MessageConfig.TextWidth) + 2 + ChatConfig.MessageConfig.TextLeftPadding * 2, 
                    boxHeight);

                Console.SetCursorPosition(left + ChatConfig.MessageConfig.UserNamePadding, _currentLine);
                Console.Write(" " + userName + " ");

                Console.ForegroundColor = ConsoleColor.Gray;
                PrintText(text, ChatConfig.MessageConfig.TextWidth, left + ChatConfig.MessageConfig.TextLeftPadding + 1, _currentLine + 1);
            }
            else
            {
                int boxWidth = (text.Length < ChatConfig.MessageConfig.TextWidth ? text.Length : ChatConfig.MessageConfig.TextWidth) + 2 + ChatConfig.MessageConfig.TextLeftPadding * 2;
                int left = ChatConfig.LineRightBorder - ChatConfig.MessageConfig.TextBoxPadding - boxWidth;

                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                PrintTextBox(left, _currentLine, boxWidth, boxHeight);

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.SetCursorPosition(ChatConfig.LineRightBorder - ChatConfig.MessageConfig.TextBoxPadding - ChatConfig.MessageConfig.UserNamePadding - userName.Length - 2, _currentLine);
                Console.Write(" " + userName + " ");

                Console.ForegroundColor = ConsoleColor.Gray;
                PrintText(text, ChatConfig.MessageConfig.TextWidth, left + ChatConfig.MessageConfig.TextLeftPadding + 1, _currentLine + 1);
            }
            _currentLine += boxHeight;
        }

        public static ConsoleColor GetColorFromUserId(int userId)
        {
            ConsoleColor[] Colors = (ConsoleColor[])Enum.GetValues(typeof(ConsoleColor));
            
            int hashCode = userId.GetHashCode();

            int colorIndex = Math.Abs(hashCode) % Colors.Length;

            return Colors[colorIndex];
        }

        private void PrintText(string text, int textWidth, int left, int top)
        {

            int index = 0, line = 0;
            while (index + textWidth <= text.Length)
            {
                Console.SetCursorPosition(left, top + line);
                
                Console.Write(text.Substring(index, textWidth));
                index += textWidth;
                line++;
            }
            
            Console.SetCursorPosition(left, top + line);
            Console.WriteLine(text.Substring(index));
        }

        private void PrintTextBox(int left, int top, int width, int height)
        {
            Console.SetCursorPosition(left, top);
            Console.Write(ChatConfig.TextBoxConfig.CornerLeftUp);
            for (int i = 0; i < width - 2; i++) Console.Write(ChatConfig.TextBoxConfig.HorizontalLine);
            Console.Write(ChatConfig.TextBoxConfig.CornerRightUp);

            for (int i = 1; i < height - 1; i++)
            {
                Console.SetCursorPosition(left, top + i);
                Console.Write(ChatConfig.TextBoxConfig.VerticalLine);
                Console.SetCursorPosition(left + width - 1, top + i);
                Console.Write(ChatConfig.TextBoxConfig.VerticalLine);
            }

            Console.SetCursorPosition(left, top + height - 1);
            Console.Write(ChatConfig.TextBoxConfig.CornerLeftDown);
            for (int i = 0; i < width - 2; i++) Console.Write(ChatConfig.TextBoxConfig.HorizontalLine);    
            Console.Write(ChatConfig.TextBoxConfig.CornerRightDown);
        }

        public void Start()
        {
            Console.Clear();
            PrintInput();

            while (true)
            {
                string? receivedMessage = null;
                lock(MessageHolder.ReceiveQueue)
                {
                    if(MessageHolder.ReceiveQueue.Count > 0)
                    {
                        receivedMessage = MessageHolder.ReceiveQueue.Dequeue();

                    }
                }

                if(receivedMessage is { })
                {
                    string[] message = receivedMessage.Split('?', 3);
                    PrintMessage(message[0], message[1], message[2]);
                    _printInput = true;

                    receivedMessage = null; 
                }
                ReadInput();
                if(_printInput) PrintInput();
            }
        }
    }

    public static class ChatConfig
    {
     
        public const int Width =50;
        public const int Height = 40;

        public const int LineLeftBorder = 1;
        public const int LineRightBorder = 49;
        
        public const int InputWidth = LineRightBorder - LineLeftBorder - 2*BorderThickness - 2*MessageConfig.TextLeftPadding;
        public const int InputMaxHeight = 3;
        
        public const int BorderThickness = 1;



        public static class TextBoxConfig
        {
            public const char CornerLeftUp = '┌';
            public const char CornerLeftDown = '└';
            public const char CornerRightUp = '┐';
            public const char CornerRightDown = '┘';
            public const char VerticalLine = '│';
            public const char HorizontalLine = '─';
        }

        public static class MessageConfig
        {
            public const int TextBoxPadding = 1;
            public const int UserNamePadding = 2;
            public const int TextWidth = 30;
            public const int TextLeftPadding = 1;
        }

    }
}
        
