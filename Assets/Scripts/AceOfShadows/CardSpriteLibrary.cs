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
            if (_faces == null || _faces.Length == 0)
            {
                return null;
            }

            var faceIndex = Mathf.Abs(cardIndex % _faces.Length);
            return _faces[faceIndex];
        }
    }
}
