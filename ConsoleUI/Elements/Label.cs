using ConsoleUI.Utils;

namespace ConsoleUI.Elements
{
    public class Label : BaseElement
    {
        public override bool Active 
        { 
            get => _active; 
            set
            {
                _active = value;
                if (value) DrawHovered();
                else Draw();
            }
        }

        public ConsoleColor TextColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }

        private List<string> _text;
        public string Text
        {
            set { _text = TextUtils.GetFittedText(Width, Height, value); }
        }

        public Label(int width, int height) : base(width, height)
        {
            _text = new List<string>() { "Label" };
            TextColor = ConsoleColor.DarkYellow;
            BackgroundColor = ConsoleColor.Black;
        }

        public override void Draw()
        {
            PrintUtils.PrintText(Left, Top, _text, TextColor, BackgroundColor);
        }

        public void DrawHovered()
        {
            PrintUtils.PrintText(Left, Top, _text, BackgroundColor, TextColor);
        }
    }
}
