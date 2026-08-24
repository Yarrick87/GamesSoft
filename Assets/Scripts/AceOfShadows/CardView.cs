using UnityEngine;

namespace GamesSoft.AceOfShadows
{
    public class CardView : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        public Transform Transform => transform;

        public void SetFace(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }

        public void SetSortingOrder(int order)
        {
            _spriteRenderer.sortingOrder = order;
        }
    }
}
