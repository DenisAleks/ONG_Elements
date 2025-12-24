using System;
using System.Collections.Generic;
using _Common;
using _Common.Pool;
using _Common.Utils;
using _Game._Scripts.Settings;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game._Scripts.Lvl.Bg
{
    public sealed class BalloonController : IDisposable
    {
        public bool IsActive => _isActive;
        
        private readonly List<Balloon> _activeBalloons = new();

        private readonly IPool<Balloon> _balloonPool;
        private readonly GameSettings _gameSettings;
        private readonly Camera _mainCamera;

        private Tween _delayTween;
        private bool _isActive;

        public BalloonController(IPool<Balloon> balloonPool, GameSettings gameSettings)
        {
            _balloonPool = balloonPool;
            _gameSettings = gameSettings;
            _mainCamera = Camera.main;
        }

        public void Init()
        {
            AimSpawnNextBalloon();
            _isActive = true;
        }

        public void Dispose()
        {
            KillDelayTween();

            foreach (var balloon in _activeBalloons)
            {
                _balloonPool.Release(balloon);
            }

            _activeBalloons.Clear();
            _isActive = false;
        }

        private void AimSpawnNextBalloon()
        {
            var settings = _gameSettings.BalloonsSettings;

            if (_activeBalloons.Count < settings.MaxCount &&
                Random.value < settings.SpawnProbability)
            {
                SpawnBalloon(settings);
            }

            ScheduleNextSpawnAttempt();
        }

        private void SpawnBalloon(BalloonsSettings settings)
        {
            var balloon = _balloonPool.Get();
            _activeBalloons.Add(balloon);

            var startPos = DefineBalloonStartPosition(settings);
            var targetPos = DefineBalloonTargetPosition(startPos);

            balloon.Init(settings, startPos, targetPos, () =>
            {
                _activeBalloons.Remove(balloon);
                _balloonPool.Release(balloon);
            });
        }

        private void ScheduleNextSpawnAttempt()
        {
            KillDelayTween();

            var delayRange = _gameSettings.BalloonsSettings.SpawnAttemptDelayRangeSeconds;
            var delay = RandomUtil.GetRandomFloat(delayRange);
            _delayTween = DOVirtual.DelayedCall(delay, AimSpawnNextBalloon);
        }

        private void KillDelayTween()
        {
            _delayTween?.Kill();
            _delayTween = null;
        }

        private Vector2 DefineBalloonStartPosition(BalloonsSettings settings)
        {
            var xMin = CamViewportToWorld(Vector3.zero).x - settings.SpawnOffsetX;
            var xMax = CamViewportToWorld(Vector3.one).x + settings.SpawnOffsetX;

            var x = RandomUtil.Either(xMin, xMax);
            var y = RandomUtil.GetRandomFloat(settings.BalloonsYRange);

            return new Vector2(x, y);
        }

        private Vector2 DefineBalloonTargetPosition(Vector2 startPos)
        {
            var settings = _gameSettings.BalloonsSettings;

            return startPos.x < 0f
                ? new Vector2(CamViewportToWorld(Vector3.one).x + settings.SpawnOffsetX, startPos.y)
                : new Vector2(CamViewportToWorld(Vector3.zero).x - settings.SpawnOffsetX, startPos.y);
        }

        private Vector2 CamViewportToWorld(Vector3 pos) => _mainCamera.ViewportToWorldPoint(pos);
    }
}