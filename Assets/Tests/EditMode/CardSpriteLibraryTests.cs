using GamesSoft.AceOfShadows;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace GamesSoft.Tests.EditMode
{
    public class CardSpriteLibraryTests
    {
        [Test]
        public void GetFace_EmptyLibrary_ReturnsNull()
        {
            var library = ScriptableObject.CreateInstance<CardSpriteLibrary>();
            Assert.That(library.GetFace(0), Is.Null);
            Object.DestroyImmediate(library);
        }

        [Test]
        public void GetFace_WrapsIndexByFaceCount()
        {
            var library = ScriptableObject.CreateInstance<CardSpriteLibrary>();
            var first = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1), Vector2.one * 0.5f);
            var second = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1), Vector2.one * 0.5f);

            var serialized = new SerializedObject(library);
            var faces = serialized.FindProperty("_faces");
            faces.arraySize = 2;
            faces.GetArrayElementAtIndex(0).objectReferenceValue = first;
            faces.GetArrayElementAtIndex(1).objectReferenceValue = second;
            serialized.ApplyModifiedPropertiesWithoutUndo();

            Assert.That(library.GetFace(0), Is.SameAs(first));
            Assert.That(library.GetFace(1), Is.SameAs(second));
            Assert.That(library.GetFace(2), Is.SameAs(first));
            Assert.That(library.GetFace(5), Is.SameAs(second));

            Object.DestroyImmediate(library);
            Object.DestroyImmediate(first);
            Object.DestroyImmediate(second);
        }
    }
}
