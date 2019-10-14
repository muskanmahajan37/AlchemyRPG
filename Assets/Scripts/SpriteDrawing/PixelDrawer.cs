using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PixelDrawer {

    // Notes on pixel art: https://blogs.unity3d.com/2015/06/19/pixel-perfect-2d/

    #region Lines

    // Bresenham's line algs taken from wikipedia https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm
    private static void BresenhamLow(Texture2D t, Vector2Int start, Vector2Int end, Color c) {
        int deltaX = end.x - start.x;
        int deltaY = end.y - start.y;
        int yDirection = 1;
        if (deltaY < 0) {
            yDirection = -1;
            deltaY *= -1;
        }
        float error = (2 * deltaY) - deltaX;

        int y = start.y;
        for (int x = start.x; x <= end.x; x++) {
            t.SetPixel(x, y, c);
            if (error > 0) {
                y += yDirection;
                error -= (2 * deltaX);
            }
            error += (2 * deltaY);
        }
    }

    private static void Bresenham(Texture2D t, Vector2Int start, Vector2Int end, Color c) {
        float deltaX = end.x - start.x;
        float deltaY = end.y - start.y;
        float deltaError = Mathf.Abs(deltaY / deltaX);  // slope of the line, between 0.0. and 1.0
                                                        // NOTE, deltaX must not == 0, IE: no vertical lines
        float error = 0.0f;
        int y = start.y;
        for (int x = start.x; x < end.x; x++) {
            t.SetPixel(x, y, c);
            error += deltaError;
            if (error >= 0.5f) {
                y += 1;
                error -= 0.1f;
            }
        }
    }

    private static void BresenhamHigh(Texture2D t, Vector2Int start, Vector2Int end, Color c) {
        int deltaX = end.x - start.x;
        int deltaY = end.y - start.y;
        int xDirection = 1;
        if (deltaX < 0) {
            xDirection = -1;
            deltaX *= -1;
        }
        float error = (2 * deltaX) - deltaY;

        int x = start.x;
        for (int y = start.y; y <= end.y; y++) {
            t.SetPixel(x, y, c);
            if (error > 0) {
                x += xDirection;
                error -= (2 * deltaY);
            }
            error = (2 * deltaX);
        }
    }

    public static void DrawLine(Texture2D t, Vector2Int start, Vector2Int end, Color c) {
        int deltaY = end.y - start.y;
        int deltaX = end.x - start.x;
        if (Mathf.Abs(deltaY) < Mathf.Abs(deltaX))  {
            if (start.x > end.x) { BresenhamLow(t, end, start, c); }
            else                 { BresenhamLow(t, start, end, c); }
        } else {
            if (start.y > end.y) { BresenhamHigh(t, end, start, c); }
            else                 { BresenhamHigh(t, start, end, c); }
        }
    }
    
    public static void drawDownwardVerticalLine(Texture2D t, Vector2Int start, int length, Color c) {
        for (int i = 0; i < length; i++)
            { t.SetPixel(start.x, start.y + i, c); }
    }

    public static void drawLeftwardHorizontalLine(Texture2D t, Vector2Int start, int length, Color c) {
        for (int i = 0; i < length; i++)
            { t.SetPixel(start.x + i, start.y, c); }
    }

    public static Sprite packTexture(Texture2D texture) {
        Rect rect = new Rect(0, 0, texture.width, texture.height);
        Vector2 piviot = new Vector2(0.5f, 0.5f);
        return Sprite.Create(texture, rect, piviot);
    }

    #endregion

    #region Curves

    private static int radiusError(int x, int y, int r) { return(x*x) + (y*y) - (r*r); }
    private static int yChange(int y) { return (2 * y) + 1; }
    private static int xChange(int x) { return 1 - (2 * x); }
    public static void MidpointCircleAlgorithim(Texture2D t, Vector2Int center, int radius, Color c) {
        int xOffset = center.x;
        int yOffset = center.y;
        int x = radius;
        int y = 0;
        while (x >= y) {
            t.SetPixel(xOffset + x, yOffset + y, c);  // o1
            t.SetPixel(xOffset - x, yOffset + y, c);  // o4
            t.SetPixel(xOffset + x, yOffset - y, c);  // o8
            t.SetPixel(xOffset - x, yOffset - y, c);  // o5

            t.SetPixel(xOffset + y, yOffset + x, c);  // o2
            t.SetPixel(xOffset - y, yOffset + x, c);  // o3
            t.SetPixel(xOffset + y, yOffset - x, c);  // o7
            t.SetPixel(xOffset - y, yOffset - x, c);  // o6
            
            int decisionFactor = 2 * (radiusError(x, y, radius) + yChange(y)) + xChange(x);
            if (decisionFactor > 0) { x--; }
            //if (radiusError(x-1, y+1, radius) < radiusError(x, y+1, radius)) { x--; }
            y ++; // y always increases
        }

    }

    #endregion
}
