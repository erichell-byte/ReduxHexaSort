using Cysharp.Threading.Tasks;
using System.Threading;
using Zenject;
using Services;

namespace Core.GameStates
{
    public abstract class SceneState : GameState
    {
        [Inject] private SceneLoader _sceneLoader;

        protected abstract string SceneName { get; }

        public override async UniTask EnterAsync(CancellationToken token)
        {
            await _sceneLoader.LoadSceneAsync(SceneName, token);
            await OnSceneLoaded(token);
        }
        
        protected virtual UniTask OnSceneLoaded(CancellationToken token)
        {
            return UniTask.CompletedTask;
        }

        public override UniTask ExitAsync(CancellationToken token)
        {
            return UniTask.CompletedTask;
        }
    }
}
