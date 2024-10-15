using ConsoleUI;

namespace ConsoleUI
{
    public abstract class Element : Interactive
    {
        public int Left { get; init; } 
        public virtual int Top { get; init; }
        public virtual int Width { get; init; }
        public virtual int Height { get; init; }

        public Element(int left, int top, int width, int height)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }

        public abstract void Render();
    }
}
