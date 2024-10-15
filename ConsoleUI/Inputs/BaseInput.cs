namespace ConsoleUI
{
    public abstract class BaseInput : Activatable
    {
        public override bool Active
        {
            get => _active;
            set
            {
                if (value != _active)
                {
                    if (value) InputThread.KeyEvent += ProcessKey;
                    else InputThread.KeyEvent -= ProcessKey;

                    _active = value;
                }
            }
        }

        protected abstract void ProcessKey(ConsoleKeyInfo keyInfo);
    }
}
