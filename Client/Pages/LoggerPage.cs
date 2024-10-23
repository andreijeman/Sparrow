using ConsoleUI.Elements;
using Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Pages
{
    public class LoggerPage : BasePage, ILogger
    {
        private Table _table;
        private Scroll _scroll;
        private Label _label;

        private BasePage? _lastPage;

        public LoggerPage()
        {
            _label = new Label(20, 1) { Left = 0, Top = 0};
            _scroll = new Scroll(30, 6, ConsoleKey.UpArrow, ConsoleKey.DownArrow) { Left = 0, Top = 1};
            
            _table = new Table(3, 1, 4, 2, null, null, null, ConsoleKey.Tab);
            
            _table.AddElement(0, 0, _label);
            _table.AddElement(1, 0, _scroll);
            _table.AddElement(2, 0, new Button(8, 3) { Text = " Ok", Action = MyAction, Left = 32, Top = 3 });
        }

        public void MyAction()
        {
            CurrentPage = _lastPage;
        }

        public void LogError(string message)
        {
            _label.TextColor = ConsoleColor.Red;
            _label.Text = " Error ";
            _scroll.Text = message;
            _lastPage = CurrentPage;
            CurrentPage = this;
        }

        public void LogInfo(string message)
        {
            _label.TextColor = ConsoleColor.DarkGreen;
            _label.Text = " Info ";
            _scroll.Text = message;
            _lastPage = CurrentPage;
            CurrentPage = this;
        }

        public void LogWarning(string message)
        {

            _label.TextColor = ConsoleColor.DarkYellow;
            _label.Text = " Warning ";
            _scroll.Text = message;
            _lastPage = CurrentPage;
            CurrentPage = this;
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
