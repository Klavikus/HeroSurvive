using System;
using CodeBase.Extensions;
using UnityEngine;

[Serializable]
public class Rectangle
{
    public static bool contains(Rectangle rectA, Rectangle rectB)
    {
        return rectB.x.ContainsInInterval(rectA.x, rectA.x + rectA.width) &&
               (rectB.x + rectB.width).ContainsInInterval(rectA.x, rectA.x + rectA.width) &&
               rectB.y.ContainsInInterval(rectA.y, rectA.y - rectA.height) &&
               (rectB.y - rectB.height).ContainsInInterval(rectA.y, rectA.y - rectA.height);
    }

    public static bool intersects(Rectangle rectA, Rectangle rectB)
    {
        return (rectA.x.ContainsInInterval(rectB.x, rectB.x + rectB.width) ||
                rectB.x.ContainsInInterval(rectA.x, rectA.x + rectA.width))
               &&
               (rectA.y.ContainsInInterval(rectB.y - rectB.height, rectB.y) ||
                rectB.y.ContainsInInterval(rectA.y - rectA.height, rectA.y));
    }

    public static Rectangle[] difference(Rectangle rectA, Rectangle rectB)
    {
        if (!intersects(rectA, rectB))
            return Array.Empty<Rectangle>();

        Rectangle[] result;
        Rectangle top = null, bottom = null, left = null, right = null;
        int rectCount = 0;

        //compute the top rectangle
        float raHeight = rectA.y - rectB.y;
        if (raHeight > 0)
        {
            top = new Rectangle(rectA.x, rectA.y, rectA.width, raHeight);
            rectCount++;
        }

        //compute the bottom rectangle
        float rbY = rectB.y - rectB.height;
        float rbHeight = rectA.height - rectB.height - raHeight;
        if (rbHeight > 0 && rbY <= rectA.y)
        {
            bottom = new Rectangle(rectA.x, rectB.y - rectB.height, rectA.width, rbHeight);
            rectCount++;
        }

        bool rectBTLYInInterval = rectB.y.ContainsInInterval(rectA.y - rectA.height, rectA.y);
        bool rectBBLYInInterval = (rectB.y - rectB.height).ContainsInInterval(rectA.y - rectA.height, rectA.y);

        float rectLeftTLY = rectBTLYInInterval ? rectB.y : rectA.y;

        float rectLeftTLX = rectA.x;

        float rectLeftWidth = rectB.x - rectA.x;
        float rectLeftHeight;


        if (rectBTLYInInterval && rectBBLYInInterval)
            rectLeftHeight = rectB.height;
        else if (rectBBLYInInterval == false && rectBTLYInInterval == false)
            rectLeftHeight = rectA.height;
        else if (rectBTLYInInterval)
            rectLeftHeight = rectB.y - (rectA.y - rectA.height);
        else
            rectLeftHeight = rectA.y - (rectB.y - rectB.height);

        if (rectLeftWidth > 0 && rectLeftHeight > 0)
        {
            left = new Rectangle(rectLeftTLX, rectLeftTLY, rectLeftWidth, rectLeftHeight);
            rectCount++;
        }

        float rectRighTLX = rectB.x + rectB.width;

        float rectRightWidth = rectA.x + rectA.width - rectRighTLX;
        float rectRightHeight = rectLeftHeight;

        float rectRightTLY = rectLeftTLY;
      
        if (rectRightWidth > 0 && rectRightHeight > 0)
        {
            right = new Rectangle(rectRighTLX, rectRightTLY, rectRightWidth, rectRightHeight);
            rectCount++;
        }

        result = new Rectangle[rectCount];
        rectCount = 0;
        if (top != null)
            result[rectCount++] = top;
        if (bottom != null)
            result[rectCount++] = bottom;
        if (left != null)
            result[rectCount++] = left;
        if (right != null)
            result[rectCount++] = right;
        return result;
    }

    public float x;
    public float y;
    public float width;
    public float height;

    public Rectangle(float x, float y, float width, float height)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;

        Corners = new[]
        {
            new Vector2(x, y),
            new Vector2(x + width, y),
            new Vector2(x + width, y - height),
            new Vector2(x, y - height),
        };
    }

    public Vector2[] Corners { get; private set; }
}