using _Game._Scripts.Lvl.Elements;
using _Game._Scripts.Model.Enums;

namespace _Game._Scripts.Lvl.Interaction
{
    public interface IGridInteractionService
    {
        void TrySwipe(Cell fromCell, SwipeDirection direction);
        bool CanInteract(Cell cell);
    }
}