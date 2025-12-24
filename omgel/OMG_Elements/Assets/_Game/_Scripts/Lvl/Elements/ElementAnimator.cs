using System;
using System.Linq;
using _Game._Scripts.Settings;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace _Game._Scripts.Lvl.Elements
{
    public class ElementAnimator : MonoBehaviour
    {
        public const string AnimNameIdle = "Idle";
        public const string AnimNameDestroy = "Destroy";
        
        private static readonly int IdleStateHash = Animator.StringToHash(AnimNameIdle);
        private static readonly int DestroyStateHash = Animator.StringToHash(AnimNameDestroy);
        
        [SerializeField] private Animator _animator;
        
        private void OnEnable() => PlayIdleRandomized();
        
        public void PlayIdle() => Play(IdleStateHash);
        public void PlayDestroy() => Play(DestroyStateHash);

        public void Init(GameSettings settings)
        {
            _animator.speed = settings.GridSettings.ElementAnimationTimeScale;
        }
        
        public float GetAnimationDuration(string animationName)
        {
            var info = _animator.runtimeAnimatorController.animationClips
                .FirstOrDefault(c => c.name == animationName);

            return info.length / _animator.speed;
        }
        
        private void PlayIdleRandomized()
        {
            Play(IdleStateHash, Random.value);
            _animator.Update(0f);
        }

        private void Play(int hash, float time = 0f) => _animator.Play(hash, 0, time);
    }
}