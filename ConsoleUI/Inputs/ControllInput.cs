namespace ConsoleUI
{
    public delegate void ControllKeyEventHandler();

    public class ControllInput : BaseInput
    {
        private Dictionary<ConsoleKey, ControllKeyEventHandler> _keyEventsDict = new Dictionary<ConsoleKey, ControllKeyEventHandler>();

        protected override void ProcessKey(ConsoleKeyInfo keyInfo)
        {
            if (_keyEventsDict.ContainsKey(keyInfo.Key)) _keyEventsDict[keyInfo.Key].Invoke();
        }

        public void RegisterKey(ConsoleKey key, ControllKeyEventHandler keyEvent)
        {
            if (_keyEventsDict.ContainsKey(key)) _keyEventsDict[key] += keyEvent;
            else _keyEventsDict.Add(key, keyEvent);
        }
    }
}
