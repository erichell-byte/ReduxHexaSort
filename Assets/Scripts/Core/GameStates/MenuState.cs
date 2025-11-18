using System.Threading;
using Cysharp.Threading.Tasks;
using Presentation.UI;
using UnityEngine.SceneManagement;
using Zenject;

namespace Core.GameStates
{
    public class MenuState : SceneState
    {
        private readonly SceneContextRegistry _sceneContextRegistry;
        private readonly IGameFlowService _gameFlowService;

        private MenuPresenter _menuPresenter;

        public MenuState(SceneContextRegistry sceneContextRegistry, IGameFlowService gameFlowService)
        {
            _sceneContextRegistry = sceneContextRegistry;
            _gameFlowService = gameFlowService;
        }

        protected override string SceneName => "Menu";

        public override async UniTask EnterAsync(CancellationToken token)
        {
            if (!IsSceneLoaded())
            {
                await base.EnterAsync(token);
            }
            else
            {
                await OnSceneLoaded(token);
            }
        }

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

            _menuPresenter = container.Resolve<MenuPresenter>();
            _menuPresenter.PlayRequested += OnPlayRequested;
            _menuPresenter.ClearProgressRequested += OnClearProgressRequested;

            return UniTask.CompletedTask;
        }

        private void CleanupUiBindings()
        {
            if (_menuPresenter == null)
            {
                return;
            }

            _menuPresenter.PlayRequested -= OnPlayRequested;
            _menuPresenter.ClearProgressRequested -= OnClearProgressRequested;
            _menuPresenter = null;
        }

        private void OnPlayRequested()
        {
            _gameFlowService.EnterGameplayAsync().Forget();
        }

        private void OnClearProgressRequested()
        {
            _gameFlowService.EnterMenuAsync().Forget();
        }

        private bool IsSceneLoaded()
        {
            var scene = SceneManager.GetSceneByName(SceneName);
            return scene.IsValid() && scene.isLoaded;
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
