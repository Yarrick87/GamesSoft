using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GamesSoft.MagicWords
{
    public class ChatMessageView : MonoBehaviour
    {
        private readonly float MaxBubbleWidth = 680f;
        private readonly  float MinBubbleWidth = 80f;

        private readonly Color AvatarFrame = new Color(0.16f, 0.17f, 0.24f, 1f);

        [SerializeField]
        private RectTransform _row;

        [SerializeField]
        private Image _avatarBackground;

        [SerializeField]
        private Image _avatarImage;

        [SerializeField]
        private TMP_Text _initialsText;

        [SerializeField]
        private TMP_Text _nameText;

        [SerializeField]
        private TMP_Text _messageText;

        [SerializeField]
        private LayoutElement _messageLayout;

        [SerializeField]
        private LayoutElement _rootLayout;

        public void Bind(SpeakerProfile speaker, string message)
        {
            _nameText.text = string.IsNullOrEmpty(speaker.Name) ? "Unknown" : speaker.Name;
            _messageText.text = message;
            _messageText.ForceMeshUpdate();

            var preferred = _messageText.GetPreferredValues(message, MaxBubbleWidth, float.PositiveInfinity);
            _messageLayout.preferredWidth = Mathf.Clamp(preferred.x + 8f, MinBubbleWidth, MaxBubbleWidth);
            _messageLayout.preferredHeight = Mathf.Max(preferred.y + 4f, 36f);

            SetPlaceholder(speaker.Name);

            if (speaker.Avatar != null)
            {
                SetAvatar(speaker.Avatar);
            }

            RebuildSize();
        }

        private void SetAvatar(Sprite sprite)
        {
            _avatarImage.sprite = sprite;
            _avatarImage.color = Color.white;
            _avatarImage.enabled = true;
            _initialsText.gameObject.SetActive(false);
        }

        private void SetPlaceholder(string speakerName)
        {
            _avatarBackground.color = ColorFromName(speakerName);
            _avatarImage.enabled = false;
            _initialsText.text = Initials(speakerName);
            _initialsText.gameObject.SetActive(true);
        }

        private void RebuildSize()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_row);
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);
            _rootLayout.preferredHeight = Mathf.Max(_row.rect.height, 72f);
        }

        private static string Initials(string speakerName)
        {
            if (string.IsNullOrWhiteSpace(speakerName))
            {
                return "?";
            }

            return speakerName.Trim().Substring(0, 1).ToUpperInvariant();
        }

        private Color ColorFromName(string speakerName)
        {
            if (string.IsNullOrEmpty(speakerName))
            {
                return AvatarFrame;
            }

            var hash = speakerName.GetHashCode();
            var hue = Mathf.Abs(hash % 360) / 360f;
            
            return Color.HSVToRGB(hue, 0.28f, 0.42f);
        }
    }
}
