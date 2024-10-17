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

        public string Text 
        {
            set
            {
                _text = TextUtils.GetFittedText(Width - 4, Height - 2, value);
                _textLeft = Left + 2;
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
                if (value) { DrawHoverred(); _watch.Start(); }
                else { Draw(); _watch.Stop(); }
            }
        }

        public ConsoleColor IdleColor { get; set; }
        public ConsoleColor HoveredColor { get; set; }
        public ConsoleColor PressedColor { get; set; }

        public ConsoleColor TextColor { get; set; }

        public ActionEventHandler? Action { get; set; }

        public Button(int left, int top, int width, int height) : base(left, top, width, height) 
        {
            IdleColor = ConsoleColor.Magenta;
            HoveredColor = ConsoleColor.Blue;
            PressedColor = ConsoleColor.DarkBlue;
            TextColor = ConsoleColor.Gray;

            _text = new List<string>() { "Button" };

            _controller.AddKeyEvent(ConsoleKey.Enter, ProcessEnterKey);
            _controller.AddKeyEvent(ConsoleKey.Spacebar, ProcessEnterKey);

            _watch = new Stopwatch(); 
        }

        public override void Draw()
        {
            PrintUtils.PrintRect(OriginLeft + Left, OriginTop + Top, Width, Height, ' ', IdleColor, IdleColor);

            PrintUtils.PrintText(OriginLeft + _textLeft, OriginTop + _textTop, _text, TextColor, IdleColor);
        }

        public void DrawHoverred()
        {
            PrintUtils.PrintRect(OriginLeft + Left, OriginTop + Top, Width, Height, ' ', HoveredColor, HoveredColor);

            PrintUtils.PrintText(OriginLeft + _textLeft, OriginTop + _textTop, _text, TextColor, HoveredColor);
        }

        public void DrawPressed()
        {
            PrintUtils.PrintRect(OriginLeft + Left, OriginTop + Top, Width, Height, ' ', PressedColor, PressedColor);

            PrintUtils.PrintText(OriginLeft + _textLeft, OriginTop + _textTop, _text, TextColor, PressedColor);
        }

        public void ProcessEnterKey()
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
