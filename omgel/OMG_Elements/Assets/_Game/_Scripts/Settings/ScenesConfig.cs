using _Game._Scripts.Utils;
using UnityEngine;

namespace _Game._Scripts.Settings
{
    [CreateAssetMenu(fileName = nameof(ScenesConfig),
        menuName = AppConstants.Paths.ScriptableObjects + nameof(ScenesConfig))]
    public sealed class ScenesConfig : ScriptableObject
    {
        [SerializeField] private string _gameScene;

        public string GameScene => _gameScene;
    }
}