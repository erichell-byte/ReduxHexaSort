using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Services
{
    public class SceneLoader
    {
        public async UniTask LoadSceneAsync(string sceneName, CancellationToken token)
        {
            var asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            asyncOperation.allowSceneActivation = false;

            while (!asyncOperation.isDone && !token.IsCancellationRequested)
            {
                if (asyncOperation.progress >= 0.9f)
                {
                    asyncOperation.allowSceneActivation = true;
                }
                await UniTask.Yield();
            }
        }

        public UniTask UnloadSceneAsync(string sceneName, CancellationToken token)
        {
            return SceneManager.UnloadSceneAsync(sceneName).ToUniTask(cancellationToken: token);
        }
    }
}