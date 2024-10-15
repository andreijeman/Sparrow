namespace ConsoleUI
{
    public class Button : BaseElement
    {
        private int _textLeft, _textTop;
        private string? _text;
        public string? Text 
        {
            get => _text;

            set
            {
                _text = value;
                if(_text != null)
                {
                    _textLeft = (Width - _text.Length) / 2;
                    _textTop = Height / 2;
                }
            }
        }

        public ConsoleColor Color { get; set; }
        public ConsoleColor TextColor { get; set; }

        public Button(ControllerEventHandler action)
        {
            _controller.RegisterKey(ConsoleKey.Enter, action);
            _controller.RegisterKey(ConsoleKey.Enter, Update);
        }

        public override void Render()
        {
            Console.ForegroundColor = Color;
            PrintUtils.PrintRect(Left, Top, Width, Height, '█');
            if(_text != null) PrintUtils.PrintString(_textLeft, _textTop, _text);
        }

        public override void Update()
        {
            //Console.ForegroundColor = ConsoleColor.Blue;
            //PrintUtils.PrintRect(Left, Top, Width, Height, '█');
            //if (_text != null) PrintUtils.PrintString(_textLeft, _textTop, _text);

            
        }
    }
}
