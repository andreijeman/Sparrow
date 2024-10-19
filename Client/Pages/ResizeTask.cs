using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Pages
{
    public delegate void ResizeEventHandler(int widt, int height);
    public static class ResizeTask
    {
        public static event ResizeEventHandler? ResizeEvent;

        static ResizeTask()
        {
            Task.Run(async () =>
            {
                int initialWidth = Console.WindowWidth;
                int initialHeight = Console.WindowHeight;

                while (true)
                {
                    if (Console.WindowWidth != initialWidth || Console.WindowHeight != initialHeight)
                    {
                        initialWidth = Console.WindowWidth;
                        initialHeight = Console.WindowHeight;

                        ResizeEvent?.Invoke(initialWidth, initialHeight);
                    }

                await Task.Delay(100);
                }
            });
        }
    }
}
