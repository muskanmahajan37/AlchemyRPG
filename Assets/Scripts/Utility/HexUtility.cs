using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class MathHex  {

  public static int hexDistance(CubeCord a, CubeCord b) {
    // Returns the distance [) between two arbitrary cube points
    // NOTE [) means this func will count the first provided hex in its count, but not the second
    return Mathf.Max(Mathf.Abs(a.x - b.x),
                     Mathf.Abs(a.y - b.y),
                     Mathf.Abs(a.z - b.z));
  }

  public static CubeCord cubicRound(Vector3 v) { return cubicRound(v.x, v.y, v.z); }
  public static CubeCord cubicRound(float xfloat, float yfloat, float zfloat) {
    int x = Mathf.RoundToInt(xfloat);
    int y = Mathf.RoundToInt(yfloat);
    int z = Mathf.RoundToInt(zfloat);

    float xDiff = Mathf.Abs(x - xfloat);
    float yDiff = Mathf.Abs(y - yfloat);
    float zDiff = Mathf.Abs(z - zfloat);

    if(xDiff > yDiff && xDiff > zDiff) {
      x = -y - z;
    } else if (yDiff > zDiff) {
      y = -x - z;
    } else {
      z = -x - y;
    }

    return new CubeCord(x, y , z);
  }

  public static Vector3 cubicLerp(CubeCord a, CubeCord b, float t) {
    float newX = Mathf.Lerp(a.x, b.x, t);
    float newY = Mathf.Lerp(a.y, b.y, t);
    float newZ = Mathf.Lerp(a.z, b.z, t);
    return new Vector3(newX, newY, newZ);
  }
  
  public static List<CubeCord> allHexesBetween(CubeCord lp, CubeCord rp) {
    int hexDist = hexDistance(lp, rp);
    List<CubeCord> result = new List<CubeCord>(hexDist);
    for(int i = 0; i <= hexDist; i++) {
      CubeCord nextInLine = cubicRound(cubicLerp(lp, rp, (1.0f * i) / (hexDist)));
      result.Add(nextInLine);
    }
    return result;
  }

  public static AxialCord axialFromCartesian(int x, int y, int tilesInWidth, int tilesInHeight) {
      /**
      * NOTE: This function requires this.height and this.width to be set acuratly.
      * Something to do with the center point.
      */
    int newQ = (x) - Mathf.FloorToInt(tilesInWidth / 2) - Mathf.FloorToInt(y / 2) + Mathf.FloorToInt(tilesInHeight / 4);
    int newR = (y - (Mathf.FloorToInt(tilesInHeight / 2)));
    return new AxialCord(newQ, newR);
  }

  
}


