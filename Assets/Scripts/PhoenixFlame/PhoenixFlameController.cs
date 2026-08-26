using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace GamesSoft.PhoenixFlame
{
    public class PhoenixFlameController : MonoBehaviour
    {
        [SerializeField]
        private Color[] _colors =
        {
            new Color(1f, 0.45f, 0.08f, 1f),
            new Color(0.25f, 0.95f, 0.35f, 1f),
            new Color(0.25f, 0.55f, 1f, 1f)
        };

        [SerializeField]
        private ParticleSystem[] _particleSystems;

        [SerializeField]
        private Button _cycleColorButton;

        [SerializeField]
        private float _transitionDuration = 0.85f;

        private int _colorIndex;
        private bool _isTransitioning;
        private Color _fireColor;
        private ParticleSystem.Particle[] _particleBuffer;

        private void Awake()
        {
            _fireColor = _colors[_colorIndex];
            _cycleColorButton.onClick.AddListener(OnCycleColorClicked);
            ApplyFireColor(_fireColor);
        }

        private void OnDestroy()
        {
            if (_cycleColorButton != null)
            {
                _cycleColorButton.onClick.RemoveListener(OnCycleColorClicked);
            }
        }

        private async void OnCycleColorClicked()
        {
            if (_isTransitioning)
            {
                return;
            }

            _isTransitioning = true;

            try
            {
                var nextIndex = (_colorIndex + 1) % _colors.Length;
                await TransitionTo(_colors[nextIndex], destroyCancellationToken);
                _colorIndex = nextIndex;
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                if (this)
                {
                    _isTransitioning = false;
                }
            }
        }

        private async Awaitable TransitionTo(Color targetColor, CancellationToken token)
        {
            var fromColor = _fireColor;
            var elapsedTime = 0f;

            while (elapsedTime < _transitionDuration)
            {
                token.ThrowIfCancellationRequested();

                elapsedTime += Time.deltaTime;
                var time = Mathf.SmoothStep(0f, 1f, elapsedTime / _transitionDuration);
                _fireColor = Color.Lerp(fromColor, targetColor, time);
                ApplyFireColor(_fireColor);
                await Awaitable.NextFrameAsync(token);
            }

            token.ThrowIfCancellationRequested();
            _fireColor = targetColor;
            ApplyFireColor(_fireColor);
        }

        private void ApplyFireColor(Color color)
        {
            var tint = (Color32)color;

            for (int i = 0; i < _particleSystems.Length; i++)
            {
                var system = _particleSystems[i];

                if (system == null)
                {
                    continue;
                }

                var main = system.main;
                main.startColor = color;

                var particleCount = system.particleCount;
                
                if (particleCount == 0)
                {
                    continue;
                }

                if (_particleBuffer == null || _particleBuffer.Length < particleCount)
                {
                    _particleBuffer = new ParticleSystem.Particle[particleCount];
                }

                var readCount = system.GetParticles(_particleBuffer);
                
                for (var p = 0; p < readCount; p++)
                {
                    var current = _particleBuffer[p].startColor;
                    _particleBuffer[p].startColor = new Color32(tint.r, tint.g, tint.b, current.a);
                }

                system.SetParticles(_particleBuffer, readCount);
            }
        }
    }
}
