using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGraphics
{
    public class Image
    {
        public int Width { get; }
        public int Height { get; }

        public Cell[,] Grid { get; }

        public void Print(int left, int top)
        {
            if (left >= 0 && left + Width < Console.BufferWidth &&
                top >= 0 && top + Height < Console.BufferHeight)
            {
                ConsoleColor predColor = ConsoleColor.White;
                for (int i = 0; i < Height; i++)
                {
                    Console.SetCursorPosition(left, top + i);
                    for (int j = 0; j < Width; j++)
                    {
                        if (Grid[i, j].Color != predColor)
                        {
                            predColor = Grid[i, j].Color;
                            Console.ForegroundColor = predColor;
                        }
                        if (Grid[i, j].Texture == '\0') Console.SetWindowSize(10, 10);
                        else Console.Write(Grid[i, j].Texture);
                    }
                }
            }
        }

        public void SetCell(int left, int top, char texture, ConsoleColor color)
        {
            if (left >= 0 && left < Width && top >= 0 && top < Height)
            {
                Grid[top, left].Texture = char.IsControl(texture) ? ' ' : texture;
                Grid[top, left].Color = color;
            }
        }

        public void Clear()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Grid[i, j].Texture = ' ';
                }
            }
        }

        public Image(int width, int height)
        {
            Width = width;
            Height = height;
            Grid = new Cell[Height, Width];
            Clear();
        }

        public struct Cell
        {
            public char Texture;
            public ConsoleColor Color;

            public Cell(char texture, ConsoleColor color)
            {
                Texture = texture;
                Color = color;
            }
        }
    }
}
