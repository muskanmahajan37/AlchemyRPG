using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NocabColorUtility : ColorUtility {
    
    public static Color createColor(int r, int g, int b, int alpha = 1)     { return createColor(floatify(r), floatify(g), floatify(b), floatify(alpha)); }
    public static Color createColor(float r, float g, float b, float alpha) { return new Color(r, g, b); }

    public static Color cloneColor(Color c) { return new Color(c.r, c.g, c.b, c.a); }

    public static Color grey(float whitePercentage, float alpahPercentage = 1f) {
        if (whitePercentage >= 1) { return Color.white; }
        if (whitePercentage <= 0) { return Color.black; }
        return new Color(whitePercentage, whitePercentage, whitePercentage, alpahPercentage);
    }

    private static float floatify(int value) {
        if (value <= 0)   { return 0.0f; }
        if (value >= 255) { return 1.0f; }
        return (value / 255f);
    }
}
