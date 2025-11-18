using Hexa.Domain.State;
using Infrastructure.Services;
using UI.Models;
using Zenject;

namespace Core.Installers
{
    public class InfrastructureInstaller : Installer<InfrastructureInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IAnimationBridge>().To<AnimationBridge>().AsSingle();
            Container.Bind<ISaveGameService>().To<SaveGameService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ScoreStorage>().AsSingle();
            Container.Bind<IUndoHistoryService>().To<UndoHistoryService>().AsSingle();
            Container.BindInterfacesAndSelfTo<TowerInputService>()
                .FromNewComponentOnNewGameObject()
                .WithGameObjectName("TowerInputService")
                .AsSingle()
                .NonLazy();
        }
    }
}
