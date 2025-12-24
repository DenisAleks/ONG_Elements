using System;

namespace _Game._Scripts.Utils
{
    public sealed class GameEventBus
    {
        public event Action GridBecomeIdle;
        public event Action LevelPassed;
        public void RaiseGridBecomeIdle() => GridBecomeIdle?.Invoke();
        public void RaiseLevelPassed() => LevelPassed?.Invoke();
    }
}