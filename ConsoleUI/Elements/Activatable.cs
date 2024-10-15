namespace ConsoleUI
{
    public abstract class Activatable
    {
        protected bool _active = false;
        public virtual bool Active { get; set; }
    }
}
