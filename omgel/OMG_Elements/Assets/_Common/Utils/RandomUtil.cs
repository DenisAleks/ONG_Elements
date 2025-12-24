using UnityEngine;

namespace _Common.Utils
{
    public static class RandomUtil
    {
        public static float GetRandomFloat(Vector2 minMax) => GetRandomFloat(minMax.x, minMax.y);
        
        public static float GetRandomFloat(float min, float max) => Random.Range(min, max);
        
        public static bool Bool() => Random.Range(0, 2) == 1;
        
        public static T Either<T>(T first, T second) => Bool() ? first : second;
    }
}