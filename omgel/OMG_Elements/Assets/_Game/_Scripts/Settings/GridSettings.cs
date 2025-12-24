using System.Linq;
using _Game._Scripts.Lvl.Elements;
using _Game._Scripts.Utils;
using UnityEngine;

namespace _Game._Scripts.Settings
{
    [CreateAssetMenu(fileName = nameof(GridSettings),
        menuName = AppConstants.Paths.ScriptableObjects + nameof(GridSettings))]
    public class GridSettings : ScriptableObject
    {
        public int MaxElementsCount => RowsMinMax.y * ColumnsMinMax.y;
        
        public Cell CellPrefab;
        public ElementSettings[] ElementSettingsArray;
        public Vector2Int RowsMinMax = new(1, 10);
        public Vector2Int ColumnsMinMax = new(3, 6);
        public float[] ScalesPerColumns = {1f, 1f, 0.8f, 0.7f};
        public float GridPositionY = 20f;
        public float CellSizeDefault = 20f; 
        public float MinSwipeDistance = 20f;
        public float ElementAnimationTimeScale = 1f;
        public float ElementSwapAnimDuration = 0.25f;
        public float FallDurationPerCell = 0.2f;


        public float GetElementScale(int columns)
        {
            return ScalesPerColumns[columns - ColumnsMinMax.x];
        }
        
        private void OnValidate()
        {
            if (RowsMinMax.y < RowsMinMax.x)
            {
                Debug.LogError("Rows MinMax is incorrect, Min must be < Max");
                RowsMinMax.x = RowsMinMax.y;
            }
            
            if (ColumnsMinMax.y < ColumnsMinMax.x)
            {
                Debug.LogError("Columns MinMax is incorrect, Min must be < Max");
                ColumnsMinMax.x = ColumnsMinMax.y;
            }

            if (ScalesPerColumns.Length < ColumnsMinMax.y - ColumnsMinMax.x + 1)
            {
                Debug.LogError($"ScalesPerColumns size is incorrect. Must be equal to ColumnsMinMax.y - ColumnsMinMax.x");
                ScalesPerColumns = new float[ColumnsMinMax.y - ColumnsMinMax.x + 1];
                for (var i = 0; i < ScalesPerColumns.Length; i++)
                {
                    ScalesPerColumns[i] = 1f;
                }
            }
        }
    }
}