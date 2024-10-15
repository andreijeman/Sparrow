namespace ConsoleUI
{
    public class Button : Element
    {
        private int _textLeft, _textTop;
        private List<string>? _text;

        public string? Text 
        {
            set
            {
                if (value != null)
                {
                    _text = TextUtils.GetFittedText(Width - 4, Height - 2, value);
                    _textLeft = Left + 2;
                    _textTop = Top + (Height - _text.Count) / 2;
                }
                else _text = null;
            }
        }

        public override bool Active
        {
            get => _active;
            set
            {
                if (value != _active)
                {
                    _controller.Active = value;
                    _active = value;
                    if (value) _currentColor = HoveredColor;
                    else _currentColor = IdleColor;
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
            _controller.AddKeyEvent(ConsoleKey.Enter, EnterKeyEvent);
        }

        public override void Render()
        {
            Console.BackgroundColor = _currentColor;
            PrintUtils.PrintRect(Left, Top, Width, Height, ' ');

            if (_text != null)
            {
                Console.ForegroundColor = TextColor;
                PrintUtils.PrintText(_textLeft, _textTop, _text);
            }
        }

        public void EnterKeyEvent()
        {
            _currentColor = PressedColor;
            Render();
            Task.Delay(100).ContinueWith(t =>
            {
                _currentColor = HoveredColor;
                Render();
                Action?.Invoke();
            });
        }
    }
}
