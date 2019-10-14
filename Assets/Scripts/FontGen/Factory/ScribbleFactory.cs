using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScribbleFactory {

    public static IPenDownScribble buildRightHookScribble(bool upPointing, bool rightPointing, int length) {
        int y = (upPointing) ? length : -length;
        int x = (rightPointing) ? length : -length;

        Vector2Int p1 = new Vector2Int(0, 0);
        Vector2Int p2 = new Vector2Int(0, y);
        Vector2Int p3 = new Vector2Int(x, y);

        List<Vector2Int> points = new List<Vector2Int>(3) { p1, p2, p3 };
        return new MultiLineScribble(points);
    }

    public static IPenDownScribble buildRandomRightHookScribble(int minSize, int maxSize) {
        // [0, 9) IE: minSize inclusive,   maxSize exclusive
        bool up = Random.value > .5;
        bool down = Random.value > .5;
        int length = Random.Range(minSize, maxSize);
        return buildRightHookScribble(up, down, length);
    }


}