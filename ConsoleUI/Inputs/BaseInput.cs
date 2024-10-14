namespace ConsoleUI
{
    public abstract class BaseInput
    {
        private bool _active;

        public virtual bool Active
        {
            get => _active;
            set
            {
                if (value != _active)
                {
                    if (value) ConsoleInput.KeyEvent += ProcessKey;
                    else ConsoleInput.KeyEvent -= ProcessKey;

                    _active = value;
                }
            }
        }

        protected abstract void ProcessKey(ConsoleKeyInfo keyInfo);
    }
}
