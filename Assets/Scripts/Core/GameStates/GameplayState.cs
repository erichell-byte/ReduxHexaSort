using System.Threading;
using Core.Managers;
using Cysharp.Threading.Tasks;
using Presentation.UI;
using UnityEngine.SceneManagement;
using Zenject;

namespace Core.GameStates
{
    public class GameplayState : SceneState
    {
        private readonly SceneContextRegistry _sceneContextRegistry;
        private readonly IGameFlowService _gameFlowService;
        private readonly LevelManager _levelManager;

        private GameOverPresenter _gameOverPresenter;

        public GameplayState(SceneContextRegistry sceneContextRegistry,
            IGameFlowService gameFlowService,
            LevelManager levelManager)
        {
            _sceneContextRegistry = sceneContextRegistry;
            _gameFlowService = gameFlowService;
            _levelManager = levelManager;
        }

        protected override string SceneName => "Game";

        public override UniTask EnterAsync(CancellationToken token) => base.EnterAsync(token);

        public override UniTask ExitAsync(CancellationToken token)
        {
            CleanupUiBindings();
            return base.ExitAsync(token);
        }

        protected override UniTask OnSceneLoaded(CancellationToken token)
        {
            CleanupUiBindings();

            var container = TryGetSceneContainer();
            if (container == null)
            {
                return UniTask.CompletedTask;
            }

            _levelManager.BeginLevel();

            _gameOverPresenter = container.Resolve<GameOverPresenter>();
            _gameOverPresenter.RestartRequested += OnRestartRequested;
            _gameOverPresenter.NextLevelRequested += OnNextLevelRequested;
            _gameOverPresenter.MainMenuRequested += OnMainMenuRequested;

            return UniTask.CompletedTask;
        }

        private void OnRestartRequested()
        {
            _levelManager.RestartCurrentLevel();
            _gameFlowService.EnterGameplayAsync().Forget();
        }

        private void OnNextLevelRequested()
        {
            if (_levelManager.HasNextLevel())
            {
                _levelManager.NextLevel();
            }

            _gameFlowService.EnterGameplayAsync().Forget();
        }

        private void OnMainMenuRequested()
        {
            _gameFlowService.EnterMenuAsync().Forget();
        }

        private void CleanupUiBindings()
        {
            if (_gameOverPresenter == null)
            {
                return;
            }

            _gameOverPresenter.RestartRequested -= OnRestartRequested;
            _gameOverPresenter.NextLevelRequested -= OnNextLevelRequested;
            _gameOverPresenter.MainMenuRequested -= OnMainMenuRequested;
            _gameOverPresenter = null;
        }

        private DiContainer TryGetSceneContainer()
        {
            var scene = SceneManager.GetSceneByName(SceneName);
            if (!scene.IsValid())
            {
                return null;
            }

            return _sceneContextRegistry.TryGetContainerForScene(scene);
        }
    }
}
