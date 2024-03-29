using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.GameCore.Domain.Models
{
    public static class RectGrid
    {
        public static List<Vector2> CalculatePoints(float cellSizeX, Rectangle rectangle)
        {
            List<Vector2> points = new List<Vector2>();

            if (rectangle.width < cellSizeX || rectangle.height < cellSizeX)
                return new List<Vector2>();

            int xSteps = (int) (rectangle.width / cellSizeX);
            int ySteps = (int) (rectangle.height / cellSizeX);

            float newX = rectangle.x;
            float newY = rectangle.y;

            for (int y = 0; y < ySteps; y++)
            {
                newY -= cellSizeX / 2;
                for (int x = 0; x < xSteps; x++)
                {
                    newX += cellSizeX / 2;
                    points.Add(new Vector2(newX, newY));
                    newX += cellSizeX / 2;
                }

                newY -= cellSizeX / 2;
                newX = rectangle.x;
            }

            return points;
        }
    }
}