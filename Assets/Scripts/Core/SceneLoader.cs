using System.Threading.Tasks;
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

        public static async Task Load(string sceneName)
        {
            await SceneManager.LoadSceneAsync(sceneName);
        }
    }
}
