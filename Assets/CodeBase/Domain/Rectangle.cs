using System;
using System.Collections;
using CodeBase.Extensions;
using UnityEngine;

/**
 * Defines a rectangle and common operations over it.
 *
 * @author Sinisha Djukic
 * @version 1.0.0 (2001)
 */
[Serializable]
public class Rectangle
{
    /**The x coordinate of the top-left corner.*/
    public int x;

    /**The y coordinate of the top-left corner.*/
    public int y;

    /**The width of the rectangle.*/
    public int width;

    /**The height of the rectangle.*/
    public int height;

    /**
     * Create a new <code>Rectangle</code> given its boundary parameters.
     *
     * @param x x coordinate
     * @param y y coordinate
     * @param width rectangle width
     * @param height recrangle height
     */
    public Rectangle(int x, int y, int width, int height)
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

    /**
     * Checks if the first rectangle contains the second.
     *
     * @param rectA first rectangle
     * @param rectB second rectangle
     * @return <code>true</code> if <code>rectA</code> contains <code>rectB</code>
     */
    public static bool contains(Rectangle rectA, Rectangle rectB)
    {
        return rectB.x.ContainsInInterval(rectA.x, rectA.x + rectA.width) &&
               (rectB.x + rectB.width).ContainsInInterval(rectA.x, rectA.x + rectA.width) &&
               rectB.y.ContainsInInterval(rectA.y, rectA.y - rectA.height) &&
               (rectB.y - rectB.height).ContainsInInterval(rectA.y, rectA.y - rectA.height);
    }

    /**
     * Checks if two rectangles intersect
     *
     * @param rectA first rectangle
     * @param rectB second rectangle
     * @return <code>true</code> if <code>rectA</code> and <code>rectB</code> intersect
     */
    public static bool intersects(Rectangle rectA, Rectangle rectB)
    {
        return (rectA.x.ContainsInInterval(rectB.x, rectB.x + rectB.width) ||
                rectB.x.ContainsInInterval(rectA.x, rectA.x + rectA.width))
               &&
               (rectA.y.ContainsInInterval(rectB.y - rectB.height, rectB.y) ||
                rectB.y.ContainsInInterval(rectA.y - rectA.height, rectA.y));
    }

    /**
     * Computes the difference of two rectangles. Difference of two rectangles
     * can produce a maximum of four rectangles. If the two rectangles do not intersect
     * a zero-length array is returned.
     *
     * @param rectA first rectangle
     * @param rectB second rectangle
     * @return non-null array of <code>Rectangle</code>s, with length zero to four
     */
    public static Rectangle[] difference(Rectangle rectA, Rectangle rectB)
    {
        if (!intersects(rectA, rectB))
            return new Rectangle[0];

        Rectangle[] result = null;
        Rectangle top = null, bottom = null, left = null, right = null;
        int rectCount = 0;

        //compute the top rectangle
        int raHeight = rectA.y - rectB.y;
        if (raHeight > 0)
        {
            top = new Rectangle(rectA.x, rectA.y, rectA.width, raHeight);
            rectCount++;
        }

        //compute the bottom rectangle
        int rbY = rectB.y - rectB.height;
        int rbHeight = rectA.height - rectB.height - raHeight;
        if (rbHeight > 0 && rbY <= rectA.y)
        {
            bottom = new Rectangle(rectA.x, rectB.y - rectB.height, rectA.width, rbHeight);
            rectCount++;
        }

        bool rectBTLYInInterval = rectB.y.ContainsInInterval(rectA.y - rectA.height, rectA.y);
        bool rectBBLYInInterval = (rectB.y - rectB.height).ContainsInInterval(rectA.y - rectA.height, rectA.y);

        int rectLeftTLY = rectBTLYInInterval ? rectB.y : rectA.y;

        int rectLeftTLX = rectA.x;

        int rectLeftWidth = rectB.x - rectA.x;
        int rectLeftHeight;


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

        int rectRighTLX = rectB.x + rectB.width;

        int rectRightWidth = rectA.x + rectA.width - rectRighTLX;
        int rectRightHeight = rectLeftHeight;

        int rectRightTLY = rectLeftTLY;
      
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
}