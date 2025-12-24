using System;
using _Common.Utils;
using _Game._Scripts.Settings;
using DG.Tweening;
using UnityEngine;

namespace _Game._Scripts.Lvl.Bg
{
    public sealed class Balloon : MonoBehaviour
    {
        private Tween _moveXTween;
        private Action _onComplete;
        private BalloonsSettings _settings;
        private Tween _moveYTween;

        public void Init(BalloonsSettings settings, Vector2 startPos, Vector2 targetPos, Action onComplete)
        {
            _settings = settings;
            _onComplete = onComplete;

            transform.position = new Vector3(startPos.x, startPos.y, transform.position.z);

            var speed = RandomUtil.GetRandomFloat(_settings.SpeedXRange);
            var frequency = RandomUtil.GetRandomFloat(_settings.FrequencyRange);
            var amplitude = RandomUtil.GetRandomFloat(_settings.AmplitudeRange);

            var distance = Mathf.Abs(targetPos.x - startPos.x);
            var duration = distance / speed;

            _moveXTween = transform
                .DOMoveX(targetPos.x, duration)
                .SetEase(Ease.Linear)
                .OnComplete(Complete);

            _moveYTween = transform
                .DOMoveY(transform.position.y + amplitude, 1f / frequency)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        public void OnDespawned()
        {
            _moveXTween?.Kill();
            _moveYTween?.Kill();
            _onComplete = null;
        }

        private void Complete()
        {
            _moveXTween?.Kill();
            _moveYTween?.Kill();
            _onComplete?.Invoke();
        }
    }
}