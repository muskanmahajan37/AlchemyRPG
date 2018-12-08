using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NocabFantasyMapGen : IModelGenerator<IModelTile>
{
  public Dictionary<AxialCord, IModelTile> buildModel(int width, int height)
  {
    NocabFantasyTile baseTile = new NocabFantasyTile(0, ELandType.Ocean, new AxialCord(0,0));
    Dictionary<AxialCord, IModelTile> world = tempUtility.buildMap(width, height, baseTile);
    

    List<float> mtPeaks = new List<float>();

    

    // MountainBuilder.BuildMountainRange()
    tempUtility.drawLine(world, new CubeCord(1,-4, 3), new CubeCord(2,2,-4));


    return world;
  }


}

// TODO rename this
public static class tempUtility {
  public static Dictionary<AxialCord, IModelTile> buildMap(int width, int height, NocabFantasyTile baseTile) {
    Dictionary<AxialCord, IModelTile> result = new Dictionary<AxialCord, IModelTile>();
    for(int x = 0; x < width; x++) {
      for (int y = 0; y < height; y++) {
        // Transform a standard x,y into q,r.
        var axialCord = TileUtility.axialFromCartesian(x, y, width, height);
        IModelTile newTile = new NocabFantasyTile(baseTile, axialCord);
        result[axialCord] = newTile;
      }
    }
    return result;
  }




  public static void drawLine(Dictionary<AxialCord, IModelTile> world, CubeCord a, CubeCord b) {
    List<CubeCord> tilesOnLine = MathHex.allHexesBetween(a, b);
    foreach(CubeCord curCubeCord in tilesOnLine) {
      AxialCord curAC = new AxialCord(curCubeCord);
      world[curAC].colorOverride = Color.yellow;
    }
  }


}
