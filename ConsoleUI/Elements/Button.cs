using System.Diagnostics;
using ConsoleUI.Inputs;
using ConsoleUI.Utils;

namespace ConsoleUI.Elements
{
    public class Button : BaseElement
    {
        private int _textLeft, _textTop;
        private List<string> _text;
        private Stopwatch _watch;
        public ConsoleColor IdleColor { get; set; }
        public ConsoleColor HoveredColor { get; set; }
        public ConsoleColor PressedColor { get; set; }

        public ConsoleColor TextColor { get; set; }

        public ActionEventHandler? Action { get; set; }

        public string Text 
        {
            set
            {
                _text = TextUtils.GetFittedText(Width - 4, Height - 2, value);
                _textTop = Top + (Height - _text.Count) / 2;

            }
        }

        public override bool Active
        {
            get => _active;
            set
            {
                _active = value;
                _controller.Active = value;
                if (value) _watch.Start(); 
                else _watch.Stop(); 
                Draw();
            }
        }
        
        public override int Left
        {
            get => _left;
            set
            {
                _left = value;
                _textLeft = value + 2;
            }
        }
        public override int Top
        {
            get => _top;
            set
            {
                _top = value;
                _textTop = value + 1;
            }
        }


        public Button(int width, int height) : base(width, height) 
        {
            IdleColor = ConsoleColor.Magenta;
            HoveredColor = ConsoleColor.Blue;
            PressedColor = ConsoleColor.DarkBlue;
            TextColor = ConsoleColor.Gray;

            _text = new List<string>() { "Button" };

            _controller.AddKeyEvent(ConsoleKey.Enter, ProcessEnterKey);
            _controller.AddKeyEvent(ConsoleKey.Spacebar, ProcessEnterKey);

            _watch = Stopwatch.StartNew(); 
        }

        public override void Draw()
        {
            if(_active) DrawHoverred();
            else 
            {
                PrintUtils.PrintRect(Left, Top, Width, Height, ' ', IdleColor, IdleColor);
                PrintUtils.PrintText(_textLeft, _textTop, _text, TextColor, IdleColor);
            }
        }

        private void DrawHoverred()
        {
            PrintUtils.PrintRect(Left, Top, Width, Height, ' ', HoveredColor, HoveredColor);

            PrintUtils.PrintText(_textLeft, _textTop, _text, TextColor, HoveredColor);
        }

        private void DrawPressed()
        {
            PrintUtils.PrintRect(Left, Top, Width, Height, ' ', PressedColor, PressedColor);

            PrintUtils.PrintText(_textLeft, _textTop, _text, TextColor, PressedColor);
        }

        private void ProcessEnterKey()
        {
            _watch.Stop();
            if(_watch.Elapsed.TotalMilliseconds > 200)
            {

                DrawPressed();

                Task.Delay(124).ContinueWith(t =>
                {
                    if(_active) DrawHoverred();
                    else Draw();

                    Action?.Invoke();
                });

                _watch.Restart();
            }
            _watch.Start();
        }
    }
}
