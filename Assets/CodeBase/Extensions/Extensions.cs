using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace CodeBase.Extensions
{
    public static class Extensions
    {
        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }        
        public static float AsPercentFactor(this float value) => value / 100;

        public static bool ContainsInInterval(this float value, float min, float max) => value >= min && value <= max;
        public static bool ContainsInInterval(this int value, int min, int max) => value >= min && value <= max;

        public static bool InCoordinatesRange(this Rectangle rectangle, Vector4 range)
        {
            foreach (Vector2 corner in rectangle.Corners)
            {
                if (corner.x.ContainsInInterval(range.x, range.y) &&
                    corner.y.ContainsInInterval(range.z, range.w))
                    continue;

                return false;
            }

            return true;
        }
    }
}