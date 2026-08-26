using GamesSoft.MagicWords;
using NUnit.Framework;
using UnityEngine;

namespace GamesSoft.Tests.EditMode
{
    public class MagicWordsDataTests
    {
        [Test]
        public void SpeakerAlignment_IsRight_OnlyForRightIgnoreCase()
        {
            Assert.That(SpeakerAlignment.IsRight("right"), Is.True);
            Assert.That(SpeakerAlignment.IsRight("RIGHT"), Is.True);
            Assert.That(SpeakerAlignment.IsRight("left"), Is.False);
            Assert.That(SpeakerAlignment.IsRight(null), Is.False);
            Assert.That(SpeakerAlignment.IsRight(string.Empty), Is.False);
        }

        [Test]
        public void JsonUtility_ParsesDialogueAndAvatars()
        {
            const string json = @"{
                ""dialogue"": [
                    { ""name"": ""Alice"", ""text"": ""Hi {win}"" },
                    { ""name"": ""Bob"", ""text"": ""*Hello*"" }
                ],
                ""avatars"": [
                    { ""name"": ""Alice"", ""url"": ""https://example.com/a.png"", ""position"": ""left"" },
                    { ""name"": ""Bob"", ""url"": ""https://example.com/b.png"", ""position"": ""right"" }
                ]
            }";

            var data = JsonUtility.FromJson<MagicWordsResponse>(json);

            Assert.That(data, Is.Not.Null);
            Assert.That(data.dialogue, Has.Length.EqualTo(2));
            Assert.That(data.dialogue[0].name, Is.EqualTo("Alice"));
            Assert.That(data.dialogue[1].text, Is.EqualTo("*Hello*"));
            Assert.That(data.avatars, Has.Length.EqualTo(2));
            Assert.That(data.avatars[1].position, Is.EqualTo("right"));
            Assert.That(SpeakerAlignment.IsRight(data.avatars[1].position), Is.True);
        }

        [Test]
        public void JsonUtility_MissingArrays_AreNull()
        {
            var data = JsonUtility.FromJson<MagicWordsResponse>("{}");
            Assert.That(data.dialogue, Is.Null);
            Assert.That(data.avatars, Is.Null);
        }
    }
}
