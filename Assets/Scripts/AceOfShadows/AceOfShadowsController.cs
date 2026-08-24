using System.Collections;
using TMPro;
using UnityEngine;

namespace GamesSoft.AceOfShadows
{
    public class AceOfShadowsController : MonoBehaviour
    {
        const int FlyingSortingOrder = 1000;
        
        [SerializeField]
        private int _cardCount = 144;
        
        [SerializeField]
        private float _dealInterval = 1f;
        
        [SerializeField]
        private float _moveDuration = 1.5f;

        [SerializeField]
        private CardView _cardPrefab;
        
        [SerializeField]
        private CardSpriteLibrary _spriteLibrary;
        
        [SerializeField]
        private CardStackView[] _stacks;
        
        [SerializeField]
        private Transform _flightLayer;
        
        [SerializeField]
        private TMP_Text _statusText;

        private int _cardsInFlight;
        private int _movedCards;
        private bool _finished;

        private void Start()
        {
            HideStatus();
            SpawnDeck();
            StartCoroutine(DealRoutine());
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        private void SpawnDeck()
        {
            var cardStack = _stacks[0];
            
            for (var cardIndex = 0; cardIndex < _cardCount; cardIndex++)
            {
                var card = Instantiate(_cardPrefab, cardStack.CardsRoot);
                
                card.name = $"Card_{cardIndex:000}";
                card.SetFace(_spriteLibrary.GetFace(cardIndex));
                cardStack.Push(card);
            }
        }

        private IEnumerator DealRoutine()
        {
            var waitTime = new WaitForSecondsRealtime(_dealInterval);
            
            var sourceCardStack = _stacks[0];
            var destinationCardStack = _stacks[1];

            while (_movedCards < _cardCount)
            {
                yield return waitTime;

                var card = sourceCardStack.Pop();

                StartCoroutine(MoveCard(card, destinationCardStack));
                
                _movedCards++;
            }
        }

        private IEnumerator MoveCard(CardView card, CardStackView destination)
        {
            _cardsInFlight++;

            var slotIndex = destination.NextSlotIndex;
            destination.BeginIncoming();

            var startPos = card.Transform.position;
            var endPos = destination.GetWorldPositionForIndex(slotIndex);
            var controlPos = (startPos + endPos) * 0.5f + Vector3.up * 1.2f;

            card.Transform.SetParent(_flightLayer, true);
            card.SetSortingOrder(FlyingSortingOrder + _cardsInFlight);

            var elapsedTime = 0f;
            
            while (elapsedTime < _moveDuration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                var time = Mathf.Clamp01(elapsedTime / _moveDuration);
                var easedTime = time * time * (3f - 2f * time);
                card.Transform.position = EvaluateArc(startPos, controlPos, endPos, easedTime);
                
                yield return null;
            }

            card.Transform.position = endPos;
            destination.Push(card);
            _cardsInFlight--;

            if (!_finished && _movedCards >= _cardCount && _cardsInFlight == 0)
            {
                _finished = true;
                ShowStatus("All cards have moved");
            }
        }

        private Vector3 EvaluateArc(Vector3 startPos, Vector3 controlPos, Vector3 endPos, float time)
        {
            var oneMinus = 1f - time;
            
            return oneMinus * oneMinus * startPos + 2f * oneMinus * time * controlPos + time * time * endPos;
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
    }
}
