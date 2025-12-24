using _Game._Scripts.Lvl.Elements;
using _Game._Scripts.Model.Enums;
using _Game._Scripts.Utils;
using UnityEngine;

namespace _Game._Scripts.Settings
{
    [CreateAssetMenu(
        fileName = nameof(ElementSettings),
        menuName = AppConstants.Paths.ScriptableObjects + nameof(ElementSettings))]
    public class ElementSettings : ScriptableObject
    {
        public ElementType Type;
        public Element Prefab;
    }
}