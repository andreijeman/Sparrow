using ConsoleUI.Interfaces;

namespace Client.Pages
{
    public abstract class BasePage : IActivatable, IDrawable
    {
        public int OriginLeft { get; init; }
        public int OriginTop { get; init; }

        protected bool _active;
        public abstract bool Active { get; set; }
        public abstract void Draw();

        public BasePage(int originLeft, int originTop)
        {
            OriginLeft = originLeft;
            OriginTop = originTop;
        }


    }
}
