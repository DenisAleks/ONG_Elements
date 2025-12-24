using _Game._Scripts.Lvl;
using _Game._Scripts.Lvl.Elements;
using UnityEngine;
using VContainer;

namespace _Game._Scripts.Save
{
    public sealed class SaveTrigger : MonoBehaviour
    {
        private LevelFlowController _flow;
        private GridController _grid;

        private void OnApplicationPause(bool pause)
        {
            if (!pause)
            {
                return;
            }

            TrySave();
        }

        private void OnApplicationQuit() => TrySave();

        [Inject]
        public void Construct(GridController grid, LevelFlowController flow)
        {
            _grid = grid;
            _flow = flow;
        }

        private void TrySave()
        {
            if (_grid == null || _flow == null)
            {
                return;
            }
            
            if (_grid.IsBusy)
            {
                return;
            }

            _flow.SaveGame(true);
        }
    }
}