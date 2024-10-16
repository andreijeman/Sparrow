using System.Diagnostics;

namespace ConsoleUI
{
    public delegate void ActionEventHandler();

    public class Controller : Activatable
    {
        private Dictionary<ConsoleKey, ActionEventHandler?> _keyEventsDict = new Dictionary<ConsoleKey, ActionEventHandler?>();

        public override bool Active
        {
            get => _active;
            set
            {
                if (value != _active)
                {
                    if (value) KeyInput.KeyEvent += ProcessKey;
                    else KeyInput.KeyEvent -= ProcessKey;

                    _active = value;
                }
            }
        }

        private void ProcessKey(ConsoleKeyInfo keyInfo)
        {
            if (_keyEventsDict.ContainsKey(keyInfo.Key)) _keyEventsDict[keyInfo.Key]?.Invoke();
        }

        public void AddKeyEvent(ConsoleKey key, ActionEventHandler action)
        {
            if (_keyEventsDict.ContainsKey(key)) _keyEventsDict[key] += action;
            else _keyEventsDict.Add(key, action);
        }

        public void RemoveKeyEvent(ConsoleKey key, ActionEventHandler action)
        {
            if (_keyEventsDict.ContainsKey(key))
            {
                _keyEventsDict[key] -= action;
                if (_keyEventsDict[key] != null) _keyEventsDict.Remove(key);
            }
        }
    }
}
