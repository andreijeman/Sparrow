using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public class TextBox : Element
    {
        private char[] _buffer;
        private int _bufferSize;
        private int _bufferLeft, _bufferTop;
        private int _bufferWidth, _bufferHeight;
        private int _index;
        private int _cursorLeft, _cursorTop;

        public char Cursor { get; set; } 
        public char[]? BorderTemplate { get; set; }
        public ConsoleColor BorderColor { get; set; }
        public ConsoleColor TextColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }

        public override bool Active
        {
            get => _active;
            set
            {
                if (value != _active)
                {
                    _controller.Active = value;
                    _active = value;
                    if (value)
                    {
                        PrintUtils.PrintChar(_cursorLeft, _cursorTop, Cursor, TextColor, BackgroundColor);
                        KeyInput.KeyEvent += ProcessKey;

                        Task.Run(async () =>
                        {
                            while (_active)
                            {
                                PrintUtils.PrintChar(_cursorLeft, _cursorTop, Cursor, TextColor, BackgroundColor);
                                await Task.Delay(400);
                                PrintUtils.PrintChar(_cursorLeft, _cursorTop, ' ', TextColor, BackgroundColor);
                                await Task.Delay(400);
                            }

                        });
                    }
                    else
                    {
                        PrintUtils.PrintChar(_cursorLeft, _cursorTop, ' ', TextColor, BackgroundColor);
                        KeyInput.KeyEvent -= ProcessKey;
                    }
                }
            }
        }

        public ActionEventHandler? Action;

        public TextBox(int left, int top, int width, int height) : base(left, top, width, height)
        {
            _index = 0;
            _bufferWidth = width - 4;
            _bufferHeight = height - 2;
            _bufferSize = _bufferWidth * _bufferHeight;
            _buffer = new char[_bufferSize];
            _bufferLeft = left + 2;
            _bufferTop = top + 1;
            _cursorLeft = _bufferLeft;
            _cursorTop = _bufferTop;
            Cursor = Assets.Cursor;
            TextColor = ConsoleColor.White;
            BackgroundColor = ConsoleColor.Black;
            BorderColor = ConsoleColor.Magenta;
            BorderTemplate = Assets.Border1;


            _controller.AddKeyEvent(ConsoleKey.Enter, EnterKeyEvent);
            _controller.AddKeyEvent(ConsoleKey.Backspace, BackspaceKeyEvent);
        }

        public override void Render()
        {
            if (BorderTemplate != null) PrintUtils.PrintBorder(Left, Top, Width, Height, BorderTemplate, BorderColor, BackgroundColor);
        }

        private void EnterKeyEvent()
        {
            _index = 0;
            
            PrintUtils.PrintRect(_bufferLeft, _bufferTop, _bufferWidth, _bufferHeight, ' ', TextColor, BackgroundColor);
            PrintUtils.PrintChar(_cursorLeft, _cursorTop, ' ', TextColor, BackgroundColor);
            
            _cursorLeft = _bufferLeft;
            _cursorTop = _bufferTop;
            
            PrintUtils.PrintChar(_cursorLeft, _cursorTop, Cursor, TextColor, BackgroundColor);
            Action?.Invoke();
        }

        private void BackspaceKeyEvent()
        {
            if (_index > 0)
            {
                _index--;

                PrintUtils.PrintChar(_cursorLeft--, _cursorTop, ' ', TextColor, BackgroundColor);

                if (_cursorLeft == _bufferLeft && _cursorTop > _bufferTop)
                {
                    PrintUtils.PrintChar(_cursorLeft, _cursorTop--, ' ', TextColor, BackgroundColor);
                    _cursorLeft = _bufferLeft + _bufferWidth;
                }

                PrintUtils.PrintChar(_cursorLeft, _cursorTop, Cursor, TextColor, BackgroundColor);

            }
        }

        private void ProcessKey(ConsoleKeyInfo keyInfo)
        {
            char ch = keyInfo.KeyChar;
            if (!char.IsControl(ch) && ch != '\0' && _index < _bufferSize)
            {
                if(_cursorLeft == _bufferLeft + _bufferWidth)
                {
                    PrintUtils.PrintChar(_cursorLeft, _cursorTop, ' ', TextColor, BackgroundColor);
                    _cursorTop++;
                    _cursorLeft = _bufferLeft;
                }

                _buffer[_index++] = ch;

                Console.ForegroundColor = TextColor;
                Console.BackgroundColor = BackgroundColor;

                PrintUtils.PrintChar(_cursorLeft++, _cursorTop, ch, TextColor, BackgroundColor);
                PrintUtils.PrintChar(_cursorLeft, _cursorTop, Cursor, TextColor, BackgroundColor);

            }
        }
    }
}
