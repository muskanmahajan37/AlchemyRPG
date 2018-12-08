using UnityEngine;

public class Noise : MonoBehaviour
{
  public static float Value(Vector3 point, float frequency) {
    point *= frequency;
    int i = (int)point.x;
    return i & 1;
  }
}
