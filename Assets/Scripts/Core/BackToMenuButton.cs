using System;
using UnityEngine;
using UnityEngine.UI;

namespace GamesSoft.Core
{
    public class BackToMenuButton : MonoBehaviour
    {
        [SerializeField]
        private Button _backButton;

        private void Awake()
        {
            _backButton.onClick.AddListener(OnBackButtonClick);
        }

        private void OnDestroy()
        {
            if (_backButton != null)
            {
                _backButton.onClick.RemoveListener(OnBackButtonClick);
            }
        }

        private async void OnBackButtonClick()
        {
            try
            {
                await SceneLoader.Load(SceneLoader.Menu, destroyCancellationToken);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception exception)
            {
                Debug.LogError($"Error loading Menu with exception: {exception}");
            }
        }
    }
}
