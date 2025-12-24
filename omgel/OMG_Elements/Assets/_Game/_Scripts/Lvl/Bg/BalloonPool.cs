using _Common.Pool;
using _Game._Scripts.Settings;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;

namespace _Game._Scripts.Lvl.Bg
{
    public sealed class BalloonPool : MonoBehaviour, IPool<Balloon>
    {
        [SerializeField] private Transform _container;

        [Inject] private GameSettings _gameSettings;
        private ObjectPool<Balloon> _pool;

        private void Awake()
        {
            var maxBalloonsCount = _gameSettings.BalloonsSettings.MaxCount;
           
            _pool = new ObjectPool<Balloon>(
                OnBalloonCreate,
                OnBalloonGet,
                OnBalloonRelease,
                OnBalloonDestroy,
                false,
                defaultCapacity: maxBalloonsCount,
                maxSize: maxBalloonsCount
            );
        }

        private Balloon OnBalloonCreate() =>
            Instantiate(_gameSettings.BalloonsSettings.GetRandomPrefab(),
                _container);

        private void OnBalloonGet(Balloon balloon) => balloon.gameObject.SetActive(true);

        private void OnBalloonRelease(Balloon balloon) => balloon.OnDespawned();

        private void OnBalloonDestroy(Balloon balloon) => Destroy(balloon.gameObject);

        public Balloon Get() => _pool.Get();
        public void Release(Balloon balloon) => _pool.Release(balloon);
    }
}