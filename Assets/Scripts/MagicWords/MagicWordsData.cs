using System;
using System.Collections.Generic;
using UnityEngine;

namespace GamesSoft.MagicWords
{
    [Serializable]
    public class MagicWordsResponse
    {
        public DialogueLine[] dialogue;
        public AvatarEntry[] avatars;
    }

    [Serializable]
    public class DialogueLine
    {
        public string name;
        public string text;
    }

    [Serializable]
    public class AvatarEntry
    {
        public string name;
        public string url;
        public string position;
    }

    public class SpeakerProfile
    {
        public string Name;
        public bool AlignRight;
        public Sprite Avatar;
        public List<string> AvatarUrls { get; } = new List<string>();
    }

    public static class SpeakerAlignment
    {
        public static bool IsRight(string position)
        {
            return !string.IsNullOrEmpty(position) &&
                   position.Equals("right", StringComparison.OrdinalIgnoreCase);
        }
    }
}
