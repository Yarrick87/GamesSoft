using UnityEngine;

namespace GamesSoft.AceOfShadows
{
    [CreateAssetMenu(menuName = "GamesSoft/AceOfShadows/CardSpriteLibrary")]
    public class CardSpriteLibrary : ScriptableObject
    {
        [SerializeField]
        private Sprite[] _faces;

        public Sprite GetFace(int cardIndex)
        {
            var faceIndex = cardIndex % _faces.Length;

            return _faces[faceIndex];
        }
    }
}
