using UnityEngine;

namespace GamesSoft.AceOfShadows
{
    public static class CardArc
    {
        public static Vector3 Evaluate(Vector3 startPos, Vector3 controlPos, Vector3 endPos, float time)
        {
            var oneMinus = 1f - time;
            return oneMinus * oneMinus * startPos
                   + 2f * oneMinus * time * controlPos
                   + time * time * endPos;
        }
    }
}
