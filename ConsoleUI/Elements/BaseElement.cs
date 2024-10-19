using ConsoleUI.Interfaces;
using ConsoleUI.Inputs;

namespace ConsoleUI.Elements
{
    public abstract class BaseElement : IActivatable, IDrawable
    {
        protected Controller _controller = new Controller();

        protected int _left;
        protected int _top;

        public virtual int Left { get => _left; set => _left = value; }
        public virtual int Top { get => _top; set => _top = value; }
        public virtual int Width { get; protected set; }
        public virtual int Height { get; protected set; }

        public BaseElement(int width, int height)
        {
            Width = width;
            Height = height;
        }

        protected bool _active;
        public abstract bool Active { get; set; }

        public abstract void Draw();
    }
}
