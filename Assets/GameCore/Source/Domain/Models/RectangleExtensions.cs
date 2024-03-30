using UnityEngine;

namespace GameCore.Source.Domain.Models
{
    public static class RectangleExtensions
    {
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