namespace ConsoleUI
{
    public delegate void KeyEventHandler(ConsoleKeyInfo keyInfo);

    public static class KeyInput
    {
        public static event KeyEventHandler? KeyEvent;

        static KeyInput()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    KeyEvent?.Invoke(Console.ReadKey(true));
                }
            });
        }
    }
}
