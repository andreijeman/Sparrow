using ConsoleUI.Interfaces;
using ConsoleUI.Inputs;

namespace ConsoleUI.Elements
{
    public abstract class BaseElement : IActivatable, IDrawable
    {
        protected Controller _controller = new Controller();

        public virtual int OriginLeft { get; set; }
        public virtual int OriginTop { get; set; }
        public int Left { get; protected set; }
        public virtual int Top { get; protected set; }
        public virtual int Width { get; protected set; }
        public virtual int Height { get; protected set; }

        public BaseElement(int left, int top, int width, int height)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }

        protected bool _active;
        public abstract bool Active { get; set; }

        public abstract void Draw();
    }
}
