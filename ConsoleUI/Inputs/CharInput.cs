namespace ConsoleUI
{
    public delegate void CharEventHandler(char ch);

    public static class CharInput
    {
        public static event CharEventHandler? CharEvent;

        static CharInput()
        {
            KeyInput.KeyEvent += ProcessKey;
        }

        private static void ProcessKey(ConsoleKeyInfo keyInfo)
        {
            char ch = keyInfo.KeyChar;
            if(!char.IsControl(ch) && ch != '\0')
            {
                CharEvent?.Invoke(ch);
            }
        }
    }
}
