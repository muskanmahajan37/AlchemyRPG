using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ELandType {
  Mountain,
  Hills,
  Plains,
  
  Beach,
  ShallowOcean,

  Ocean,
  DeepOcean,

  Lake
}

// TODO : Static classes is a code smell. Is this a math equivilent or will people inherent from this?
public static class ELandTypeMethods {
  public static Color getColor(this ELandType elt) {
    switch(elt) {
      case ELandType.Mountain: return Color.black;
      case ELandType.Hills: return Color.grey;
      case ELandType.Plains: return Color.green;

      case ELandType.Beach: return Color.yellow;
      case ELandType.ShallowOcean: return c(144, 195, 212); // Light blue
      case ELandType.Ocean:        return c( 51, 153, 255); // Light Blue

      case ELandType.DeepOcean:    return c(  0, 185, 105);  // Dark Blue
      case ELandType.Lake:         return c( 19, 232, 228);  // Teal
      default:                     return Color.white; // TODO: Error handeling here? What is the default world tile? 
    }
  }

  public static Color c(int red, int green, int blue) {
    return new Color(red / 255f, green / 255f, blue / 255f);
  }
}
