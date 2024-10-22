using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Pages
{
    public abstract class BasePage
    {
        protected static BasePage? _currentPage;
        public static BasePage? CurrentPage 
        { 
            get => _currentPage; 
            set
            {
                _currentPage?.Close();
                Thread.Sleep(100);
                _currentPage = value;
                _currentPage?.Show();
            }
        }

        public abstract void Show();
        public abstract void Close();
        public abstract void Resize(int windowWidth, int windowHeight);
    }
}
