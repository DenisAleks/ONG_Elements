using _Game._Scripts.Settings;
using DG.Tweening;

namespace _Game._Scripts.Lvl.Elements
{
    /// <summary>
    ///     Performs the logical swap and the animation of two cells.
    /// </summary>
    public class GridSwapper
    {
        private readonly GridController _grid;
        private readonly GameSettings _settings;

        public GridSwapper(GridController grid, GameSettings settings)
        {
            _grid = grid;
            _settings = settings;
        }

        public void Swap(Cell from, Cell to)
        {
            _grid.SetBusy();
            
            SwapCellTypes(from, to);

            var fromEl = _grid.ExtractElement(from);
            var toEl = _grid.ExtractElement(to);

            from.SetState(CellState.Moving);
            to.SetState(CellState.Moving);
            
            AnimateSwap(from, to, fromEl, toEl);
        }

        private void SwapCellTypes(Cell a, Cell b)
        {
            var tmp = a.ElementType;
            a.SetElement(b.ElementType);
            b.SetElement(tmp);
        }

        private void AnimateSwap(Cell from, Cell to,
            Element fromEl, Element toEl)
        {
            var duration = _settings.GridSettings.ElementSwapAnimDuration;
            var seq = DOTween.Sequence();

            if (fromEl != null)
            {
                _grid.BindElement(fromEl, to);
                seq.Join(fromEl.transform.DOLocalMove(to.transform.localPosition, duration));
            }

            if (toEl != null)
            {
                _grid.BindElement(toEl, from);
                seq.Join(toEl.transform.DOLocalMove(from.transform.localPosition, duration));
            }

            seq.OnComplete(() =>
            {
                from.SetState(CellState.Idle);
                to.SetState(CellState.Idle);
                _grid.ApplyGravity();
            });
        }
    }
}