using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

  // How many hexes exist in the Model Map
  public int width;
  public int height;

  public  GameObject GOTilePrefab;

  public IModelGenerator<IModelTile> mapBuilder;

  public bool debugMode;

  private float wiuu; public float widthInUnityUnits { get{ return wiuu;} set { print("WARNING: resetting the value widthInUnityUnits is almost always wrong.");} }
  private float hiuu; public float heightInUnityUnits { get{ return hiuu;} set { print("WARNING: resetting the value heightInUnityUnits is almost always wrong.");} }

  // How many hexes should we draw 
  // This is different from the intuitive `this.width` and `this.height` values.
  // widthHexCount is the number of hexes we are going to draw. We don't always draw
  // the all the hexes all the time. 
  private int widthHexCount;
  private int heightHexCount;

  private Dictionary<AxialCord, IModelTile> modelTiles;
  private Dictionary<AxialCord, GameObject> tileGOs;

  // A utility class that does math and store vars that need to know the uu size of tiles. 
  private HexDrawingHelper hexDrawingHelper = new HexDrawingHelper(0.5f); // TODO: make this a paramater and/or pull side length from provided tile sprite? 

	// Use this for initialization
  // TODO: should this be awake? 
  //       Awake is needed (initially) for CamSizer to know the width/ height in Unity Units, and that is calculated here
	void Awake () {
    // TODO: Find a way to paramaterize this
    this.mapBuilder = new NocabFantasyMapGen();
    this.modelTiles = this.mapBuilder.buildModel(width, height);
    this.tileGOs = new Dictionary<AxialCord, GameObject>(width * height);
    
    Camera cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); 

    if (debugMode) {
      widthHexCount = width;
      heightHexCount = height;
    } else {
      // Calculate the number of tiles that can fit in the width, plus a buffer
      float heightInUU = 2.0f * cam.orthographicSize;
      float widthInUU = heightInUU * cam.aspect;
      widthHexCount = Mathf.CeilToInt(widthInUU / hexDrawingHelper.c2cW) + 1;
      heightHexCount = Mathf.CeilToInt(heightInUU / hexDrawingHelper.c2cH) + 1;
    }
    // Now place some modelTiles onto the game object tiles. 
    this.tileGOs = hexDrawingHelper.buildGOMap(widthHexCount, heightHexCount, GOTilePrefab);

    foreach (GameObject gotile in this.tileGOs.Values) {
      Debug.Log("nocab flag updating tile with model ");
      TileController tc = gotile.GetComponent<TileController>();
      AxialCord ac = new AxialCord(tc.q, tc.r);
      Debug.Log("new ac q r    " + ac.q + "   " + ac.r);
      IModelTile imt = this.modelTiles[ac];
      Debug.Log("imt   q r   "  + imt.q +  "" + imt.r);
      tc.modelTile = this.modelTiles[ac];
    }

    // Find the width of the entire world.
    this.wiuu = Mathf.Abs(this.hexDrawingHelper.c2cW * widthHexCount);
    this.hiuu = Mathf.Abs(this.hexDrawingHelper.c2cH * heightHexCount);

    // Reset the location of the tiles.
    // We do this because cs 0,0 is in the top left (and our tiles are built that way)
    // while unity 0,0 is in bottom right. This resets everything so our camera is actually looking at something
    this.moveAllTiles(new Vector3(0.0f, 0.0f, 0.0f));

    // Move the camera to the middle of the tile map
    cam.transform.position = new Vector3(widthInUnityUnits / 2, heightInUnityUnits / 2, 0);
  }


  private AxialCord axialFromCartesian(int x, int y) {
    // Todo: Can this be moved into hex or tile utility scripts? 
    /**
      * NOTE: This function requires this.height and this.width to be set acuratly.
      * Something to do with the center point.
      */
    int newQ = (x) - Mathf.FloorToInt(width / 2) - Mathf.FloorToInt(y / 2) + Mathf.FloorToInt(height / 4);
    int newR = (y - (Mathf.FloorToInt(height / 2)));
    return new AxialCord(newQ, newR);
  }

  private Vector3 unityPositionFromCartesian(int x, int y) {
    // TODO: Can this be moved into hex or tile utility scripts? 
    float size = .5f; // The length of one side of the tile
    float h = 2 * size; // The tile is 1 unit tall from top point to bottom point
    float w = Mathf.Sqrt(3) * size;  // Math magic 

    float horizDelta = w;
    float rowBump = 0;
    float horizPadding = 0.05f;
    if (y % 2 == 1) { rowBump = w / 2; }
    float vertDelta = h * 0.75f;
    float vertPadding = 0.05f;

    // negative y because cs people have 0,0 in top left
    return new Vector3(((horizDelta + horizPadding) * x) + rowBump, -1 * (vertDelta + vertPadding) * y, 20); // TODO: make z here a variable.
  }




  // TO BE SORTED
  ///////////////////////////////////////////////
  /*                                           */
  ///////////////////////////////////////////////
  // Getters

  public GameObject getTile(int q, int r) { return this.tileGOs[new AxialCord(q, r)]; }

  public IModelTile GetModelTile(int q, int r) { return this.modelTiles[new AxialCord(q, r)]; }


  // Getters
  ///////////////////////////////////////////////
  /*                                           */
  ///////////////////////////////////////////////
  // Utility

  public void moveAllTiles(Vector3 direction) {
    // Moves all the tile GameObjects in the specified direction
    // If the tile moves to far in the x or y direction, then it will loop back arround to the opposite side

    // NOTE: This is potentially called every frame. 

    foreach(GameObject go in this.tileGOs.Values) {
      float newX = (go.transform.position.x + direction.x) % this.widthInUnityUnits;
      if (newX > this.widthInUnityUnits) {
        newX = newX % this.widthInUnityUnits;
        TileController tc = go.GetComponent<TileController>();
        AxialCord newModelAC = wrapRightToLeft(tc.modelTile.axialCord, this.widthHexCount, this.width);
        tc.modelTile = this.modelTiles[newModelAC];
      }
      if (newX <= 0) { newX += this.widthInUnityUnits; }

      float newY = (go.transform.position.y + direction.y) % this.heightInUnityUnits;
      if (newY <= 0) { newY += this.heightInUnityUnits; }

      float newZ = (go.transform.position.z + direction.z);

      go.transform.position = new Vector3(newX, newY, newZ);
    }
  }



  public AxialCord wrapHorizontal(AxialCord curModelAxialCord, int numberOfHexesInCamWidth, int totalHexesInWidth, int flip) {
    // flip =  1 => moving left to right
    // flip = -1 => moving right to left
    // If confused about math, assume flip == 1 and popLeftToRight

    // Our leftmost tile 'pops' over to the far right
    // What is it's new model tile? 

    // The new model tile cords will be calculated below
    int curModelQ = curModelAxialCord.q;
    int curModelR = curModelAxialCord.r;

    // Move right down the row by NumHexesInCamWidth + 1
    int moveRight = Mathf.CeilToInt(numberOfHexesInCamWidth) + 1;

    int maxPossibleQ = Mathf.FloorToInt((totalHexesInWidth - 1) / 2.0f);
    // If moveRight will push us past the edge of the map, loop all the way left
    if (moveRight > maxPossibleQ) 
      { moveRight -= ( (totalHexesInWidth - 1) * flip); }

    AxialCord newModelCoord = new AxialCord(curModelQ + (moveRight * flip), curModelR);
    return newModelCoord;
  }

  public AxialCord wrapLeftToRight(AxialCord curModelAxialCord, int numberOfHexesInCamWidth, int totalHexesInTotalWidth) {
    return wrapHorizontal(curModelAxialCord, numberOfHexesInCamWidth, totalHexesInTotalWidth, 1);
  }

  public AxialCord wrapRightToLeft(AxialCord curModelAxialCord, int numberOfHexesInCamWidth, int totalHexesInTotalWidth) {
    return wrapHorizontal(curModelAxialCord, numberOfHexesInCamWidth, totalHexesInTotalWidth, -1);
  }


/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////

    // TODO: Remove below/ scratch work below
    public void test()
    {
        // Warning: This.width and this.height should be 6,7
        var result1 = axialFromCartesian(0, 0);
        var expected1 = new AxialCord(-2, -3);
        compare(result1, expected1);

        var result2 = axialFromCartesian(6, 0);
        var expected2 = new AxialCord(4, -3);
        compare(result2, expected2);

        var result3 = axialFromCartesian(0, 6);
        var expected3 = new AxialCord(-5, 3);
        compare(result3, expected3);

        var result4 = axialFromCartesian(6, 6);
        var expected4 = new AxialCord(1, 3);
        compare(result4, expected4);

        var result5 = axialFromCartesian(0, 5);
        var expected5 = new AxialCord(-4, 2);
        compare(result5, expected5);

        var result6 = axialFromCartesian(6, 5);
        var expected6 = new AxialCord(2, 2);
        compare(result6, expected6);

        var result7 = axialFromCartesian(3, 3);
        var expected7 = new AxialCord(0, 0);
        compare(result7, expected7);

        print("----------------------------------------------");
    }

    private void compare(AxialCord actual, AxialCord expected)
    {
        if (actual.q != expected.q ||
            expected.r != expected.r)
        {
            print("Actual vs Expected: (" + actual.q + ", " + actual.r + ")  vs  (" + expected.q + ", " + expected.r + ")");
        }
    }
}























class HexDrawingHelper : MonoBehaviour {
  // Used only for hex utility that cares about the side length of a hex 

  public readonly float sideLength; // The length of one side of the tile
  public readonly float h;// = 2.0f * sideLength; // The tile is 1 unit tall from top point to bottom point
  public readonly float w;// = Mathf.Sqrt(3) * sideLength;  // Math magic 

  public readonly float horizPadding;// = 0.05f; // TODO: might be the wrong term
  public readonly float oddRowBump;// = (w + horizPadding) / 2.0f;

  public readonly float flatEdgeHeight;// = h * 0.75f;
  public readonly float vertPadding;// = 0.03f;

  public readonly float c2cW;// = w + horizPadding;
  public readonly float c2cH;// = flatEdgeHeight + vertPadding;



  public HexDrawingHelper(float sideLength) {
    this.sideLength = sideLength;
    this.h = 2.0f * sideLength;
    this.w = Mathf.Sqrt(3) * sideLength;

    this.horizPadding = 0.05f; // TOOD: might be wrong term, make this var
    this.oddRowBump = (w + horizPadding) / 2.0f;

    this.flatEdgeHeight = h * 0.75f;
    this.vertPadding = 0.03f;

    this.c2cW = w + horizPadding;
    this.c2cH = flatEdgeHeight + vertPadding;

  }

  public Vector3 unityPositionFromCartesian(int x, int y) {
    // TODO: what units is this all in? UnityUnits? 
    // float sideLength = .5f;   // The length of one side of the tile
    // float h = 2 * sideLength; // The tile is 1 unit tall from top point to bottom point
    // float w = Mathf.Sqrt(3) * sideLength;  // Math magic 

    // float horizPadding = 0.05f; // TODO: might be the wrong term
    // float oddRowBump = 0;
    // if (y % 2 == 1) { oddRowBump = (w / 2.0f) + (horizPadding / 2.0f); }
    // Debug.Log("odd row bump: " + oddRowBump);
    float bump = 0;
    if (y % 2 == 1) { bump = oddRowBump; }
    float horizDelta = (c2cW * x) + bump;

    // float flatEdgeHeight = h * 0.75f;
    // float vertPadding = 0.03f;
    float vertDelta = c2cH * y;

    // negative y because cs people have 0,0 in top left
    return new Vector3(horizDelta, -1 * vertDelta, 20); // TODO: make z here a variable.
  }

  public Dictionary<AxialCord, GameObject> buildGOMap(int width, int height, GameObject tilePrefab) {
    // 1) Build a bunch of tile gameobject prefab objects
    // NOTE: This is the ONLY place we use cartesian (x,y) coords.
    // Use axial coords instead :) 

    GameObject tileHolder = new GameObject("TileHolder");
    Dictionary<AxialCord, GameObject> result = new Dictionary<AxialCord, GameObject>(width * height);

    for(int x = 0; x < width; x++) {
      for (int y = 0; y < height; y++) {
        GameObject newTile = Instantiate(tilePrefab);
        newTile.transform.parent = tileHolder.transform;
        // Transform a standard x,y into q,r.
        var axialCord = MathHex.axialFromCartesian(x, y, width, height);
        // IModelTile m = modelTiles[axialCord];

        TileController newTC = newTile.GetComponent<TileController>();
        // newTC.setModelTile(m);
        newTC.setAbsoluteCords(axialCord);

        result.Add(axialCord, newTile);

        // Put the tile in the right place
        newTile.transform.position = this.unityPositionFromCartesian(x, y);
      }
    }

    return result;
  }

}




