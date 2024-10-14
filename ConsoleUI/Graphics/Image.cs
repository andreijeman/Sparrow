namespace ConsoleUI
{
    public class Image
    {
        public int Width { get; }
        public int Height { get; }

        public Cell[,] Grid { get; }

        public Image(int width, int height)
        {
            Width = width;
            Height = height;
            Grid = new Cell[Height, Width];
            Clear();
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
