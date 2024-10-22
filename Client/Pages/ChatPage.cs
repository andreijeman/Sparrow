using ConsoleUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Pages
{
    public class ChatPage : BasePage
    {
        private Table _table;

        public ChatPage()
        {
            _table = new Table(2, 1, 4, 2, null, null, null, ConsoleKey.Tab);
        }
        

        public override void Close()
        {
            throw new NotImplementedException();
        }

        public override void Resize(int windowWidth, int windowHeight)
        {
            throw new NotImplementedException();
        }

        public override void Show()
        {
            throw new NotImplementedException();
        }
    }
}
