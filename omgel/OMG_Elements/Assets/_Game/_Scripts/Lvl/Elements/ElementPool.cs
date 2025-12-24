using System.Collections.Generic;
using _Game._Scripts.Model.Enums;
using _Game._Scripts.Settings;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;

namespace _Game._Scripts.Lvl.Elements
{
    public sealed class ElementPool : MonoBehaviour
    {
        [SerializeField] private Transform _container;

        [Inject] private GameSettings _gameSettings;

        private Dictionary<ElementType, ObjectPool<Element>> _pools;

        public void Awake()
        {
            _pools = new Dictionary<ElementType, ObjectPool<Element>>();

            foreach (var config in _gameSettings.GridSettings.ElementSettingsArray)
            {
                CreatePoolFor(config, _gameSettings.GridSettings.MaxElementsCount);
            }
        }

        private void CreatePoolFor(ElementSettings settings, int capacity)
        {
            var pool = new ObjectPool<Element>(
                () =>
                {
                    var e = Instantiate(settings.Prefab, _container);
                    return e;
                },
                e =>
                {
                    e.gameObject.SetActive(true);
                },
                e =>
                {
                    e.OnDespawned();
                    e.gameObject.SetActive(false);
                },
                e => Destroy(e.gameObject),
                false,
                capacity,
                capacity
            );

            _pools.Add(settings.Type, pool);
        }

        public Element Get(ElementType type)
        {
            if (!_pools.TryGetValue(type, out var pool))
            {
                Debug.LogError($"No pool for element type {type}");
                return null;
            }

            return pool.Get();
        }

        public void Release(Element element) => _pools[element.Type].Release(element);
    }
}