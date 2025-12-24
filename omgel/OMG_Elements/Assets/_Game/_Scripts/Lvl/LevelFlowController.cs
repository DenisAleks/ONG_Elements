using _Game._Scripts.Lvl.Bg;
using _Game._Scripts.Lvl.Elements;
using _Game._Scripts.Model.Data.Save;
using _Game._Scripts.Save;
using _Game._Scripts.Utils;
using UnityEngine;

namespace _Game._Scripts.Lvl
{
    public sealed class LevelFlowController
    {
        private readonly LevelRepository _levels;
        private readonly GridController _grid;
        private readonly BalloonController _balloons;
        private readonly ISaveService _save;
        private readonly GameEventBus _eventBus;

        private int _currentIndex;

        public LevelFlowController(LevelRepository levels, GridController grid, 
            BalloonController balloons, ISaveService saveService, GameEventBus eventBus)
        {
            _levels = levels;
            _grid = grid;
            _balloons = balloons;
            _save = saveService;
            _eventBus = eventBus;

            _eventBus.GridBecomeIdle += HandleGridBecomeIdle;
            _eventBus.LevelPassed += HandleLevelPassed;
        }

        public void Deinit()
        {
            _eventBus.GridBecomeIdle -= HandleGridBecomeIdle;
            _eventBus.LevelPassed -= HandleLevelPassed;
        }

        public void StartGame()
        {
            EnsureLoaded();

            if (_save.TryLoad(out var save))
            {
                LoadFromSave(save);
            }
            else
            {
                StartFirst();
            }
        }
        
        public void SaveGame(bool includeLevelProgress)
        {
            var data = new GameSaveData
            {
                LevelId = _levels.Levels[_currentIndex].Id,
                Grid = includeLevelProgress ? _grid.CreateSaveData() : null
            };

            _save.Save(data);
        }
        
        public void RestartCurrentLevel()
        {
            SaveGame(false);
            
            Debug.Log($"Restart level {_currentIndex}");
            LoadCurrent();
        }

        public void PlayNextLevel()
        { 
            SaveGame(false);
            
            _currentIndex = (_currentIndex + 1) % _levels.Levels.Count;
            Debug.Log($"Start Next level: {_currentIndex}");
            LoadCurrent();
        }
        
        private void LoadFromSave(GameSaveData save)
        {
            _currentIndex = save.LevelId - 1;
            
            var gridSave = save.Grid;

            if (gridSave != null && gridSave.Columns > 0 && gridSave.Rows > 0)
            {
                var level = _levels.Levels[_currentIndex];
                
                _grid.Dispose();
                _grid.RestoreFromSave(level, save.Grid);

                if (!_balloons.IsActive)
                {
                    _balloons.Dispose();
                    _balloons.Init();
                }
            }
            else
            {
                PlayNextLevel();
            }
        }

        private void StartFirst()
        {
            EnsureLoaded();
            _currentIndex = 0;
            LoadCurrent();
        }
        
        private void HandleGridBecomeIdle()
        {
            if (_grid.IsBusy)
            {
                return;
            }
            
            // Debug.Log($"Grid is idle -> save progress for level: {_currentIndex}");
            SaveGame(true);
        }
        
        private void HandleLevelPassed() => PlayNextLevel();

        private void LoadCurrent()
        {
            var level = _levels.Levels[_currentIndex];

            _grid.Dispose();
            _grid.Init(level);

            if (!_balloons.IsActive)
            {
                _balloons.Dispose();
                _balloons.Init();
            }
            
            SaveGame(false);
        }

        private void EnsureLoaded()
        {
            if (_levels.IsLoaded)
            {
                return;
            }

            Debug.LogError("Levels are not loaded yet");
        }
    }
}