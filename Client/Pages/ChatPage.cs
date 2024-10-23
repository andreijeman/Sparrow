using ConsoleUI.Elements;
using Server.Postman;

namespace Client.Pages
{
    public class ChatPage : BasePage
    {
        private Table _table;
        private Scroll _usersScroll;
        private List<string> _users;
        public Scroll _scroll;
        public TextBox _textBox;
        public Button _connectButton;
        public ConsoleUI.Elements.Label _connectLabel;

        public ChatPage()
        {
            _usersScroll = new Scroll(12, 10, ConsoleKey.UpArrow, ConsoleKey.DownArrow) { Left =  61, Top = 1};
            _users = new List<string>();
            _scroll = new Scroll(60, 24, ConsoleKey.UpArrow, ConsoleKey.DownArrow);
            _textBox = new TextBox(60, 4) { Top = 24 };
            _connectLabel = new ConsoleUI.Elements.Label(12, 2) { Left = 61, Top = 12, Text = "Not\nconnected", TextColor = ConsoleColor.Red };
            _connectButton = new Button(12, 3) { Left = 61, Top = 14, Text = "Connect" };

            _table = new Table(6, 1, 6, 2, null, null, null, ConsoleKey.Tab);
            _table.AddElement(0, 0, _scroll);
            _table.AddElement(1, 0, _textBox);
            _table.AddElement(2, 0, new ConsoleUI.Elements.Label(12, 1) { Left = 61, Text = "Online users" });
            _table.AddElement(3, 0, _usersScroll);
            _table.AddElement(4, 0, _connectLabel);
            _table.AddElement(5, 0, _connectButton);
        }

        public void PrintReceivedPacket(Packet packet)
        {
            switch(packet.Label)
            {
                case Server.Postman.Label.Connected:
                    string t = packet.Sender;
                    _users.Add(packet.Sender);
                    _usersScroll.AddText(t);
                    _usersScroll.Draw();
                    break;
                case Server.Postman.Label.Disconnected:
                    _users.Remove(packet.Sender);
                    if (_users.Any()) _usersScroll.Text = string.Join("\n", _users.Where(x => x != null).Select(x => x));
                    else _usersScroll.Text = "";
                    _usersScroll.Draw();
                    break;
                case Server.Postman.Label.Message:
                    _scroll.AddText($"{packet.Sender}: {packet.Data}");
                    break;
                default: break;
            }
        }

        public void ConnectionAction(bool isConnected)
        {
            if(isConnected)
            {
                _connectLabel.Text = "Connected";
                _connectLabel.TextColor = ConsoleColor.Green;
                BasePage.CurrentPage = this;
            }
            else
            {
                _connectLabel.Text = "Not\nConnected";
                _connectLabel.TextColor = ConsoleColor.Red;
            }
        }
        
        public override void Show()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            _table.Draw();
        }

        public override void Run()
        {
            ResizeTask.ResizeEvent += Resize;
            _table.Active = true;
            Resize(Console.WindowWidth, Console.WindowHeight);
        }
        public override void Close()
        {
            _table.Active = false;
            ResizeTask.ResizeEvent -= Resize;
        }
        public override void Resize(int windowWidth, int windowHeight)
        {
            int temp = (windowWidth - _table.Width) / 2;
            _table.Left = temp < 0 ? 0 : temp;

            temp = (windowHeight - _table.Height) / 2;
            _table.Top = temp < 0 ? 0 : temp;
            Show();
        }
    }
}
