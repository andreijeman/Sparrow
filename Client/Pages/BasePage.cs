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
                _currentPage = value;
                _currentPage?.Run();
            }
        }

        public abstract void Show();
        public abstract void Run();
        public abstract void Close();
        public abstract void Resize(int windowWidth, int windowHeight);
    }
}
