using System.Linq;
using _Game._Scripts.Utils;
using UnityEngine;

namespace _Game._Scripts.Settings
{
    [CreateAssetMenu(fileName = nameof(GameSettings), 
        menuName = AppConstants.Paths.ScriptableObjects + nameof(GameSettings))]
    public class GameSettings: ScriptableObject
    {
        public BalloonsSettings BalloonsSettings => _balloonsSettings;
        public GridSettings GridSettings => _gridSettings;
        public bool AllowMultipleElementsInteraction => _allowMultyInteracion;
        
        [SerializeField] private BalloonsSettings _balloonsSettings;
        [SerializeField] private GridSettings _gridSettings;
        [SerializeField] private bool _allowMultyInteracion;
    }
}