using _Game._Scripts.Save;
using _Game._Scripts.Settings;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Game._Scripts.Main
{
    public sealed class BootstrapLifetimeScope : LifetimeScope
    {
        [SerializeField] private ScenesConfig _scenesConfig;
        [SerializeField] private GameSettings _gameSettings;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_scenesConfig);
            builder.RegisterInstance(_gameSettings);
            
            builder.Register<SceneLoader>(Lifetime.Singleton);
            builder.Register<ISaveService, JsonSaveService>(Lifetime.Singleton);
            builder.RegisterEntryPoint<AppStartup>();
        }
    }
}

