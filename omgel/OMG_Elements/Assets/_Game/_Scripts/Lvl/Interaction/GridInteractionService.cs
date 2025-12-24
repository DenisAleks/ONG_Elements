using _Game._Scripts.Lvl.Elements;
using _Game._Scripts.Model.Enums;
using _Game._Scripts.Settings;

namespace _Game._Scripts.Lvl.Interaction
{
    //Handles all player interaction rules - decide if swipes valid or not
    public sealed class GridInteractionService : IGridInteractionService
    {
        private readonly GridController _grid;
        private readonly GameSettings _gameSettings;

        public GridInteractionService(GridController grid, GameSettings gameSettings)
        {
            _grid = grid;
            _gameSettings = gameSettings;
        }

        public bool CanInteract(Cell cell)
            => cell.ElementType != ElementType.None && cell.IsInteractable;

        public void TrySwipe(Cell fromCell, SwipeDirection direction)
        {
            if (!_gameSettings.AllowMultipleElementsInteraction)
            {
                if (_grid.IsBusy)
                {
                    return;
                }
            }

            if (!CanInteract(fromCell))
            {
                return;
            }

            if (!_grid.TryGetNeighbor(fromCell, direction, out var targetCell))
            {
                // no target found
                return;
            }

            // target exists but interaction is not allowed
            if (!CanInteractTarget(fromCell, targetCell, direction))
            {
                return;
            }

            _grid.MoveCell(fromCell, targetCell);
        }
        
        private bool CanInteractTarget(Cell from, Cell target, SwipeDirection direction)
        {
            // player can't swipe in the 'sky' even if cell of type None exists there 
            if (direction == SwipeDirection.Up && target.ElementType == ElementType.None)
            {
                return false;
            }

            return true;
        }
    }
}