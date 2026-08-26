using GamesSoft.AceOfShadows;
using NUnit.Framework;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace GamesSoft.Tests.EditMode
{
    public class CardStackViewTests
    {
        [Test]
        public void Pop_EmptyStack_ReturnsNull()
        {
            var stack = CreateStack(out var root);
            Assert.That(stack.Pop(), Is.Null);
            Object.DestroyImmediate(root);
        }

        [Test]
        public void PushThenPop_ReturnsSameCard_AndUpdatesNextSlot()
        {
            var stack = CreateStack(out var root);
            var cardObject = new GameObject("Card", typeof(SpriteRenderer), typeof(CardView));
            var card = cardObject.GetComponent<CardView>();
            var cardSerialized = new SerializedObject(card);
            cardSerialized.FindProperty("_spriteRenderer").objectReferenceValue = cardObject.GetComponent<SpriteRenderer>();
            cardSerialized.ApplyModifiedPropertiesWithoutUndo();

            Assert.That(stack.NextSlotIndex, Is.EqualTo(0));
            stack.BeginIncoming();
            Assert.That(stack.NextSlotIndex, Is.EqualTo(1));

            stack.Push(card);
            Assert.That(stack.NextSlotIndex, Is.EqualTo(1));
            Assert.That(stack.Pop(), Is.SameAs(card));
            Assert.That(stack.NextSlotIndex, Is.EqualTo(0));
            Assert.That(stack.Pop(), Is.Null);

            Object.DestroyImmediate(cardObject);
            Object.DestroyImmediate(root);
        }

        [Test]
        public void GetWorldPositionForIndex_UsesHorizontalCardOffset()
        {
            var stack = CreateStack(out var root, out var cardsRoot);
            var offset = new Vector3(0.08f, 0f, 0f);
            var serialized = new SerializedObject(stack);
            serialized.FindProperty("_cardOffset").vector3Value = offset;
            serialized.ApplyModifiedPropertiesWithoutUndo();

            Assert.That(stack.GetWorldPositionForIndex(0), Is.EqualTo(cardsRoot.position));
            Assert.That(stack.GetWorldPositionForIndex(3), Is.EqualTo(cardsRoot.position + offset * 3));

            Object.DestroyImmediate(root);
        }

        private static CardStackView CreateStack(out GameObject root)
        {
            return CreateStack(out root, out _);
        }

        private static CardStackView CreateStack(out GameObject root, out Transform cardsRootTransform)
        {
            root = new GameObject("Stack");
            var cardsRoot = new GameObject("CardsRoot");
            cardsRoot.transform.SetParent(root.transform);
            cardsRootTransform = cardsRoot.transform;

            var counterObject = new GameObject("Counter", typeof(TextMeshPro));
            counterObject.transform.SetParent(root.transform);

            var stack = root.AddComponent<CardStackView>();
            var serialized = new SerializedObject(stack);
            serialized.FindProperty("_cardsRoot").objectReferenceValue = cardsRoot.transform;
            serialized.FindProperty("_counter").objectReferenceValue = counterObject.GetComponent<TextMeshPro>();
            serialized.ApplyModifiedPropertiesWithoutUndo();
            return stack;
        }
    }
}
