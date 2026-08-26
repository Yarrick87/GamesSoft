using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GamesSoft.AceOfShadows
{
    public class CardStackView : MonoBehaviour
    {
        [SerializeField]
        private Transform _cardsRoot;
        
        [SerializeField]
        private TMP_Text _counter;
        
        [SerializeField]
        private Vector3 _cardOffset = new Vector3(0f, -0.08f, 0f);

        private readonly List<CardView> _cards = new List<CardView>();
        private int _incomingCount;

        public int NextSlotIndex => _cards.Count + _incomingCount;

        public Transform CardsRoot => _cardsRoot;

        public void BeginIncoming()
        {
            _incomingCount++;
        }

        public void Push(CardView card)
        {
            _incomingCount = Mathf.Max(0, _incomingCount - 1);
            
            _cards.Add(card);
            card.Transform.SetParent(_cardsRoot, false);
            
            ApplyLayout(card, _cards.Count - 1);
            RefreshCounter();
        }

        public CardView Pop()
        {
            if (_cards.Count == 0)
            {
                return null;
            }

            var card = _cards[_cards.Count - 1];
            _cards.RemoveAt(_cards.Count - 1);
            
            RefreshCounter();
            
            return card;
        }

        public Vector3 GetWorldPositionForIndex(int index)
        {
            return _cardsRoot.TransformPoint(GetLocalPosition(index));
        }

        private Vector3 GetLocalPosition(int index)
        {
            return _cardOffset * index;
        }

        private void ApplyLayout(CardView card, int index)
        {
            var cardTransform = card.Transform;
            cardTransform.localPosition = GetLocalPosition(index);
            card.SetSortingOrder(index);
        }

        private void RefreshCounter()
        {
            _counter.text = _cards.Count.ToString();
        }
    }
}
