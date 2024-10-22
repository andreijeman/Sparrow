using ConsoleUI.Elements;

namespace Client.Pages
{
    public class ConnectPage : BasePage
    {
        public delegate Task<bool> ConnectEventHandler(string ip, string port, string password, string username);
        public event ConnectEventHandler? ConnectEvent;

        private Table _table;
        private TextBox _serverIp;
        private TextBox _serverPort;
        private TextBox _serverPassword;
        private TextBox _username;

        public ConnectPage() 
        {
            _table = new Table(9, 1, 14, 2, null, null, ConsoleKey.UpArrow, ConsoleKey.DownArrow) { Left = 35, Top = 3 };
            _serverIp = new TextBox(20, 3) { Left = 0, Top = 1 };
            _serverPort = new TextBox(20, 3) { Left = 0, Top = 5 };
            _serverPassword = new TextBox(20, 3) { Left = 0, Top = 9 };
            _username = new TextBox(20, 3) { Left = 0, Top = 13 };

            _table.AddElement(0, 0, new Label(20, 1) { Left = 0, Top = 0, Text = "Server ip" });
            _table.AddElement(1, 0, _serverIp);
            _table.AddElement(2, 0, new Label(20, 1) { Left = 0, Top = 4, Text = "Server port"});
             _table.AddElement(3, 0, _serverPort);
            _table.AddElement(4, 0, new Label(20, 1) { Left = 0, Top = 8, Text = "Server password" });
             _table.AddElement(5, 0, _serverPassword);
            _table.AddElement(6, 0, new Label(20, 1) { Left = 0, Top = 12, Text = "Username" });
             _table.AddElement(7, 0, _username);

            _table.AddElement(8, 0, new Button(20, 3) { Left = 0, Top = 17, Text = "    Connect", 
                Action = () =>  ConnectEvent?.Invoke(_serverIp.Text, _serverPort.Text, _serverPassword.Text, _username.Text) });
        }

        public override void Show()
        {
            _table.Draw();
            _table.Active = true;
        }

        public override void Close()
        {
            _table.Active = false;
        }

        public override void Resize(int windowWidth, int windowHeight)
        {
            _table.Left = (windowWidth - _table.Width) / 2;
            _table.Top = (windowHeight - _table.Height) / 2;
            Console.Clear();
            _table.Draw();
        }
    }
}
