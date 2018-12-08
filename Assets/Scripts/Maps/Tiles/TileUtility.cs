using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileUtility {


    public static AxialCord axialFromCartesian(int x, int y, int width, int height)
    {
        /**
         * NOTE: This function requires this.height and this.width to be set acuratly.
         * Something to do with the center point.
         */
        int newQ = (x) - Mathf.FloorToInt(width / 2) - Mathf.FloorToInt(y / 2) + Mathf.FloorToInt(height / 4);
        int newR = (y - (Mathf.FloorToInt(height / 2)));
        return new AxialCord(newQ, newR);
    }

}
