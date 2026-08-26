using System;
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

            var operation = SceneManager.LoadSceneAsync(sceneName);
            
            if (operation == null)
            {
                throw new InvalidOperationException($"Scene '{sceneName}' could not be loaded.");
            }

            _isLoading = true;

            try
            {
                while (!operation.isDone)
                {
                    await Awaitable.NextFrameAsync();
                }
            }
            finally
            {
                _isLoading = false;
            }
        }
    }
}
