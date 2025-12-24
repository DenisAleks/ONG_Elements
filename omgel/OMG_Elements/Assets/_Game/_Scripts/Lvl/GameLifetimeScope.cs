using _Common.Pool;
using _Game._Scripts.Lvl.Bg;
using _Game._Scripts.Lvl.Elements;
using _Game._Scripts.Lvl.Interaction;
using _Game._Scripts.Lvl.UI;
using _Game._Scripts.Utils;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Game._Scripts.Lvl
{
    public sealed class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private BalloonPool _balloonPool;
        [SerializeField] private CellPool _cellPool;
        [SerializeField] private ElementPool _elementPool;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<GameEventBus>(Lifetime.Scoped);
            
            builder.RegisterComponent(_balloonPool).AsImplementedInterfaces();
            builder.RegisterComponent(_cellPool).AsSelf();
            builder.RegisterComponent(_elementPool).AsSelf();
            
            builder.Register<BalloonController>(Lifetime.Scoped);
            builder.Register<LevelRepository>(Lifetime.Scoped);
            builder.Register<GridController>(Lifetime.Scoped);
            builder.Register<LevelFlowController>(Lifetime.Scoped);
            
            builder.Register<IGridInteractionService, GridInteractionService>(Lifetime.Scoped);
          
            builder.RegisterComponentInHierarchy<GridInputReader>();
            builder.RegisterComponentInHierarchy<LevelButtonsPanel>();
          
            builder.RegisterEntryPoint<GameController>();
        }
    }
}