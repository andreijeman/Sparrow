using ConsoleUI.Utils;

namespace ConsoleUI.Elements
{
    public class Table : BaseElement
    {
        public int Rows { get; init; }
        public int Columns { get; init; }

        private BaseElement[,] _grid;

        private int _currentRow, _currentColumn;

        public ConsoleColor BackgroundColor {  get; set; }
        public ConsoleColor BorderColor {  get; set; }
        public Char[] BorderTemplate {  get; set; }

        public override bool Active
        {
            get => _active;
            set
            {
                _active = value;
                _controller.Active = value; 
                _grid[_currentRow, _currentColumn].Active = value;
            }
        }

        public override int Left 
        { 
            get => _left; 
            set
            {
                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        if (_grid[i, j] != null)
                        {
                            _grid[i, j].Left += value - _left;
                        }
                    }
                }
                _left = value;
            }
        }

        public override int Top
        {
            get => _top;
            set
            {
                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        if (_grid[i, j] != null)
                        {
                            _grid[i, j].Top += value - _top;
                        }
                    }
                }
                _top = value;
            }
        }

        public int MarginLeft { get; init; }
        public int MarginTop { get; init; }

        public Table(int rows, int columns, int marginLeft, int marginTop, ConsoleKey? leftkey, ConsoleKey? rightKey, ConsoleKey? upKey, ConsoleKey? downKey) : base(0, 0)
        {
            Rows = rows;
            Columns = columns;
            _grid = new BaseElement[rows, columns];

            MarginLeft = marginLeft;
            MarginTop = marginTop;

            if(leftkey != null) _controller.AddKeyEvent((ConsoleKey)leftkey, ProcessLeftKey);
            if (rightKey != null) _controller.AddKeyEvent((ConsoleKey)rightKey, ProcessRightKey);
            if (upKey != null) _controller.AddKeyEvent((ConsoleKey)upKey, ProcessUpKey);
            if (downKey != null) _controller.AddKeyEvent((ConsoleKey) downKey, ProcessDownKey);

            BorderColor = ConsoleColor.DarkMagenta;
            BorderTemplate = Assets.Border3;
            BackgroundColor = ConsoleColor.Black;
        }

        public void AddElement(int row, int column, BaseElement element)
        {
            if(row >= 0 && row < Rows && column >= 0 && column < Columns)
            {
                int temp = element.Left + element.Width + 2 * MarginLeft;
                if(Width < temp) Width = temp;

                temp = element.Top + element.Height + 2 * MarginTop;
                if(Height < temp) Height = temp;

                element.Left += Left + MarginLeft;
                element.Top += Top + MarginTop;

                _grid[row, column] = element;
                //_currentRow = row;
                //_currentColumn = column;

            }
        }

        private void ProcessRightKey()
        {
            int temp = _currentColumn;

            for(int i = _currentColumn + 1; i < Columns; i++) if (_grid[_currentRow, i] != null) { _currentColumn = i; break; }
            
            if(_currentColumn == temp)
            {
                for (int i = 0; i < _currentColumn; i++) if (_grid[_currentRow, i] != null) { _currentColumn = i; break; }      
            }

            if (temp != _currentColumn)
            {
                _grid[_currentRow, temp].Active = false;
                _grid[_currentRow, _currentColumn].Active = true;                
            }
        }

        private void ProcessLeftKey()
        {
            int temp = _currentColumn;

            for (int i = _currentColumn - 1; i >= 0; i--) if (_grid[_currentRow, i] != null) { _currentColumn = i; break; }
            
            if (_currentColumn == temp)
            {
                for (int i = Columns - 1; i > _currentColumn; i--) if (_grid[_currentRow, i] != null) { _currentColumn = i; break; }
            }

            if (temp != _currentColumn)
            {
                _grid[_currentRow, temp].Active = false;
                _grid[_currentRow, _currentColumn].Active = true;
            }
        }

        private void ProcessUpKey()
        {
            int temp = _currentRow;

            for (int i = _currentRow - 1; i >= 0; i--) if (_grid[i, _currentColumn] != null) { _currentRow = i; break; }

            if (_currentRow == temp)
            {
                for (int i = Rows - 1; i > _currentRow; i--) if (_grid[i, _currentColumn] != null) { _currentRow = i; break; }
            }

            if (temp != _currentRow)
            {
                _grid[temp, _currentColumn].Active = false;
                _grid[_currentRow, _currentColumn].Active = true;
            }
        }

        private void ProcessDownKey()
        {
            int temp = _currentRow;

            for (int i = _currentRow + 1; i < Rows; i++) if (_grid[i, _currentColumn] != null) { _currentRow = i; break; }

            if (_currentRow == temp)
            {
                for (int i = 0; i < _currentRow; i++) if (_grid[i, _currentColumn] != null) { _currentRow = i; break; }
            }

            if (temp != _currentRow)
            {
                _grid[temp, _currentColumn].Active = false;
                _grid[_currentRow, _currentColumn].Active = true;
            }
        }

        public override void Draw()
        {
            PrintUtils.PrintRect(Left, Top, Width, Height, ' ', BackgroundColor, BackgroundColor);
            PrintUtils.PrintBorder(Left, Top, Width, Height, BorderTemplate, BorderColor, BackgroundColor);

            for(int i = 0; i < Rows; i++)
            {
                for(int j = 0; j < Columns; j++)
                {
                    if(_grid[i, j] != null) _grid[i, j].Draw();
                }
            }
        }
    }
}
