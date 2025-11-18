using Core.Interfaces;
using Core.Managers;
using Gameplay.Board;
using Gameplay.Towers;
using Hexa.Domain.Boosters;
using Hexa.Domain.Player;
using Hexa.Domain.Rules;
using Hexa.Domain.State;
using Infrastructure.Services;
using Presentation.UI;
using Services;
using UI.Models;
using UI.Views;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private PlayerTower _playerTowerPrefab;
        
        public override void InstallBindings()
        {
            Container.Bind<LevelConfig>().FromMethod(GetCurrentLevelConfig).AsTransient();
            Container.Bind<IGameplayRuleEvaluator>().To<GameplayRuleService>().AsSingle();
            Container.Bind<IGameResultService>().To<GameResultService>().AsSingle();
            Container.Bind<IBoardGeneratorService>().To<BoardGeneratorService>().AsSingle();
            Container.Bind<ITowerPlacementService>().To<TowerPlacementService>().AsSingle();
            Container.Bind<IChainReactionService>().To<ChainReactionService>().AsSingle();
            Container.Bind<IAnimationBridge>().To<AnimationBridge>().AsSingle();
            Container.Bind<IBoosterCommand>().To<DestroyStackCommand>().AsSingle();
            Container.Bind<IBoosterCommand>().To<UndoMoveCommand>().AsSingle();
            Container.Bind<IBoosterCommand>().To<ShuffleBoardCommand>().AsSingle();
            Container.Bind<IBoosterCatalog>().To<BoosterCatalog>().AsSingle();
            Container.Bind<IBoosterExecutor>().To<BoosterExecutor>().AsSingle();
            Container.BindInterfacesAndSelfTo<ScoreStorage>().AsSingle();
            Container.Bind<IUndoHistoryService>().To<UndoHistoryService>().AsSingle();
            Container.Bind<BoostersModel>().AsSingle();
            Container.Bind<GameOverModel>().AsSingle();
            Container.Bind<BoosterPresenter>().AsSingle();
            Container.Bind<HUDPresenter>().AsSingle();
            Container.Bind<GameOverPresenter>().AsSingle();
            Container.Bind<IPlayerDeckService>().To<PlayerDeckService>().AsSingle();
            Container.Bind<BoardController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IBoardBoosterContext>().FromResolveGetter<BoardController>(controller => controller);
            Container.Bind<TowerDragService>().AsSingle().NonLazy();
            Container.Bind<GUIView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<BoostersView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<GameOverView>().FromComponentInHierarchy().AsSingle();
            Container.BindFactory<PlayerTower, PlayerTowerFactory>().FromComponentInNewPrefab(_playerTowerPrefab).AsSingle();
        }
        
        private LevelConfig GetCurrentLevelConfig(InjectContext context)
        {
            var levelManager = context.Container.TryResolve<LevelManager>();
            return levelManager?.CurrentLevel;
        }
    }
    
    public class PlayerTowerFactory : PlaceholderFactory<PlayerTower> { }
}
