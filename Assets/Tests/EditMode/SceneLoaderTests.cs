using GamesSoft.Core;
using NUnit.Framework;

namespace GamesSoft.Tests.EditMode
{
    public class SceneLoaderTests
    {
        [Test]
        public void SceneConstants_MatchBuildSceneNames()
        {
            Assert.That(SceneLoader.Menu, Is.EqualTo("MainMenu"));
            Assert.That(SceneLoader.AceOfShadows, Is.EqualTo("AceOfShadowsGame"));
            Assert.That(SceneLoader.MagicWords, Is.EqualTo("MagicWordsGame"));
            Assert.That(SceneLoader.PhoenixFlame, Is.EqualTo("PhoenixFlameGame"));
        }
    }
}
