using UnityEngine;

namespace Utils
{
    public static class Utils
    {
        public static Vector2 Clamp(Vector2 value, Vector2 min, Vector2 max) =>
            new Vector2(Mathf.Clamp(value.x, min.x, max.x), Mathf.Clamp(value.y, min.y, max.y));

        public static Vector2 Clamp01(Vector2 value) =>
            new Vector2(Mathf.Clamp01(value.x), Mathf.Clamp01(value.y));

        public static Vector2 Clamp(Vector2 value, float minX, float maxX, float minY, float maxY) =>
            new Vector2(Mathf.Clamp(value.x, minX, maxX), Mathf.Clamp(value.y, minY, maxY));

        public static bool IsBetween(Vector2 value, Vector2 min, Vector2 max) =>
            min.x <= value.x && value.x <= max.x &&
            min.y <= value.y && value.y <= max.y;

        public static int CohenSutherlandTBRL(Vector2 value, Vector2 bottomLeft, Vector2 topRight)
        {
            int Tpow = 0b1000;
            int Bpow = 0b100;
            int Rpow = 0b10;
            int Lpow = 0b1;

            bool T = value.y > topRight.y;
            bool B = value.y < bottomLeft.y;
            bool R = value.x > topRight.x;
            bool L = value.x < bottomLeft.x;

            return
                Lpow * ((L) ? 1 : 0) +
                Rpow * ((R) ? 1 : 0) +
                Bpow * ((B) ? 1 : 0) +
                Tpow * ((T) ? 1 : 0);

        }

        public static Vector2 screenSize { get => new Vector2(Screen.width, Screen.height); }

        public static (Vector2 p1prime, Vector2 p2prime) clipSegmentToRectangle(Vector2 p1, Vector2 p2, Vector2 bottomLeft, Vector2 topRight)
        {
            int Tpow = 0b1000;
            int Bpow = 0b100;
            int Rpow = 0b10;
            int Lpow = 0b1;

            int tbrl1 = CohenSutherlandTBRL(p1, bottomLeft, topRight);
            int tbrl2 = CohenSutherlandTBRL(p2, bottomLeft, topRight);

            if(tbrl1 == 0 && tbrl2 == 0) return (p1, p2);
            if((tbrl1 & tbrl2) != 0) return (Vector2.negativeInfinity, Vector2.negativeInfinity);

            //Ensure p2 is always the one outside
            if(tbrl2 == 0) return clipSegmentToRectangle(p2, p1, bottomLeft, topRight);

            Vector2 directionVector = (p2 - p1).normalized;
            if((tbrl2 & Tpow) != 0)
            {
                float lambda = (topRight.y - p1.y) / directionVector.y;
                Vector2 p2prime = p1 + lambda * directionVector;
                return (p1, p2prime);
            }
            if ((tbrl2 & Bpow) != 0)
            {
                float lambda = (bottomLeft.y - p1.y) / directionVector.y;
                Vector2 p2prime = p1 + lambda * directionVector;
                return (p1, p2prime);
            }
            if ((tbrl2 & Rpow) != 0)
            {
                float lambda = (topRight.x - p1.x) / directionVector.x;
                Vector2 p2prime = p1 + lambda * directionVector;
                return (p1, p2prime);
            }
            if ((tbrl2 & Lpow) != 0)
            {
                float lambda = (bottomLeft.x - p1.x) / directionVector.x;
                Vector2 p2prime = p1 + lambda * directionVector;
                return (p1, p2prime);
            }

            return (Vector2.negativeInfinity, Vector2.negativeInfinity);
        }

        public static Vector3 Clamp(Vector3 value, Vector3 min, Vector3 max) =>
            new Vector3(Mathf.Clamp(value.x, min.x, max.x), Mathf.Clamp(value.y, min.y, max.y), Mathf.Clamp(value.z, min.z, max.z));

        public static Vector3 Clamp01(Vector3 value) =>
            new Vector3(Mathf.Clamp01(value.x), Mathf.Clamp01(value.y), Mathf.Clamp01(value.z));

        public static bool IsBetween(Vector3 value, Vector3 min, Vector3 max) =>
            min.x <= value.x && value.x <= max.x &&
            min.y <= value.y && value.y <= max.y &&
            min.z <= value.z && value.z <= max.z;
    }
}