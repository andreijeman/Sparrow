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


        public Table(int rows, int columns)
        {
            Rows = rows;
            Columns = Columns;
            _grid = new Element[rows, columns];

            _controller.AddKeyEvent(ConsoleKey.LeftArrow, LeftArrowEvent);
            _controller.AddKeyEvent(ConsoleKey.RightArrow, RightArrowEvent);
            _controller.AddKeyEvent(ConsoleKey.UpArrow, UpArrowEvent);
            _controller.AddKeyEvent(ConsoleKey.DownArrow, DownArrowEvent);
        }

        public void AddElement(int row, int column, Element element)
        {
            if(row >= 0 && row < Rows && column >= 0 && column < Columns)
            {
                if (_grid[_currentRow, _currentColumn] != null) _grid[_currentRow, _currentColumn].Active = false;
                _grid[row, column] = element;
                _currentRow = row;
                _currentColumn = column;
                _grid[row, column].Active = true;

            }
        }

        public void LeftArrowEvent()
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

        public void RightArrowEvent()
        {
            for (int i = _currentColumn -1; i >= 0; i--)
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

        public void UpArrowEvent()
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

        public void DownArrowEvent()
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

    }
}
