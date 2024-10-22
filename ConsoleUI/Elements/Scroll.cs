using ConsoleUI.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI.Elements
{
    public class Scroll : BaseElement
    {
        private List<string> _lines;
        private int _index, _frameWidth, _frameHeight, _frameLeft, _frameTop;

        private Stopwatch _watch;
        private int _thumbLeft;
        private int _thumbTop;

        public char ThumbTexture {  get; set; }
        public char[] BorderTemplate { get; set; }
        public ConsoleColor BorderColor { get; set; }
        public ConsoleColor TextColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }
        public ConsoleColor HoveredColor { get; set; }

        public Scroll(int width, int height, ConsoleKey upKey, ConsoleKey downKey) : base(width, height) 
        { 
            _lines = new List<string>();
            _index = 0;
            _frameWidth = width - 4;
            _frameHeight = height - 2;
            _frameLeft = Left + 2;
            _frameTop = Top + 1;

            ThumbTexture = '█';
            BorderTemplate = Assets.Border1;
            BorderColor = ConsoleColor.Magenta;
            TextColor = ConsoleColor.White;
            BackgroundColor = ConsoleColor.Black;
            HoveredColor = ConsoleColor.DarkCyan;

            _controller.AddKeyEvent(upKey, ScrollUp);
            _controller.AddKeyEvent(downKey, ScrollDown);

            _thumbLeft = Left + Width - 1;
            _thumbTop = Top + 1;

            _watch = Stopwatch.StartNew();
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
                _thumbLeft = value + Width - 1;
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
            if(_active) PrintUtils.PrintBorder(Left, Top, Width, Height, BorderTemplate, HoveredColor, BackgroundColor);
            else PrintUtils.PrintBorder(Left, Top, Width, Height, BorderTemplate, BorderColor, BackgroundColor);
            
            DrawLines();
            DrawThumb();
        }

        public void DrawLines()
        {
            int i = _index;
            int diff = _lines.Count - _index;
            int lim = diff < _frameHeight ? diff : _frameHeight;
            int n = 0;

            while (n < lim)
            {
                PrintUtils.PrintString(_frameLeft, _frameTop + n, _lines[i], TextColor, BackgroundColor);
                PrintUtils.PrintRect(_frameLeft + _lines[i].Length, _frameTop + n, _frameWidth - _lines[i].Length, 1, ' ', BackgroundColor, BackgroundColor);
                i++; n++;
            }
        }

        private void DrawThumb()
        {
            int temp = _thumbTop;
            float ratio = _lines.Count > 0 ? (float)_index / (_lines.Count > _frameHeight ? _lines.Count - _frameHeight : _lines.Count) : 0;
            _thumbTop = Top + 1 + (int)(ratio * (_frameHeight - 1));

            PrintUtils.PrintChar(_thumbLeft, temp, BorderTemplate[7], _active ? HoveredColor : BorderColor, BackgroundColor);
            PrintUtils.PrintChar(_thumbLeft, _thumbTop, ThumbTexture, _active ? HoveredColor : BorderColor, BackgroundColor);
           
        }

        public void AddText(string text)
        {
            var temp = TextUtils.GetFittedText(_frameWidth, _frameHeight, text);
            _lines.AddRange(temp);
            if(_lines.Count - 1 - _index == _frameHeight) _index += temp.Count;
            DrawLines();
            DrawThumb();
        }

        public void ScrollUp()
        {
            _watch.Stop();
            if (_watch.Elapsed.TotalMilliseconds > 20 && _index > 0)
            {
                _index--;
                DrawLines();
                DrawThumb();

                _watch.Restart();
            }
            _watch.Start();
        }

        public void ScrollDown()
        {
            _watch.Stop();
            if (_watch.Elapsed.TotalMilliseconds > 20 && _index < _lines.Count - _frameHeight)
            {
                _index++;
                DrawLines();
                DrawThumb();

                _watch.Restart();
            }
            _watch.Start();
        }

        public void SetText(string text)
        {
            _lines.Clear();
            AddText(text);
        }
    }
}
