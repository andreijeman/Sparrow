namespace ConsoleUI
{
    public abstract class BaseElement : Activatable
    {
        public int Left { get; set; }
        public int Top { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        protected ControllerInput _controller = new ControllerInput();

        public override bool Active
        {
            get => _active;
            set
            {
                if (value != _active)
                {                    
                    _controller.Active = value;
                    _active = value;
                }
            }
        }

        public abstract void Render();

        public abstract void Update();
    }
}
