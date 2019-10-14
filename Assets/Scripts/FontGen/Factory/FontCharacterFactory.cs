using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FontCharacterFactory {


    public static IFontCharacter RandonPenDownCharacter(int ply) {
        // TODO: This will always use scribbles of len 5
        //  1) Build a char inside a given box size
        //  2) Build a char with variable scribble sizes
        //  3) Ensure no scribble overlap

        PenDownCharacter ch = new PenDownCharacter();
        for(int i = 0; i < ply; i++) {
            IPenDownScribble scribble = ScribbleFactory.buildRandomRightHookScribble(3, 3);
            ch.addScribble(scribble);
        }
        return ch;
    }


    public static IFontCharacter buildBoundedCharacter(int width, int height) {
        // Build a font character that fits inside the provided bounds
        // It's reccomended that width and height are multiples of 4 but not required

        List<IPenDownScribble> scribbles = new List<IPenDownScribble>();
        Vector2Int lastPoint = new Vector2Int(0, 0);

        int maxX = int.MinValue;
        int minX = int.MaxValue;
        int maxY = int.MinValue;
        int minY = int.MaxValue;

        int ply = 5;
        for (int i = 0; i < ply; i++) {
            // 1) Generate scribble
            IPenDownScribble s = ScribbleFactory.buildRandomRightHookScribble(4, 4);

            // 2) Validate scribble
            // Compare the existing maxX to the scribble's translated max X value
            int potentialMaxX = Mathf.Max(maxX, s.rightBound() + lastPoint.x);
            int potentialMinX = Mathf.Min(minX, s.leftBound()  + lastPoint.x);
            int potentialMaxY = Mathf.Max(maxY, s.upBound()    + lastPoint.y);
            int potentialMinY = Mathf.Min(minY, s.downBound()  + lastPoint.y);

            int potentialWidth = potentialMaxX - potentialMinX;
            int potentialHeight = potentialMaxY - potentialMinY;

            if (potentialWidth > width ||
                potentialHeight > height) {
                // If adding the new scribble makes the character too big
                // skip it
                continue;
            }

            //Debug.Log("--------------------");
            //Debug.Log("oldMaxX: " + maxX + "   newMaxX:" + potentialMaxX);
            //Debug.Log("oldMaxY: " + maxY + "   newMaxY:" + potentialMaxY);
            //Debug.Log("oldMinX: " + minX + "   newMinX:" + potentialMinX);
            //Debug.Log("oldMinY: " + minY + "   newMinY:" + potentialMinY);

            // 3) Update factory state
            maxX = potentialMaxX;
            minX = potentialMinX;
            maxY = potentialMaxY;
            minY = potentialMinY;
            lastPoint += s.lastPoint();

            // 4) Add scribble to result
            scribbles.Add(s);
        }

        // 5) Translate true center to (0, 0)
        int centerX = (maxX + minX) / 2;
        int centerY = (maxY + minY) / 2;

        scribbles[0].translate(-centerX, -centerY);
        Debug.Log("centerx: " + centerX + "  centerY: " + centerY);
        // NOTE: Because the PenDownCharacter is drawn based off the end pos of
        //       the preceding scribble, we only need to translate the first scribble
        //       and all the others will automatically be in place. 


        return new PenDownCharacter(scribbles);
    }
    

}
