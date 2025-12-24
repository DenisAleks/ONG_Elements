using System.Collections.Generic;
using _Game._Scripts.Model.Enums;

namespace _Game._Scripts.Lvl.Elements
{
    public sealed class GridBlaster
    {
        private readonly GridController _grid;
        private readonly ElementPool _elementPool;

        private int _activeDestroys;

        public GridBlaster(GridController grid, ElementPool elementPool)
        {
            _grid = grid;
            _elementPool = elementPool;
        }

        public void BlastGroups(List<List<Cell>> groups)
        {
            _activeDestroys = 0;

            foreach (var group in groups)
            {
                foreach (var cell in group)
                {
                    BlastCell(cell);
                }
            }
        }

        private void BlastCell(Cell cell)
        {
            cell.SetState(CellState.Destroying);

            if (!_grid.TryGetElement(cell, out var element))
                return;

            _activeDestroys++;

            element.Blast(() =>
            {
                RemoveElement(cell, element);

                _activeDestroys--;
                if (_activeDestroys == 0)
                {
                    _grid.ApplyGravity();
                }
            });
        }

        private void RemoveElement(Cell cell, Element element)
        {
            cell.SetElement(ElementType.None);
            cell.SetState(CellState.Idle);

            _grid.RemoveElement(element);
            _elementPool.Release(element);
        }
    }
}