namespace GD.Extentions
{
    using UnityEngine;

    public static class iTweenExtentions
    {
        public static void ScaleFrom(this GameObject go, Vector2 scale, float time, iTween.EaseType ease = iTween.EaseType.linear)
        {
            iTween.ScaleFrom(go,
                             iTween.Hash("x", scale.x,
                                         "y", scale.y,
                                         "time", time,
                                         "easetype", ease));
        }

        public static void ScaleFrom(this GameObject go, Vector2 scale, float time, iTween.EaseType ease, string oncomplete)
        {
            iTween.ScaleFrom(go,
                 iTween.Hash("x", scale.x,
                                         "y", scale.y,
                                         "time", time,
                                         "easetype", ease,
                                         "oncomplete", oncomplete));
        }
    }
}
