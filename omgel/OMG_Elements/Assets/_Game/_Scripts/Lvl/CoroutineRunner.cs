using UnityEngine;

namespace _Game._Scripts.Lvl
{
    /// <summary>
    /// Used in non-monobehaviour scripts to run coroutines
    /// </summary>
    public sealed class CoroutineRunner : MonoBehaviour
    {
        private static CoroutineRunner _instance;

        public static CoroutineRunner Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                var go = new GameObject(nameof(CoroutineRunner));
                DontDestroyOnLoad(go);
                _instance = go.AddComponent<CoroutineRunner>();
                return _instance;
            }
        }
    }
}