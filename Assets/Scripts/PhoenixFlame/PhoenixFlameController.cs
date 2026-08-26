using UnityEngine;
using UnityEngine.UI;

namespace GamesSoft.PhoenixFlame
{
    public class PhoenixFlameController : MonoBehaviour
    {
        private static readonly int ColorIndexHash = Animator.StringToHash("ColorIndex");

        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private ParticleSystem[] _particleSystems;

        [SerializeField]
        private Button _cycleColorButton;

        [SerializeField]
        private int _colorCount = 3;

        [SerializeField]
        private Color _fireColor = new Color(1f, 0.45f, 0.08f, 1f);

        private int _colorIndex;
        private Color _appliedColor;
        private ParticleSystem.Particle[] _particleBuffer;

        private void Awake()
        {
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

        private void LateUpdate()
        {
            if (_fireColor == _appliedColor)
            {
                return;
            }

            ApplyFireColor(_fireColor);
        }

        private void OnCycleColorClicked()
        {
            if (_animator == null || _animator.IsInTransition(0))
            {
                return;
            }

            _colorIndex = (_colorIndex + 1) % Mathf.Max(1, _colorCount);
            _animator.SetInteger(ColorIndexHash, _colorIndex);
        }

        private void ApplyFireColor(Color color)
        {
            _appliedColor = color;
            var tint = (Color32)color;

            for (var i = 0; i < _particleSystems.Length; i++)
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
