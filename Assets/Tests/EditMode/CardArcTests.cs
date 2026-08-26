using GamesSoft.AceOfShadows;
using NUnit.Framework;
using UnityEngine;

namespace GamesSoft.Tests.EditMode
{
    public class CardArcTests
    {
        [Test]
        public void Evaluate_AtZero_ReturnsStart()
        {
            var start = new Vector3(1f, 2f, 3f);
            var control = new Vector3(4f, 5f, 6f);
            var end = new Vector3(7f, 8f, 9f);

            Assert.That(CardArc.Evaluate(start, control, end, 0f), Is.EqualTo(start));
        }

        [Test]
        public void Evaluate_AtOne_ReturnsEnd()
        {
            var start = new Vector3(1f, 2f, 3f);
            var control = new Vector3(4f, 5f, 6f);
            var end = new Vector3(7f, 8f, 9f);

            Assert.That(CardArc.Evaluate(start, control, end, 1f), Is.EqualTo(end));
        }

        [Test]
        public void Evaluate_AtHalf_UsesQuadraticBezierFormula()
        {
            var start = Vector3.zero;
            var control = new Vector3(0f, 2f, 0f);
            var end = new Vector3(2f, 0f, 0f);

            // B(0.5) = 0.25*P0 + 0.5*P1 + 0.25*P2
            var mid = CardArc.Evaluate(start, control, end, 0.5f);
            Assert.That(mid.x, Is.EqualTo(0.5f).Within(0.0001f));
            Assert.That(mid.y, Is.EqualTo(1f).Within(0.0001f));
        }
    }
}
