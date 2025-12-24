using System;
using _Game._Scripts.Model.Enums;

namespace _Game._Scripts.Model.Data.Save
{
    [Serializable]
    public sealed class GridSaveData
    {
        public int Rows;
        public int Columns;
        public ElementType[] Cells;
    }
}