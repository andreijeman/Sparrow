using ConsoleUI.Elements;

namespace Client.Pages
{
    public class ConnectPage : IPage
    {
        private Table _table = new Table(0, 0, 9, 1, 14, 2, null, null, ConsoleKey.UpArrow, ConsoleKey.DownArrow);

        public ConnectPage() 
        {
            ResizeTask.ResizeEvent += Resize;

            _table.AddElement(0, 0, new Label(0, 0, 20, 1, "Server ip"));
             _table.AddElement(1, 0, new TextBox(0, 1, 20, 3));
            _table.AddElement(6, 0, new Label(0, 12, 20, 1, "Your username"));
             _table.AddElement(7, 0, new TextBox(0, 13, 20, 3));
            _table.AddElement(4, 0, new Label(0, 8, 20, 1, "Server password"));
             _table.AddElement(5, 0, new TextBox(0, 9, 20, 3));
            _table.AddElement(2, 0, new Label(0, 4, 20, 1, "Server port"));
             _table.AddElement(3, 0, new TextBox(0, 5, 20, 3));

            Button b = new Button(0, 17, 20, 3);
            b.Text = "    Connect";
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

        }
    }
}
