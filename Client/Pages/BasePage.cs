using ConsoleUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Client
{
    public delegate void RedirectPageEventHandler(BasePage page);

    public abstract class BasePage
    {
        public event RedirectPageEventHandler? RedirectPage;

        public abstract void Render();
    }
}
