using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game._Scripts.Main
{
    public sealed class SceneLoader
    {
        public AsyncOperation LoadAdditive(string sceneName)
        {
            var op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            op.allowSceneActivation = true;
            return op;
        }

        public AsyncOperation Unload(string sceneName)
        {
            return SceneManager.UnloadSceneAsync(sceneName);
        }
    }
}