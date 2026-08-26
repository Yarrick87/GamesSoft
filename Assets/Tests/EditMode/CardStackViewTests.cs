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

        private static CardStackView CreateStack(out GameObject root)
        {
            root = new GameObject("Stack");
            var cardsRoot = new GameObject("CardsRoot");
            cardsRoot.transform.SetParent(root.transform);

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
