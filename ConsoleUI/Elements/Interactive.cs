using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public abstract class Interactive : Activatable
    {
        public override bool Active
        {
            get => _active;
            set
            {
                if (_active != value)
                {
                    _active = value;
                    _controller.Active = true;
                }
            }
        }

        protected readonly Controller _controller = new Controller();
    }
}
