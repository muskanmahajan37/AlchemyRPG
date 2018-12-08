using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MountainBuilder {




  public static List<float> heightBetweenTwoPoints(float leftPoint, float rightPoint, int divisions, float drama) {
    /**
      Build a list of new heights. 
      The first element of the list is always leftPoint, 
      the last element is always rightPoint.

      The divisions input says how many peaks or valeys should
      be in the resulting list. divisions = 0 implys a list of length 2 (left and right points only)

      Drama implys how dramatic the changes can be. TODO, this isn't implimented

     */

    // So, find the "midle" division

    int size = 2 + divisions;
    int lastIndex = size - 1;

    float[] result = new float[size];
    result[0] = leftPoint;
    result[lastIndex] = 0;


    for(int i = 0; i < divisions; i++) {
      // Find the height for the middle index
      int midelIndex = (0 + lastIndex) / 2;
      float averageHeight = (result[0] + result[lastIndex]) / 2;

      float bump = 0;

      result[midelIndex] = averageHeight + bump;
    }



    return new List<float>(result);
  }

  private class LineSeg {
    public int l;
    public int r;

    public LineSeg(int l, int r) { this.l = l; this.r = r; }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType()) { return false; }
        LineSeg other = (LineSeg)obj;
        return other.l == this.l && other.r == this.r;
    }
    
    // override object.GetHashCode
    public override int GetHashCode()
    {
      int hash = 397;
        
      int hashCode = 0;
      hashCode = (hashCode * hash) ^ this.l.GetHashCode();
      hashCode = (hashCode * hash) ^ this.r.GetHashCode();
      return hashCode;
    }
  }

  // TODO: consider having this take in List<LineSeg> and List<LineSeg> linesToDo
  public static List<float> buildSlope(int size, float leftHeight, float rightHeight) {
    // result is a list of heights such that result[tile_index] == height of mountain on that tile
    List<float> result = new List<float>(size);
    result[0] = leftHeight;
    result[size - 1] = rightHeight;

    // Invariants on linesToDo:
    //    - linesToDo is a list of line segments
    //    - the leftPoint and rightPoint in the LineSeg will always be a valid height in result
    //    - the indexes between leftPoint and rightPoint are not set
    //    - once linesToDo is empty, every index in result will be generated with a valid height
    List<LineSeg> linesToDo = new List<LineSeg>();
    linesToDo.Add(new LineSeg(0, size - 1));

    while (linesToDo.Count > 0) {
      LineSeg curSeg = linesToDo[0];
      int lp = curSeg.l;
      int rp = curSeg.r;
      linesToDo.RemoveAt(0);

      if (Mathf.Abs(lp - rp) == 1) {
        // If the 2 points are right next to eachother
        // remove this line seg from the set
       continue;
      }
      if (Mathf.Abs(lp - rp) == 2) {
        // If there's 1 space between the 2 points
        float lHeight = result[lp];
        float rHeight = result[rp];
        float bump = 0; // TODO: The bump value based of dist between lp and rp over total size
        result[lp + 1] = ((lHeight + rHeight) / 2) + bump;
        continue;

      } else {

        // Else >1 space between lp and rp
        int l1 = lp;
        int r1 = (lp + rp) / 2;
        int l2 = r1 + 1;
        int r2 = rp;

        float bump1 = 0; // TODO: these
        float bump2 = 0;
        float bump3 = 0;

        float midHeight = (result[lp] + result[rp] / 2) + bump1;

        result[r1] = midHeight + bump2;
        result[l2] = midHeight + bump3;

        linesToDo.Add(new LineSeg(l1, r1));
        linesToDo.Add(new LineSeg(l2, r2));
      } // End else
    } // End while 
    return result;
  }

  

  public static void BuildMountainRange(List<AxialCord> peaks) {

    foreach(AxialCord ac in peaks) {
      // I need to get all the tiles between two unity points
    }


  }

}
