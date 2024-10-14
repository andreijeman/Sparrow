namespace ConsoleUI
{
    public delegate void TextEventHandler(string text);

    public class TextInput : BaseInput
    {
        public event TextEventHandler? TextEvent;
        public event TextEventHandler? EnterEvent;

        private char[] _buffer;
        private int _size;
        private int _index;

        public TextInput(int size)
        {
            _buffer = new char[size];   
            _size = size;
            _index = 0;
        }

        protected override void ProcessKey(ConsoleKeyInfo keyInfo)
        {
            char ch = keyInfo.KeyChar;
            if(!char.IsControl(ch) && ch != '\0')
            {
                if(_index < _size - 1)
                {
                    _buffer[++_index] = ch;
                    TextEvent?.Invoke(new string(_buffer, 0, _index + 1));
                }
            }
            else if(keyInfo.Key == ConsoleKey.Backspace)
            {
                if(_index > 0) TextEvent?.Invoke(new string(_buffer, 0, _index--));
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                EnterEvent?.Invoke(new string(_buffer, 0, _index + 1));
                _index = 0;
            }
        }
    }
}
