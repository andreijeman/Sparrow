using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public class Table : Interactive
    {
        public int Rows { get; init; }
        public int Columns { get; init; }

        private Element[,] _grid;

        private int _currentRow, _currentColumn;

        public override bool Active
        {
            get => _active;
            set
            {
                if(_active != value)
                {
                    _active = value;
                    _controller.Active = value; 
                    _grid[_currentRow, _currentColumn].Active = value;
                }
            }
        }


        public Table(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            _grid = new Element[rows, columns];

            _controller.AddKeyEvent(ConsoleKey.LeftArrow, ProcessLeftArrow);
            _controller.AddKeyEvent(ConsoleKey.RightArrow, ProcessRightArrow);
            _controller.AddKeyEvent(ConsoleKey.UpArrow, ProcessUpArrow);
            _controller.AddKeyEvent(ConsoleKey.DownArrow, ProcessDownArrow);
        }

        public void AddElement(int row, int column, Element element)
        {
            if(row >= 0 && row < Rows && column >= 0 && column < Columns)
            {
                _grid[row, column] = element;
                _currentRow = row;
                _currentColumn = column;

            }
        }

        public void ProcessRightArrow()
        {
            for(int i = _currentColumn + 1; i < Columns; i++)
            {
                if (_grid[_currentRow, i] != null)
                {
                    _grid[_currentRow, _currentColumn].Active = false;
                    _currentColumn = i;
                    _grid[_currentRow, _currentColumn].Active = true;
                    break;
                }
            }
        }

        public void ProcessLeftArrow()
        {
            for (int i = _currentColumn - 1; i >= 0; i--)
            {
                if (_grid[_currentRow, i] != null)
                {
                    _grid[_currentRow, _currentColumn].Active = false;
                    _currentColumn = i;
                    _grid[_currentRow, _currentColumn].Active = true;
                    break;
                }
            }

        }

        public void ProcessUpArrow()
        {
            for (int i = _currentRow - 1; i >= 0; i--)
            {
                if (_grid[i, _currentColumn] != null)
                {
                    _grid[_currentRow, _currentColumn].Active = false;
                    _currentRow = i;
                    _grid[_currentRow, _currentColumn].Active = true;
                    break;
                }
            }

        }

        public void ProcessDownArrow()
        {
            for (int i = _currentRow + 1; i < Rows; i++)
            {
                if (_grid[i, _currentColumn] != null)
                {
                    _grid[_currentRow, _currentColumn].Active = false;
                    _currentRow = i;
                    _grid[_currentRow, _currentColumn].Active = true;
                    break;
                }
            }
        }

    }
}
