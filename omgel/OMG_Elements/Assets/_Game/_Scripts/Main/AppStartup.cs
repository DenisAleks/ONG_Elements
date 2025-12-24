using _Game._Scripts.Settings;
using VContainer.Unity;

namespace _Game._Scripts.Main
{
    public sealed class AppStartup : IStartable
    {
        private readonly SceneLoader _sceneLoader;
        private readonly ScenesConfig _sceneConfig;

        public AppStartup(SceneLoader sceneLoader, ScenesConfig scenesConfig)
        {
            _sceneLoader = sceneLoader;
            _sceneConfig = scenesConfig;
        }

        public void Start()
        {
            var op = _sceneLoader.LoadAdditive(_sceneConfig.GameScene);
        }
    }
}