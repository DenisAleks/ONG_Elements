using UnityEngine;

namespace _Game._Scripts.Lvl
{
    public sealed class GridPositioner
    {
        private readonly float _cellSize;
        private readonly float _startX;
        private readonly float _startY;

        public GridPositioner(int columns, float cellSize, float startY)
        {
            _cellSize = cellSize;
            _startY = startY;

            var totalWidth = columns * _cellSize;
            _startX = -totalWidth * 0.5f + _cellSize * 0.5f;
        }

        public Vector3 GetLocalPosition(int x, int y) 
            => new(_startX + x * _cellSize, _startY + y * _cellSize, 0f);
    }
}