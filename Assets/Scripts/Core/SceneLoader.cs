using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GamesSoft.Core
{
    public static class SceneLoader
    {
        public const string Menu = "MainMenu";
        public const string AceOfShadows = "AceOfShadowsGame";
        public const string MagicWords = "MagicWordsGame";
        public const string PhoenixFlame = "PhoenixFlameGame";

        private static bool _isLoading;

        public static async Awaitable Load(string sceneName, CancellationToken cancellationToken = default)
        {
            if (_isLoading)
            {
                return;
            }

            cancellationToken.ThrowIfCancellationRequested();

            _isLoading = true;

            try
            {
                var operation = SceneManager.LoadSceneAsync(sceneName);

                while (!operation.isDone)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await Awaitable.NextFrameAsync(cancellationToken);
                }
            }
            finally
            {
                _isLoading = false;
            }
        }
    }
}
