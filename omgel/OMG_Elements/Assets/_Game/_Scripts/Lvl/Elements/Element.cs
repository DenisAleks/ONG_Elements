using System;
using _Game._Scripts.Model.Enums;
using _Game._Scripts.Settings;
using DG.Tweening;
using UnityEngine;

namespace _Game._Scripts.Lvl.Elements
{
    public sealed class Element : MonoBehaviour
    {
        public ElementType Type => _type;
        

        [SerializeField] private ElementType _type;
        [SerializeField] private ElementSorting _sorting;
        [SerializeField] private ElementAnimator _animator;
        [SerializeField] private SpriteRenderer _skin;

        private Tween _destroyDelayTween;
        
        public void Init(GameSettings settings, float scale)
        {
            _animator.Init(settings);
            
            transform.localScale = Vector3.one * scale;
            gameObject.name = $"El_{_type}";
        }

        public void UpdateSortingOrder(Cell toCell) => _sorting.UpdateSortingOrder(toCell);
        public void OnDespawned()
        {
            transform.DOKill();
            _destroyDelayTween?.Kill();
        }
        
        public void Blast(Action onComplete)
        {
            var duration = _animator.GetAnimationDuration(ElementAnimator.AnimNameDestroy);
            
            _animator.PlayDestroy();
            
            _destroyDelayTween = DOVirtual.DelayedCall(duration, () => onComplete?.Invoke());
        }
    }
}