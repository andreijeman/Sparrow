namespace ConsoleUI
{
    public delegate void KeyEventHandler(ConsoleKeyInfo keyInfo);

    public static class ConsoleInput
    {
        public static event KeyEventHandler? KeyEvent;

        static ConsoleInput()
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
