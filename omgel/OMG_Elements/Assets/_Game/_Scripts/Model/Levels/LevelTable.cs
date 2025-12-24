using _Game._Scripts.Model.Enums;

namespace _Game._Scripts.Model.Levels
{
    public sealed class LevelTable
    {
        public readonly int Id;
        public readonly int Rows;
        public readonly int Columns;
        public readonly ElementType[,] Grid;

        public LevelTable(int id, int rows, int columns)
        {
            Id = id;
            Rows = rows;
            Columns = columns;
            Grid = new ElementType[rows, columns];
        }
    }
}