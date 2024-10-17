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

        public Label(int left, int top, int width, int height, string text) : base(left, top, width, height)
        {
            _text = TextUtils.GetFittedText(width, height, text);
            TextColor = ConsoleColor.DarkYellow;
            BackgroundColor = ConsoleColor.Black;
        }

        public override void Draw()
        {
            PrintUtils.PrintText(OriginLeft + Left, OriginTop + Top, _text, TextColor, BackgroundColor);
        }

        public void DrawHovered()
        {
            PrintUtils.PrintText(OriginLeft + Left, OriginTop + Top, _text, BackgroundColor, TextColor);
        }
    }
}
