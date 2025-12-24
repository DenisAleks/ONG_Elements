using _Game._Scripts.Model.Enums;
using _Game._Scripts.Settings;
using DG.Tweening;

namespace _Game._Scripts.Lvl.Elements
{
    /// <summary>
    ///     Handles gravity for the whole grid.
    ///     Y = 0 is bottom, Y increases upwards.
    /// </summary>
    public sealed class GridGravity
    {
        private readonly GridController _grid;
        private readonly GameSettings _settings;

        private int _activeFalls;

        public GridGravity(GridController grid, GameSettings settings)
        {
            _grid = grid;
            _settings = settings;
        }

        public void ApplyGravity()
        {
            _activeFalls = 0;

            for (var x = 0; x < _grid.Columns; x++)
            {
                ApplyGravityToColumn(x);
            }

            // nothing moved → resolve immediately
            if (_activeFalls == 0)
            {
                _grid.Normalize();
            }
        }

        private void ApplyGravityToColumn(int x)
        {
            var grid = _grid.GetCellGrid();
            var emptyCount = 0;

            // bottom -> top
            for (var y = 0; y < _grid.Rows; y++)
            {
                var cell = grid[y, x];

                if (cell.ElementType == ElementType.None)
                {
                    emptyCount++;
                    continue;
                }

                if (emptyCount == 0)
                {
                    continue;
                }

                var targetY = y - emptyCount;
                var target = grid[targetY, x];

                Fall(cell, target);
            }
        }

        private void Fall(Cell from, Cell to)
        {
            if (!_grid.TryGetElement(from, out var element))
            {
                return;
            }

            LockColumn(from, to);

            from.SetElement(ElementType.None);
            to.SetElement(element.Type);

            _grid.ExtractElement(from);
            _grid.BindElement(element, to);

            var distance = from.MatrixPosition.y - to.MatrixPosition.y;
            var duration = distance * _settings.GridSettings.FallDurationPerCell;

            _activeFalls++;

            element.transform
                .DOLocalMove(to.transform.localPosition, duration)
                .SetEase(Ease.InQuad)
                .OnComplete(() =>
                {
                    UnlockColumn(from, to);

                    _activeFalls--;
                    if (_activeFalls == 0)
                    {
                        _grid.Normalize();
                    }
                });
        }

        private void LockColumn(Cell from, Cell to)
        {
            var x = from.MatrixPosition.x;

            for (var y = to.MatrixPosition.y; y <= from.MatrixPosition.y; y++)
            {
                _grid.GetCellGrid()[y, x].SetState(CellState.Falling);
            }
        }

        private void UnlockColumn(Cell from, Cell to)
        {
            var x = from.MatrixPosition.x;

            for (var y = to.MatrixPosition.y; y <= from.MatrixPosition.y; y++)
            {
                _grid.GetCellGrid()[y, x].SetState(CellState.Idle);
            }
        }
    }
}