using System;
using UnityEngine;

namespace _Game._Scripts.Model.Enums
{
    public enum ElementType
    {
        None = 0,
        Fire = 1,
        Water = 2,
    }
    
    public static class ElementTypeExtensions {

        public static Color ToDebugColor(this ElementType type)
        {
            switch (type)
            {
                case ElementType.None:
                    return Color.gray9;
                case ElementType.Fire:
                    return Color.coral;
                case ElementType.Water:
                    return Color.cornflowerBlue;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}