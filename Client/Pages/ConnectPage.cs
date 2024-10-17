using ConsoleUI.Elements;

namespace Client.Pages
{
    public class ConnectPage : BasePage
    {
        public override bool Active
        {
            get => _active = true;
            set => _active = value;
        }

        private Table _table;

        public ConnectPage(int originLeft, int originTop) : base(originLeft, originTop)
        {
            OriginLeft = originLeft;
            OriginTop = originTop;

            //_table = new Table(4, 1);
            //for (int i = 3; i >= 0; i--)
            //{
            //    TextBox t = new TextBox(OriginLeft, OriginTop + i * 4, 20, 3);
            //    _table.AddElement(i, 0, t);
            //}
        }

        public override void Draw()
        {
            
        }


    }
}
