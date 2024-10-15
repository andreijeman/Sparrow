namespace ConsoleUI
{
    public delegate void KeyEventHandler(ConsoleKeyInfo keyInfo);

    public static class InputThread
    {
        public static event KeyEventHandler? KeyEvent;

        static InputThread()
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
