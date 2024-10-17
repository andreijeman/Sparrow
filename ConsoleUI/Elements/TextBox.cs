using ConsoleUI.Utils;
using ConsoleUI.Inputs;
using System.Diagnostics;

namespace ConsoleUI.Elements
{
    public class TextBox : BaseElement
    {
        private char[] _buffer;
        private int _bufferSize;
        private int _bufferLeft, _bufferTop;
        private int _bufferWidth, _bufferHeight;
        private int _index;
        private int _cursorLeft, _cursorTop;

        private Stopwatch _watch;

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
                _controller.Active = value;
                _active = value;
                if (value)
                {
                    KeyInput.KeyEvent += ProcessKey;

                    Task.Run(async () =>
                    {
                        while (_active)
                        {
                            PrintUtils.PrintChar(OriginLeft + _cursorLeft, OriginTop + _cursorTop, Cursor, TextColor, BackgroundColor);
                            await Task.Delay(400);
                            PrintUtils.PrintChar(OriginLeft + _cursorLeft, OriginTop + _cursorTop, ' ', TextColor, BackgroundColor);
                            await Task.Delay(400);
                        }

                    });
                }
                else
                {
                    PrintUtils.PrintChar(OriginLeft + _cursorLeft, OriginTop + _cursorTop, ' ', TextColor, BackgroundColor);
                    KeyInput.KeyEvent -= ProcessKey;
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

            _watch = new Stopwatch();
        }

        public override void Draw()
        {
            if (BorderTemplate != null) PrintUtils.PrintBorder(OriginLeft + Left, OriginTop + Top, Width, Height, BorderTemplate, BorderColor, BackgroundColor);
        }

        private void EnterKeyEvent()
        {
            _watch.Stop();
            if (_watch.Elapsed.TotalMilliseconds > 200)
            {

                _index = 0;
            
                PrintUtils.PrintRect(OriginLeft + _bufferLeft, OriginTop + _bufferTop, _bufferWidth, _bufferHeight, ' ', TextColor, BackgroundColor);
                PrintUtils.PrintChar(OriginLeft + _cursorLeft, OriginTop + _cursorTop, ' ', TextColor, BackgroundColor);
            
                _cursorLeft = _bufferLeft;
                _cursorTop = _bufferTop;
            
                PrintUtils.PrintChar(OriginLeft + _cursorLeft, OriginTop + _cursorTop, Cursor, TextColor, BackgroundColor);
                Action?.Invoke();

                _watch.Restart();
            }
            _watch.Start();
        }

        private void BackspaceKeyEvent()
        {
            if (_index > 0)
            {
                _index--;

                PrintUtils.PrintChar(OriginLeft + _cursorLeft--, OriginTop + _cursorTop, ' ', TextColor, BackgroundColor);

                if (_cursorLeft == _bufferLeft && _cursorTop > _bufferTop)
                {
                    PrintUtils.PrintChar(OriginLeft + _cursorLeft, OriginTop + _cursorTop--, ' ', TextColor, BackgroundColor);
                    _cursorLeft = _bufferLeft + _bufferWidth;
                }

                PrintUtils.PrintChar(OriginLeft + _cursorLeft, OriginTop + _cursorTop, Cursor, TextColor, BackgroundColor);

            }
        }

        private void ProcessKey(ConsoleKeyInfo keyInfo)
        {
            char ch = keyInfo.KeyChar;
            if (!char.IsControl(ch) && ch != '\0' && _index < _bufferSize)
            {
                if(_cursorLeft == _bufferLeft + _bufferWidth)
                {
                    PrintUtils.PrintChar(OriginLeft + _cursorLeft, OriginTop + _cursorTop, ' ', TextColor, BackgroundColor);
                    _cursorTop++;
                    _cursorLeft = _bufferLeft;
                }

                _buffer[_index++] = ch;

                Console.ForegroundColor = TextColor;
                Console.BackgroundColor = BackgroundColor;

                PrintUtils.PrintChar(OriginLeft + _cursorLeft++, OriginTop + _cursorTop, ch, TextColor, BackgroundColor);
                PrintUtils.PrintChar(OriginLeft + _cursorLeft, OriginTop + _cursorTop, Cursor, TextColor, BackgroundColor);

            }
        }
    }
}
