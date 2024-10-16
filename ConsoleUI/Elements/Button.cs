namespace ConsoleUI
{
    public class Button : Element
    {
        private int _textLeft, _textTop;
        private List<string> _text;

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
                if (value != _active)
                {
                    _active = value;
                    _controller.Active = value;
                    if (value) RenderHoverred();
                    else Render();

                }
            }
        }

        private ConsoleColor _currentColor;
        public ConsoleColor IdleColor { get; set; }
        public ConsoleColor HoveredColor { get; set; }
        public ConsoleColor PressedColor { get; set; }

        public ConsoleColor TextColor { get; set; }

        public ActionEventHandler? Action { get; set; }

        public Button(int left, int top, int width, int height) : base(left, top, width, height) 
        {
            IdleColor = ConsoleColor.Magenta;
            HoveredColor = ConsoleColor.Blue;
            PressedColor = ConsoleColor.DarkYellow;
            TextColor = ConsoleColor.White;

            Text = "Button";

            _controller.AddKeyEvent(ConsoleKey.Enter, ProcessEnterKey);
            _controller.AddKeyEvent(ConsoleKey.Spacebar, ProcessEnterKey);
        }

        public override void Render()
        {
            PrintUtils.PrintRect(Left, Top, Width, Height, ' ', IdleColor, IdleColor);

            PrintUtils.PrintText(_textLeft, _textTop, _text, TextColor, IdleColor);
        }

        public void RenderHoverred()
        {
            PrintUtils.PrintRect(Left, Top, Width, Height, ' ', HoveredColor, HoveredColor);

            PrintUtils.PrintText(_textLeft, _textTop, _text, TextColor, HoveredColor);
        }

        public void RenderPressed()
        {
            PrintUtils.PrintRect(Left, Top, Width, Height, ' ', PressedColor, PressedColor);

            PrintUtils.PrintText(_textLeft, _textTop, _text, TextColor, PressedColor);
        }

        public void ProcessEnterKey()
        {
            RenderPressed();

            Task.Delay(124).ContinueWith(t =>
            {
                if(_active) RenderHoverred();
                else Render();

                Action?.Invoke();
            });
        }
    }
}
