using _Game._Scripts.Model.Enums;

namespace _Game._Scripts.Model.Levels
{
    public static class LevelTableParser
    {
        public static LevelTable Parse(int id, string csv)
        {
            var lines = csv.Split('\n');

            var rows = lines.Length;
            var columns = lines[0].Split(',').Length;

            var table = new LevelTable(id, rows, columns);

            for (var r = 0; r < rows; r++)
            {
                var values = lines[r].Trim().Split(',');

                var flippedRow = rows - 1 - r;
                for (var c = 0; c < columns; c++)
                {
                    table.Grid[flippedRow, c] = (ElementType)int.Parse(values[c]);
                }
            }

            return table;
        }
    }
}