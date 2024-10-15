namespace ConsoleUI
{
    public delegate void ControllerEventHandler();

    public class ControllerInput : BaseInput
    {
        private Dictionary<ConsoleKey, ControllerEventHandler> _keyEventsDict = new Dictionary<ConsoleKey, ControllerEventHandler>();

        private int n;
        protected override void ProcessKey(ConsoleKeyInfo keyInfo)
        {
            Console.SetCursorPosition(0, 2);
            Console.Write(n++);
            if (_keyEventsDict.ContainsKey(keyInfo.Key)) _keyEventsDict[keyInfo.Key].Invoke();
        }

        public void RegisterKey(ConsoleKey key, ControllerEventHandler keyEvent)
        {
            if (_keyEventsDict.ContainsKey(key)) _keyEventsDict[key] += keyEvent;
            else _keyEventsDict.Add(key, keyEvent);
        }
    }
}
