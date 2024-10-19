using ConsoleUI.Elements;

namespace Client.Pages
{
    public class ConnectPage : IPage
    {
        private Table _table = new Table(9, 1, 14, 2, null, null, ConsoleKey.UpArrow, ConsoleKey.DownArrow) { Left = 35, Top = 3};

        public ConnectPage() 
        {
            ResizeTask.ResizeEvent += Resize;
            Label label;

            label = new Label(20, 1) { Left = 0, Top = 0, Text = "Server ip" };
            _table.AddElement(0, 0, new Label(20, 1) { Left = 0, Top = 0, Text = "Server ip" });
            _table.AddElement(1, 0, new TextBox(20, 3) { Left = 0, Top = 1});
            _table.AddElement(6, 0, new Label(20, 1) { Left = 0, Top = 12, Text = "Server port"});
             _table.AddElement(7, 0, new TextBox(20, 3) { Left = 0, Top = 13});
            _table.AddElement(4, 0, new Label(20, 1) { Left = 0, Top = 8, Text = "Server password" });
             _table.AddElement(5, 0, new TextBox(20, 3) { Left = 0, Top = 9 });
            _table.AddElement(2, 0, new Label(20, 1) { Left = 0, Top = 4, Text = "Server port" });
             _table.AddElement(3, 0, new TextBox(20, 3) { Left = 0, Top = 5 });

            Button b = new Button(20, 3) { Left = 0, Top = 17, Text = "    Connect" };
            _table.AddElement(8, 0, b);


        }

        public void Show()
        {
            _table.Draw();
            _table.Active = true;
        }

        public void Close()
        {
            _table.Active = false;
        }

        public void Resize(int windowWidth, int windowHeight)
        {
            _table.Left = (windowWidth - _table.Width) / 2;
            _table.Top = (windowHeight - _table.Height) / 2;
            Console.Clear();
            _table.Draw();
        }
    }
}
