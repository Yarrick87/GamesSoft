using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace GamesSoft.MagicWords
{
    public class MagicWordsController : MonoBehaviour
    {
        private const string DefaultEndpoint = "https://private-624120-softgamesassignment.apiary-mock.com/v3/magicwords";

        [SerializeField]
        private string _endpoint = DefaultEndpoint;

        [SerializeField]
        private ChatMessageView _leftMessagePrefab;

        [SerializeField]
        private ChatMessageView _rightMessagePrefab;

        [SerializeField]
        private RectTransform _content;

        [SerializeField]
        private ScrollRect _scrollRect;

        [SerializeField]
        private TMP_Text _statusText;

        readonly Dictionary<string, SpeakerProfile> _speakers = new Dictionary<string, SpeakerProfile>();
        readonly List<Texture2D> _loadedTextures = new List<Texture2D>();

        private async void Start()
        {
            ShowStatus("Loading...");

            try
            {
                var data = await FetchDialogue();
                BuildSpeakerProfiles(data);

                await LoadAvatars();

                HideStatus();
                ShowMessages(data);
            }
            catch (Exception exception)
            {
                Debug.LogError($"Magic Words failed to load: {exception}");
                ShowStatus("Failed to load dialogue");
            }
        }

        private void OnDestroy()
        {
            for (int i = 0; i < _loadedTextures.Count; i++)
            {
                if (_loadedTextures[i] != null)
                {
                    Destroy(_loadedTextures[i]);
                }
            }
        }

        private async Awaitable<MagicWordsResponse> FetchDialogue()
        {
            using var request = UnityWebRequest.Get(_endpoint);
            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                throw new Exception(request.error);
            }

            var data = JsonUtility.FromJson<MagicWordsResponse>(request.downloadHandler.text);

            if (data?.dialogue == null || data.dialogue.Length == 0)
            {
                throw new Exception("Dialogue data is missing");
            }

            return data;
        }

        private void BuildSpeakerProfiles(MagicWordsResponse data)
        {
            _speakers.Clear();

            if (data.avatars != null)
            {
                for (int i = 0; i < data.avatars.Length; i++)
                {
                    var entry = data.avatars[i];

                    if (string.IsNullOrWhiteSpace(entry?.name))
                    {
                        continue;
                    }

                    if (!_speakers.TryGetValue(entry.name, out SpeakerProfile profile))
                    {
                        profile = new SpeakerProfile
                        {
                            Name = entry.name,
                            AlignRight = IsRight(entry.position)
                        };
                        _speakers.Add(entry.name, profile);
                    }

                    if (!string.IsNullOrWhiteSpace(entry.url))
                    {
                        profile.AvatarUrls.Add(entry.url);
                    }
                }
            }

            for (int i = 0; i < data.dialogue.Length; i++)
            {
                var speakerName = data.dialogue[i]?.name;

                if (string.IsNullOrWhiteSpace(speakerName) || _speakers.ContainsKey(speakerName))
                {
                    continue;
                }

                _speakers.Add(speakerName, new SpeakerProfile
                {
                    Name = speakerName,
                    AlignRight = false
                });
            }
        }

        private async Awaitable LoadAvatars()
        {
            foreach (var pair in _speakers)
            {
                var sprite = await LoadFirstValidAvatar(pair.Value.AvatarUrls);

                if (sprite != null)
                {
                    pair.Value.Avatar = sprite;
                }
            }
        }

        private async Awaitable<Sprite> LoadFirstValidAvatar(List<string> urls)
        {
            if (urls.Count == 0)
            {
                return null;
            }

            foreach (string url in urls)
            {
                var sprite = await TryLoadSprite(url);

                if (sprite != null)
                {
                    return sprite;
                }
            }

            return null;
        }

        private async Awaitable<Sprite> TryLoadSprite(string url)
        {
            if (string.IsNullOrWhiteSpace(url) || !Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                return null;
            }

            try
            {
                using var request = UnityWebRequestTexture.GetTexture(url);
                await request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    return null;
                }

                var texture = DownloadHandlerTexture.GetContent(request);

                if (texture == null)
                {
                    return null;
                }

                _loadedTextures.Add(texture);

                return Sprite.Create(
                    texture,
                    new Rect(0f, 0f, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f),
                    100f);
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"Avatar failed to load from {url}: {exception.Message}");
                return null;
            }
        }

        private void ShowMessages(MagicWordsResponse data)
        {
            for (int i = 0; i < data.dialogue.Length; i++)
            {
                var line = data.dialogue[i];

                if (line == null || string.IsNullOrWhiteSpace(line.name))
                {
                    continue;
                }

                if (!_speakers.TryGetValue(line.name, out SpeakerProfile speaker))
                {
                    continue;
                }

                var prefab = speaker.AlignRight ? _rightMessagePrefab : _leftMessagePrefab;
                var message = Instantiate(prefab, _content);
                message.Bind(speaker, DialogueTextFormatter.Format(line.text));
            }

            ScrollToTop();
        }

        private void ScrollToTop()
        {
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(_content);
            _scrollRect.verticalNormalizedPosition = 1f;
        }

        private void HideStatus()
        {
            _statusText.gameObject.SetActive(false);
        }

        private void ShowStatus(string message)
        {
            _statusText.text = message;
            _statusText.gameObject.SetActive(true);
        }

        private static bool IsRight(string position)
        {
            return !string.IsNullOrEmpty(position) &&
                   position.Equals("right", StringComparison.OrdinalIgnoreCase);
        }
    }
}
