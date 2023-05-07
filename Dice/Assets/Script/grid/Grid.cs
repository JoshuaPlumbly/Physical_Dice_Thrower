namespace Plumbly.Grid
{
    public class Grid<T>
    {
        private int _rows;
        private int _column;
        private T[,] _cells;

        public Grid(int rows, int collums)
        {
            _rows = rows;
            _column = collums;
            _cells = new T[_rows, _column];
        }

        public T GetCell(int row, int collum)
        {
            return _cells[row, collum];
        }

        public void SetCell(T value, int row, int collum)
        {
            _cells[row, collum] = value;
        }
    }
}