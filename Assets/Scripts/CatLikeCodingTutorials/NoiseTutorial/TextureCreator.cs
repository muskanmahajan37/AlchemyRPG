using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureCreator : MonoBehaviour
{ 
  [Range(2, 512)]
  public int resolution = 256;

  public float frequency = 1f;

  private Texture2D texture;

  private void OnEnable () {
    if (texture == null) {
      texture = new Texture2D(resolution, resolution, TextureFormat.RGB24, false);
      texture.name = "Procedural Texture";
      texture.wrapMode = TextureWrapMode.Clamp;  // No edge wraping
      texture.filterMode = FilterMode.Trilinear; // Better smothing when viewed from afar
      texture.anisoLevel = 9;                    // Highest value, prevents fuzzy when looking at an angle
      GetComponent<MeshRenderer>().material.mainTexture = texture;
    }
    FillTexture();
  }

  private void Update() {
    if (transform.hasChanged) {
      transform.hasChanged = false;
      Debug.Log("nocab flag 1");
      FillTexture();
    }
  }

  public void FillTexture () {
		if (texture.width != resolution) {
			texture.Resize(resolution, resolution);
		}

    // TODO: Rename these to BL, BR, TL, TR
		Vector3 point00 = transform.TransformPoint(new Vector3(-0.5f, -0.5f));
		Vector3 point10 = transform.TransformPoint(new Vector3( 0.5f, -0.5f));
		Vector3 point01 = transform.TransformPoint(new Vector3(-0.5f,  0.5f));
		Vector3 point11 = transform.TransformPoint(new Vector3( 0.5f,  0.5f));

		float stepSize = 1f / resolution;
		for (int y = 0; y < resolution; y++) {
      float yCenterPoint = y + 0.5f;
      
      // Interpolation in the y direction
      // This gives us 2 "rails" on either side of the quad, which we walk down with each y++
      Vector3 point0 = Vector3.Lerp(point00, point01, yCenterPoint * stepSize);  // lerp bottom left  -> top left
      Vector3 point1 = Vector3.Lerp(point10, point11, yCenterPoint * stepSize);  // lerp bottom right -> top right
      

			for (int x = 0; x < resolution; x++) {
        float xCenterPoint = x + 0.5f;
        // Interpolate between the two rails calculated above, to generate the eact point we're considering
        // We use this to visualize the local coord of the quad. Black is in center
        Vector3 point = Vector3.Lerp(point0, point1, xCenterPoint * stepSize);

        float new_r = point.x;
        float new_g = point.y;
        float new_b = point.z;
				texture.SetPixel(x, y, Color.white * Noise.Value(point, this.frequency));
			}
		}
		texture.Apply();
	}
}
