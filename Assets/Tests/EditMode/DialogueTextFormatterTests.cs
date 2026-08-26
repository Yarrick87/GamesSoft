using GamesSoft.MagicWords;
using NUnit.Framework;

namespace GamesSoft.Tests.EditMode
{
    public class DialogueTextFormatterTests
    {
        [Test]
        public void Format_NullOrEmpty_ReturnsEmpty()
        {
            Assert.That(DialogueTextFormatter.Format(null), Is.EqualTo(string.Empty));
            Assert.That(DialogueTextFormatter.Format(string.Empty), Is.EqualTo(string.Empty));
        }

        [Test]
        public void Format_KnownEmojiToken_ReplacesWithUnicode()
        {
            var result = DialogueTextFormatter.Format("Hello {satisfied}");
            Assert.That(result, Does.Contain("\U0001F60A"));
            Assert.That(result, Does.Not.Contain("{satisfied}"));
        }

        [Test]
        public void Format_UnknownEmojiToken_UsesFallbackSmile()
        {
            var result = DialogueTextFormatter.Format("{unknown_token}");
            Assert.That(result, Is.EqualTo("\u263A"));
        }

        [Test]
        public void Format_ItalicMarkers_ConvertsToTmpTags()
        {
            var result = DialogueTextFormatter.Format("Say *hello* now");
            Assert.That(result, Is.EqualTo("Say <i>hello</i> now"));
        }

        [Test]
        public void Format_EmojiAndItalics_Together()
        {
            var result = DialogueTextFormatter.Format("*Nice* {laughing}");
            Assert.That(result, Is.EqualTo("<i>Nice</i> \U0001F602"));
        }
    }
}
