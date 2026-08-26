using TMPro;
using UnityEngine;

namespace GamesSoft.Core
{
    public class FpsCounter : MonoBehaviour
    {
        private static FpsCounter _instance;

        [SerializeField]
        private TMP_Text _label;

        [SerializeField]
        private float _updateInterval = 0.25f;

        private float _timer;
        private int _frames;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        private void Update()
        {
            _frames++;
            _timer += Time.unscaledDeltaTime;

            if (_timer < _updateInterval)
            {
                return;
            }

            var fps = _frames / _timer;
            _label.text = $"FPS: {fps:0}";
            _timer = 0f;
            _frames = 0;
        }
    }
}
