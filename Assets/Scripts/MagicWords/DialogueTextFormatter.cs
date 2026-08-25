using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GamesSoft.MagicWords
{
    public static class DialogueTextFormatter
    {
        private static readonly Regex TokenRegex = new Regex(@"\{([^}]+)\}", RegexOptions.Compiled);
        private static readonly Regex ItalicRegex = new Regex(@"\*([^*]+)\*", RegexOptions.Compiled);

        private static readonly Dictionary<string, string> UnicodeEmojis = new Dictionary<string, string>
        {
            { "satisfied", "\U0001F60A" },
            { "intrigued", "\U0001F609" },
            { "neutral", "\u2639" },
            { "affirmative", "\U0001F600" },
            { "laughing", "\U0001F602" },
            { "win", "\U0001F604" }
        };

        public static string Format(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            var textWithEmojis = TokenRegex.Replace(text, match =>
            {
                var token = match.Groups[1].Value.Trim();

                if (UnicodeEmojis.TryGetValue(token, out string emoji))
                {
                    return emoji;
                }

                return "\u263A";
            });

            return ItalicRegex.Replace(textWithEmojis, "<i>$1</i>");
        }
    }
}
