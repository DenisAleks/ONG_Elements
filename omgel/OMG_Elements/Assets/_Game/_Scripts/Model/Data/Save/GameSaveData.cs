using System;

namespace _Game._Scripts.Model.Data.Save
{
    [Serializable]
    public sealed class GameSaveData
    {
        public int LevelId;
        public GridSaveData Grid;
    }
}