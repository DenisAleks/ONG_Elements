using System;
using _Game._Scripts.Lvl.Elements;
using VContainer.Unity;

namespace _Game._Scripts.Lvl
{
    public sealed class GameController : IStartable, ITickable, IDisposable
    {
        private readonly LevelRepository _levels;
        private readonly LevelFlowController _flow;
        private readonly GridController _grid;

        public GameController(LevelRepository levels, LevelFlowController flow, GridController grid)
        {
            _levels = levels;
            _flow = flow;
            _grid = grid;
        }

        public void Start()
        {
            _levels.LoadAll(() =>
            {
                _flow.StartGame();
            });
        }
        
        public void Tick() => _grid.Tick();

        public void Dispose() => _flow.Deinit();
    }

}