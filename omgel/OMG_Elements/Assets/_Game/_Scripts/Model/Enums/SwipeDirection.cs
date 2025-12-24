using UnityEngine;

namespace _Game._Scripts.Model.Enums
{
    public enum SwipeDirection
    {
        Up,
        Down,
        Left,
        Right
    }
    
    public static class SwipeDirectionExtensions
    {
        public static readonly SwipeDirection[] All =
        {
            SwipeDirection.Up,
            SwipeDirection.Down,
            SwipeDirection.Left,
            SwipeDirection.Right
        };
        
        public static Vector2Int ToOffset(this SwipeDirection dir) =>
            dir switch
            {
                SwipeDirection.Up => Vector2Int.up,
                SwipeDirection.Down => Vector2Int.down,
                SwipeDirection.Left => Vector2Int.left,
                SwipeDirection.Right => Vector2Int.right,
                _ => Vector2Int.zero
            };
    }
}