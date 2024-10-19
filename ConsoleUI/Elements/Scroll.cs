using ConsoleUI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI.Elements
{
    public class Scroll : BaseElement
    {
        private List<string> _lines;
        private int _index, _frameWidth, _frameHeight, _frameLeft, _frameTop;

        public char[] BorderTemplate { get; set; }
        public ConsoleColor BorderColor { get; set; }
        public ConsoleColor TextColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }
        public ConsoleColor HoveredColor { get; set; }

        public Scroll(int width, int height, ConsoleKey upKey, ConsoleKey downKey) : base(width, height) 
        { 
            _lines = new List<string>();
            _index = -1;
            _frameWidth = width - 4;
            _frameHeight = height - 2;
            _frameLeft = Left + 2;
            _frameTop = Top + 1;

            BorderTemplate = Assets.Border1;
            BorderColor = ConsoleColor.Magenta;
            TextColor = ConsoleColor.White;
            BackgroundColor = ConsoleColor.Black;
            HoveredColor = ConsoleColor.DarkCyan;

            _controller.AddKeyEvent(upKey, ScrollUp);
            _controller.AddKeyEvent(downKey, ScrollDown);
        }

        public override bool Active 
        {
            get => _active;
            set
            {
                _active = value;
                _controller.Active = value;
                Draw();
            }
        }

        public override int Left
        {
            get => _left;
            set
            {
                _left = value;
                _frameLeft = value + 2;
            }
        }

        public override int Top
        {
            get => _top;
            set
            {
                _top = value;
                _frameTop = value + 1;
            }
        }

        public override void Draw()
        {
            if(_active) DrawHovered();
            else PrintUtils.PrintBorder(Left, Top, Width, Height, BorderTemplate, BorderColor, BackgroundColor);
            DrawLines();
        }

        private void DrawHovered()
        {
            PrintUtils.PrintBorder(Left, Top, Width, Height, BorderTemplate, HoveredColor, BackgroundColor);
        }

        public void DrawLines()
        {
            int diff = _index - _frameHeight + 1;
            int i = diff > 0 ? diff : 0, 
                n = 0;

            while (i <= _index)
            {
                PrintUtils.PrintString(_frameLeft, _frameTop + n, _lines[i], TextColor, BackgroundColor);
                PrintUtils.PrintRect(_frameLeft + _lines[i].Length, _frameTop + n, _frameWidth - _lines[i].Length, 1, ' ', BackgroundColor, BackgroundColor);
                i++; n++;
            }

            PrintUtils.PrintString(Left + 2, Top + Height - 1, $" {_index + 1}/{_lines.Count} ", _active ? HoveredColor : BorderColor, BackgroundColor);
        }

        public void AddText(string text)
        {
            var temp = TextUtils.GetFittedText(_frameWidth, _frameHeight, text);
            if(_index == _lines.Count - 1) _index += temp.Count;
            _lines.AddRange(temp);
            DrawLines();
        }

        public void ScrollUp()
        {
            if(_index >= _frameHeight)
            {
                _index--;
                DrawLines();
            }
        }

        public void ScrollDown()
        {
            if (_index < _lines.Count - 1)
            {
                _index++;
                DrawLines();
            }
        }
    }
}
