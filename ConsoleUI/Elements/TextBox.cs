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

        public char[]? BorderTemplate { get; set; }
        public ConsoleColor BorderColor { get; set; }
        public ConsoleColor TextColor { get; set; }

        public override bool Active
        {
            get => _active;
            set
            {
                if (value != _active)
                {
                    _controller.Active = value;
                    _active = value;
                    if (value) CharInput.CharEvent += CharEvent;
                    else CharInput.CharEvent -= CharEvent;
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

            _controller.AddKeyEvent(ConsoleKey.Enter, EnterKeyEvent);
            _controller.AddKeyEvent(ConsoleKey.Backspace, BackspaceKeyEvent);
        }

        public override void Render()
        {
            if (BorderTemplate != null)
            {
                Console.ForegroundColor = BorderColor;
                PrintUtils.PrintBorder(Left, Top, Width, Height, BorderTemplate);
            }
        }

        private void EnterKeyEvent()
        {
            _index = 0;
            Action?.Invoke();
        }

        private void BackspaceKeyEvent()
        {
            if (_index > 0)
            {
                _index--;
                if(_cursorLeft <= _bufferLeft)
                {
                    _cursorLeft = _bufferLeft + _bufferWidth;
                    _cursorTop--;
                }

                PrintUtils.PrintPoint(--_cursorLeft, _cursorTop, ' ');
                Console.SetCursorPosition(_cursorLeft, _cursorTop);

            }
        }

        private void CharEvent(char ch)
        {
            if (_index < _bufferSize)
            {
                _buffer[_index++] = ch;
                if(_cursorLeft >= _bufferLeft + _bufferWidth)
                {
                    _cursorLeft = _bufferLeft;
                    _cursorTop++;

                }

                Console.ForegroundColor = TextColor;
                PrintUtils.PrintPoint(_cursorLeft++, _cursorTop, ch);
            }
        }
    }
}
