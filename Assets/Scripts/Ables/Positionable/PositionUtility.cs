using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PositionUtility : IPositionable {
    public Vector2 pos;

    public float x {
        get { return pos.x; }
        set { this.pos.x = value; }
    }

    public float y {
        get { return pos.y; }
        set { this.pos.y = value; }
    }

    public PositionUtility(Vector2 position) {
        this.pos = position;
    }

    public PositionUtility(float x, float y) : this(new Vector2(x, y)) { }

    public Vector2 position() { return this.pos; }

    public PositionUtility getPositionUtility() { return this; }


}