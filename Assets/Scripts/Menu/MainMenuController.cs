using System;
using GamesSoft.Core;
using UnityEngine;
using UnityEngine.UI;

namespace GamesSoft.Menu
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField]
        private Button _aceOfShadowsButton;

        [SerializeField]
        private Button _magicWordsButton;

        [SerializeField]
        private Button _phoenixFlameButton;

        private void Awake()
        {
            _aceOfShadowsButton.onClick.AddListener(OnAceOfShadowsButtonClick);
            _magicWordsButton.onClick.AddListener(OnMagicWordsButtonClick);
            _phoenixFlameButton.onClick.AddListener(OnPhoenixFlameButtonClick);
        }

        private void OnDestroy()
        {
            if(_aceOfShadowsButton != null) _aceOfShadowsButton.onClick.RemoveListener(OnAceOfShadowsButtonClick);
            {
                _aceOfShadowsButton.onClick.RemoveListener(OnAceOfShadowsButtonClick);
            }

            if (_magicWordsButton != null)
            {
                _magicWordsButton.onClick.RemoveListener(OnMagicWordsButtonClick);
            }

            if (_phoenixFlameButton != null)
            {
                _phoenixFlameButton.onClick.RemoveListener(OnPhoenixFlameButtonClick);
            }
        }

        private void OnAceOfShadowsButtonClick()
        {
            LoadGameScene(SceneLoader.AceOfShadows);
        }

        private void OnMagicWordsButtonClick()
        {
            LoadGameScene(SceneLoader.MagicWords);
        }

        private void OnPhoenixFlameButtonClick()
        {
            LoadGameScene(SceneLoader.PhoenixFlame);
        }

        private async void LoadGameScene(string sceneName)
        {
            try
            {
                await SceneLoader.Load(sceneName);
            }
            catch (Exception exception)
            {
                Debug.LogError($"Error loading scene {sceneName} with exception: {exception}");
            }
        }
    }
}
