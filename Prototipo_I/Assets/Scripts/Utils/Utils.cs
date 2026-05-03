using UnityEngine;

namespace Utils
{
    public static class Utils
    {
        public static Vector2 Clamp(Vector2 value, Vector2 min, Vector2 max) =>
            new Vector2(Mathf.Clamp(value.x, min.x, max.x), Mathf.Clamp(value.y, min.y, max.y));

        public static Vector2 Clamp01(Vector2 value) =>
            new Vector2(Mathf.Clamp01(value.x), Mathf.Clamp01(value.y));

        public static Vector3 Clamp(Vector3 value, Vector3 min, Vector3 max) =>
            new Vector3(Mathf.Clamp(value.x, min.x, max.x), Mathf.Clamp(value.y, min.y, max.y), Mathf.Clamp(value.z, min.z, max.z));

        public static Vector3 Clamp01(Vector3 value) =>
            new Vector3(Mathf.Clamp01(value.x), Mathf.Clamp01(value.y), Mathf.Clamp01(value.z));
    }
}