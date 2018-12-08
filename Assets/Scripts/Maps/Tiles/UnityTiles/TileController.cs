using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxialCord {
    // A small container for cartesian points using ints
    /**
     * Read (Q, R)
     * (0,0) is in the center of a hex grid.
     * +R direction is down. 
     * -R direction is up.
     *      In other words, the center horizontal line has r = 0.
     *      All horizontial lines below that increase r by 1..
     * 
     * +Q direction is NE
     * -Q direction is SW
     *      In other words, the 45 degree line that runs from (SE <-> NW) has q = 0
     *      All parellel lines to the right/above/NE of that increase q by 1.
     */


    public AxialCord(CubeCord cube) {
        this.q = cube.x;
        this.r = cube.z;
    }
    public AxialCord(int q, int r) {
        this.q = q;
        this.r = r;
    }
    public int q {
        get { return qCoord; }
        set { qCoord = value; }
    }
    public int r {
        get { return rCoord; }
        set { rCoord = value; }
    }

    private int qCoord;
    private int rCoord;


    public override bool Equals(object obj) {
        AxialCord other = obj as AxialCord;
        if (other == null) return false;
        return this.q == other.q && this.r == other.r;
    }

    public override int GetHashCode() {
        // TODO: Clean this up. I think the second implimentation is better.
        // int hash = 17;
        // hash = ((hash + q) << 5) - (hash + q);
        // hash = ((hash + r) << 5) - (hash + r);
        // return hash;

        int hash = 397;
        
        int hashCode = 0;
        hashCode = (hashCode * hash) ^ this.q.GetHashCode();
        hashCode = (hashCode * hash) ^ this.r.GetHashCode();
        return hashCode;
    }

}

public class CubeCord {
    // A small container for cube points using ints
    /**
     * (0,0,0) is the center of a hex grid
     * 
     * +X direction is
     * -X direction is
     *      In other words, the 45 degree line that runs from (SE <-> NW) has x = 0
     *      All parellel lines to the right/above/NE of that increase x by 1.
     *      NOTE: cube.x = axial.q
     * 
     * +Y Direction is
     * -Y direction is
     *      In other words, the 45 degree line that runs from (SW <-> NE) has y = 0
     *      All parellel lines to the left/above/NW of that increase y by 1.
     * 
     * +Z direction is down
     * -Z direction is up
     *      In other words, the center horizontal line has z = 0.
     *      All horizontial lines below that increase z by 1.
     *      NOTE: cube.z = axial.r
     * 
     */
    public CubeCord(AxialCord axial) {
        this.x = axial.q;
        this.z = axial.r;
        this.y = (-this.x) - this.z;
    }
    public CubeCord(int x, int y, int z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public int x {
        get;
        set;
    }
    public int y {
        get;
        set;
    }
    public int z {
        get;
        set;
    }

    public override bool Equals(object obj) {
        CubeCord other = obj as CubeCord;
        if (other == null) return false;
        return (this.x == other.x) && 
               (this.y == other.y) && 
               (this.z == other.z);
    }

    
    public override int GetHashCode() {
        // Taken from here http://dmauro.com/post/77011214305/a-hashing-function-for-x-y-z-coordinates
        // The commented out part is from the link. IDK if it works or not
        // TODO: make sure this works

        // int tempX;
        // if (x >= 0) { tempX = 2 * x; } else { tempX = -2 * x - 1; }
        // int tempY;
        // if (y >= 0) { tempY = 2 * y; } else { tempY = -2 * y - 1; }
        // int tempZ;
        // if (z >= 0) { tempZ = 2 * z; } else { tempZ = -2 * z - 1; }

        // int max = Mathf.Max(tempX, tempY, tempZ);
        // int hash = ((int)Mathf.Pow(max,3)) + (2 * max * tempZ) + tempZ;
        // if (max == tempZ) {
        //     hash += (int) Mathf.Pow(Mathf.Max(tempX, tempY), 2);
        // }

        // if (tempY >= tempX) {
        //     hash += tempX + tempY;
        // } else {
        //     hash += tempY;
        // }
        // return hash;
        
        int hash = 397;
        
        int hashCode = 0;
        hashCode = (hashCode * hash) ^ this.x.GetHashCode();
        hashCode = (hashCode * hash) ^ this.y.GetHashCode();
        hashCode = (hashCode * hash) ^ this.z.GetHashCode();
        return hashCode;
    }
}


public class TileController : MonoBehaviour {
    /**
     * A script used to control a nocabTile Game Object. 
     * So this Monobehavior Component contain a ModelTile Controller, and is given commands on how to update/ redraw itself.
     * 
     * Information/ commands should flow into this tile. Rarely (in the case of some look ups/ click events) should info flow out.
     */

    private IModelTile mt; 
    public IModelTile modelTile { get {return this.getModelTile(); } set {this.setModelTile(value);} }

    // TODO: make this better
    private int Q;
    public int q { get {return Q;} set { /* TODO: is leaving this blank ok? */ }}
    private int R;
    public int r { get {return R;} set {/* TODO: is leaving this blank ok? */}}


    // Used to describe the position of this tile in the map grid. The tile may be showing a ModelTile with a different coord position. 
    private AxialCord absoluteAxialCord;
    private CubeCord absoluteCubeCord;

    private SpriteRenderer sprite;

    void Awake () {
      this.sprite = this.GetComponent<SpriteRenderer>();
    }

    public void setModelTile(IModelTile newMT) {
        // TODO: error checking for nulls/ overriding of modelTiles. Also in this.absolute*Cord fields
        this.mt = newMT;
        this.changeColor(newMT.tileColor());  // TODO: Impliment providing different images instead of colors
    }

    public IModelTile getModelTile() {
        return this.mt;
    }

    public void setAbsoluteCords(int q, int r) {
        // WARNING: This should only be called during initilization
        setAbsoluteCords(new AxialCord(q, r));
    }
    public void setAbsoluteCords(int x, int y, int z) {
        // WARNING: This should only be called during initilization
        setAbsoluteCords(new CubeCord(x, y, z));
    }
    public void setAbsoluteCords(AxialCord ac) {
        // WARNING: This should only be called during initilization
        this.absoluteAxialCord = ac;
        this.absoluteCubeCord = new CubeCord(ac);

        this.Q = ac.q;
        this.R = ac.r;

        if (Q == 0 && R == 0) {
            this.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
    public void setAbsoluteCords(CubeCord cc) {
        // WARNING: This should only be called during initilization
        this.absoluteAxialCord = new AxialCord(cc);
        this.absoluteCubeCord = cc;
    }

    private void changeColor(Color c) {
      if (this.GetComponent<SpriteRenderer>() == null) {print("nocab SR is null");}
      if (this.GetComponent<SpriteRenderer>().color == null) {print("nocab color is null");}
      if (c == null) {print("nocab other color is null");}
      this.sprite.color = c;
    }

    public void OnMouseUp() {
      Debug.Log("Nocab flag 1 model q, r  " + modelTile.q + "  " + modelTile.r);
    }

}
