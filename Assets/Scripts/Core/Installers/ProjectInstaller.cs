using System.Collections.Generic;
using Core.GameStates;
using Core.Managers;
using Gameplay.Board;
using Infrastructure.Services;
using Services;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
	public class ProjectInstaller : MonoInstaller
	{
		[SerializeField] private Levels _levels;
		
		public override void InstallBindings()
		{
			InfrastructureInstaller.Install(Container);

			Container.Bind<SceneLoader>().AsSingle();
			Container.Bind<LevelManager>().AsSingle().WithArguments(_levels.LevelConfigs);
			Container.Bind<IGameFlowService>().To<GameFlowService>().AsSingle();
			
			Container.Bind<LoadingState>().AsTransient();
			Container.Bind<MenuState>().AsTransient();
			Container.Bind<GameplayState>().AsTransient();
			Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle();
			Container.Bind<List<GameState>>().FromMethod(context => new List<GameState>
			{
				context.Container.Resolve<LoadingState>(),
				context.Container.Resolve<MenuState>(),
				context.Container.Resolve<GameplayState>()
			}).WhenInjectedInto<GameStateMachine>();
		}
	}
}
