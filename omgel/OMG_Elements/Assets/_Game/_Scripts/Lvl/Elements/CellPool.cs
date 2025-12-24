using _Common.Pool;
using _Game._Scripts.Settings;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;

namespace _Game._Scripts.Lvl.Elements
{
    public class CellPool : MonoBehaviour, IPool<Cell>
    {
        [SerializeField] private Transform _container;

        [Inject] private GameSettings _gameSettings;
        private ObjectPool<Cell> _pool;

        private void Awake()
        {
            var maxCount = _gameSettings.GridSettings.MaxElementsCount;

            _pool = new ObjectPool<Cell>(
                OnCreate,
                OnGet,
                OnRelease,
                OnCellDestroy,
                false,
                defaultCapacity: maxCount,
                maxSize: maxCount
            );
        }

        private Cell OnCreate() => Instantiate(_gameSettings.GridSettings.CellPrefab, _container);

        private void OnGet(Cell cell)
        {
            cell.gameObject.SetActive(true);
            cell.SetState(CellState.Idle);
        }

        private void OnRelease(Cell cell)
        {
            cell.OnDespawned();
            cell.gameObject.SetActive(false);
        }

        private void OnCellDestroy(Cell cell) => Destroy(cell.gameObject);

        public Cell Get() => _pool.Get();
        public void Release(Cell cell) => _pool.Release(cell);
    }
}