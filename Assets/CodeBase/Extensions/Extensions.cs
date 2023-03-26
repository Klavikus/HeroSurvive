using System.Collections.Generic;
using CodeBase.Domain;
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

        public static int[] ConvertIndexFromLinear(this int index, int numRows, int numCols)
        {
            if (index == 0)
                return new[] {0, 0};

            int col = index % numCols;
            int row = (index - col) / numRows;

            return new[] {row, col};
        }

        public static int ConvertIndexToLinear(this int[] yx, int numCols) => yx[0] * numCols + yx[1];
    }
}