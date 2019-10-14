using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Runtime.InteropServices;

public class FontGenScratch
{



}



// TODO Make this generic type
public class PenDownCrdinalFontShape
{
    // TODO: Make this readonly
    // This set does NOT contain the start or end points. 
    private HashSet<CardinalFontPoint> points;
    public HashSet<CardinalFontPoint> getPoints { get { return this.points; } }

    private CardinalFontPoint startPoint;  // The pen down location of this shape
    public CardinalFontPoint getStartPoint { get { return this.startPoint; } }

    private CardinalFontPoint endPoint;    // The pen up location of this shape
    public CardinalFontPoint getEndPoint { get { return this.endPoint; } }

    public PenDownCrdinalFontShape(HashSet<CardinalFontPoint> points, CardinalFontPoint startPoint, CardinalFontPoint endPoint)
    {
        this.points = points;
        this.points.Remove(startPoint);
        this.points.Remove(endPoint);

        this.startPoint = startPoint;
        this.endPoint = endPoint;
    }

    public PenDownCrdinalFontShape combine(PenDownCrdinalFontShape otherShape) {
        // This will automatically offset the provided otherShape to start exactly where this shape ends
        // Add the start of the other shape to end of this shape
        Vector2Int offset = new Vector2Int(
            this.getEndPoint.pos.x - otherShape.getStartPoint.pos.x,
            this.getEndPoint.pos.y - otherShape.getStartPoint.pos.y);

        foreach(CardinalFontPoint otherPoint in otherShape.getPoints) {
            Vector2Int newPos = new Vector2Int(
                otherPoint.pos.x + offset.x,
                otherPoint.pos.y + offset.y);
            CardinalFontPoint newPoint = new CardinalFontPoint(otherPoint);
            newPoint.pos = newPos;
            this.points.Add(newPoint);
        }

        // NOTE: This.endPoint (old) == (otherShape.startPoint + offset)
        // add the old endpoint to the middle points
        // We do a deep clone here because this.endPoint will change on the next operation
        this.points.Add(new CardinalFontPoint(this.endPoint));
        
        // Update the endpoint of this shape
        Vector2Int newEndPointPos = new Vector2Int(
            otherShape.endPoint.pos.x + offset.x, 
            otherShape.endPoint.pos.y + offset.y);

        this.endPoint = new CardinalFontPoint(newEndPointPos);

        // TODO: Consider error checking here
        //   things like if (this.points.Remove(this.endPoint) => sky falling)
        
        return this;
    }
}

public class CardinalFontShape {
    /**
     * Only describes up right down left
     * All points are unit distance apart
     */
    
    
    // TODO: Make this read only
    protected HashSet<CardinalFontPoint> points;
    public HashSet<CardinalFontPoint> getPoints { get { return points; } }

    public CardinalFontShape(HashSet<CardinalFontPoint> pts) {
        this.points = pts;
    }
    
    
}

public class CardinalFontPoint
{
    // Initialize to empty directions
    private bool[] directions = new bool[4] { false, false, false, false};
    private readonly static int RIGHT = 0;
    private readonly static int DOWN = 1;
    private readonly static int LEFT = 2;
    private readonly static int UP = 3;

    // TODO: find a way to make these values read only after initialization
    public bool rightFull { get { return directions[RIGHT]; } }
    public bool downFull  { get { return directions[DOWN]; } }
    public bool leftFull  { get { return directions[LEFT]; } }
    public bool upFull    { get { return directions[UP]; } }

    public Vector2Int pos = new Vector2Int(0, 0);

    public CardinalFontPoint(Vector2Int pos)
    { this.pos = pos; }

    public CardinalFontPoint(CardinalFontPoint other) {
        this.pos = new Vector2Int(other.pos.x, other.pos.y);
        for (int i = RIGHT; i < UP + 1; i++) {
            this.directions[i] = other.directions[i];
        }
    }
    
    public override bool Equals(object obj) {
        CardinalFontPoint other = obj as CardinalFontPoint;
        if (other == null) {
            return false;
        }
        return (this.pos == other.pos) && 
            this.rightFull == other.rightFull && 
            this.downFull == other.downFull && 
            this.leftFull == other.leftFull && 
            this.upFull == other.upFull;
    }

    public override int GetHashCode() {
        // TODO: TEST THIS
        Debug.Log("WARNING HASH FUNCTION NOT TESTED FOR CardinalFontPoint Objects");
        int hash = 17;
        hash = hash * 23 + this.pos.x;
        hash = hash * 23 + this.pos.y; // Add in the position hash
        for(int i = 0; i < this.directions.Length; i++) {
            // Foreach direction this is pointing
            hash = hash * 23;
            if (this.directions[i]) {
                hash += 1;
            }
        }
        return hash;
    }

    public override string ToString() {
        StringBuilder sb = new StringBuilder();
        sb.Append("(CardinalFontPoint:  pos(");
        sb.Append(this.pos.x);
        sb.Append(", ");
        sb.Append(this.pos.y);
        sb.Append(")  r=");
        sb.Append(this.rightFull);
        sb.Append(" d=");
        sb.Append(this.downFull);
        sb.Append(" l=");
        sb.Append(this.leftFull);
        sb.Append(" u=");
        sb.Append(this.upFull);
        sb.Append("hash=");
        sb.Append(this.GetHashCode());
        sb.Append(")");
        return sb.ToString();
    }

    public CardinalFontPoint pointsRight() {
        this.directions[RIGHT] = true;
        return this;
    }

    public CardinalFontPoint pointsDown() {
        this.directions[DOWN] = true;
        return this;
    }

    public CardinalFontPoint pointsLeft() {
        this.directions[LEFT] = true;
        return this;
    }

    public CardinalFontPoint pointsUp() {
        this.directions[UP] = true;
        return this;
    }
}


public static class CardionalFontValidator {
    
    // TODO: make these all sets instead of lists

    public static bool validate(HashSet<CardinalFontPoint> pointsA, HashSet<CardinalFontPoint> pointsB, Vector2Int offset) {
        //NOTE:
        // RUNTIME O(n*m) based on len of both lists
        foreach (CardinalFontPoint pb in pointsB) {
            if ( ! validate(pointsA, pb, offset)) {
                // Any failed point implies total fail
                return false;
            }
        }
        return true;
    }

    public static bool validate(HashSet<CardinalFontPoint> points, CardinalFontPoint otherPoint, Vector2Int offset) {
        // NOTE:
        //  RUNTIME O(n) based on len of points
        foreach (CardinalFontPoint p in points) {
            if ( ! validate(p, otherPoint, offset)) {
                // Any failed point implies total fail
                return false;
            }
        }
        return true;
    }

    public static bool validate(CardinalFontPoint pointA, CardinalFontPoint pointB, Vector2Int offset) {
        // NOTE: offset is applied as a transformation to pointB
        return validate(pointA, pointB, offset.x, offset.y);
    }

    public static bool validate(CardinalFontPoint pointA, CardinalFontPoint pointB, float offsetX = 0.0f, float offsetY = 0.0f)
    {
        // NOTE: the offset is applied as a transformation to pointB

        // Two points are NOT valid <=> they have the same position AND directions overlap
        // Logically equivelent: 
        //     * different positions => valid
        //     * same pos with different directions => valid
        
        bool samePos = Mathf.Approximately((pointB.pos.x + offsetX), pointA.pos.x);
        samePos &=     Mathf.Approximately((pointB.pos.y + offsetY), pointA.pos.y);
        if ( ! samePos) {
            // If the two points are NOT aproximatly the same position
            return true;
        }
        // Check for direction overlap
        return (pointA.rightFull && pointB.rightFull) ||
            (pointA.downFull && pointB.downFull) ||
            (pointA.leftFull && pointB.leftFull) ||
            (pointA.upFull && pointB.upFull);

    }

}



public static class PenDownFontUtility {
    public static PenDownCrdinalFontShape newUpRight()
    {
        /**
         * (0,0) U=T
         * (0,1) D=T R=T
         * (1,1) L=T
         */
        CardinalFontPoint p1 = new CardinalFontPoint(new Vector2Int(0, 0));
        p1.pointsUp();

        CardinalFontPoint p2 = new CardinalFontPoint(new Vector2Int(0, 1));
        p2.pointsDown().pointsRight();

        CardinalFontPoint p3 = new CardinalFontPoint(new Vector2Int(1, 1));
        p3.pointsLeft();

        return new PenDownCrdinalFontShape(new HashSet<CardinalFontPoint> { p2 }, p1, p3);
    }

    public static PenDownCrdinalFontShape newUpLeft()
    {
        /**
         * (0,0) U=T
         * (0,1) D=T L=T
         * (-1,1) R=T
         */
        CardinalFontPoint p1 = new CardinalFontPoint(new Vector2Int(0, 0));
        p1.pointsUp();

        CardinalFontPoint p2 = new CardinalFontPoint(new Vector2Int(0, 1));
        p2.pointsDown().pointsLeft();

        CardinalFontPoint p3 = new CardinalFontPoint(new Vector2Int(-1, 1));
        p3.pointsRight();

        return new PenDownCrdinalFontShape(new HashSet<CardinalFontPoint> { p2 }, p1, p3);
    }

    public static PenDownCrdinalFontShape newDownRight()
    {
        /**
         * (0,0)  D=T
         * (0,-1) U=T R=T
         * (1,-1) L=T
         */
        CardinalFontPoint p1 = new CardinalFontPoint(new Vector2Int(0, 0));
        p1.pointsDown();

        CardinalFontPoint p2 = new CardinalFontPoint(new Vector2Int(0, -1));
        p2.pointsUp().pointsRight();

        CardinalFontPoint p3 = new CardinalFontPoint(new Vector2Int(1, -1));
        p3.pointsLeft();

        return new PenDownCrdinalFontShape(new HashSet<CardinalFontPoint> { p2 }, p1, p3);
    }

    public static PenDownCrdinalFontShape newDownLeft()
    {
        /**
         * (0,0)   D=T
         * (0,-1)  U=T L=T
         * (-1,-1) R=T
         */
        CardinalFontPoint p1 = new CardinalFontPoint(new Vector2Int(0, 0));
        p1.pointsDown();

        CardinalFontPoint p2 = new CardinalFontPoint(new Vector2Int(0, -1));
        p2.pointsUp().pointsLeft();

        CardinalFontPoint p3 = new CardinalFontPoint(new Vector2Int(-1, -1));
        p3.pointsRight();

        return new PenDownCrdinalFontShape(new HashSet<CardinalFontPoint> { p2 }, p1, p3);
    }

    public static PenDownCrdinalFontShape newRandShape()
    {
        float randomNumber = Random.value;
        if (randomNumber < 0.25)
        { return newUpLeft(); }
        if (randomNumber < 0.5)
        { return newUpRight(); }
        if (randomNumber < 0.75)
        { return newDownLeft(); }
        return newDownRight();
    }

    public static PenDownCrdinalFontShape newRandShape(int ply) {
        // ply is the number of shapes you want to add together
        // 1 ply => generate a random base shape
        // 2 ply => connect 2 random  base shapes
        // 3 ply => connect 3 random base shapes
        // etc...

        PenDownCrdinalFontShape resultShape = newRandShape();
        for (int i = 0; i < ply - 1; i++) {
            PenDownCrdinalFontShape newShape = newRandShape();
            Vector2Int offset = resultShape.getEndPoint.pos;
            int tries = 0;
            while ( ! CardionalFontValidator.validate(resultShape.getPoints, newShape.getPoints, offset)) {
                // While the new shape is invalid
                if (tries >= 5) { break; }  // TODO: Fix this, it's bad design
                tries++;
                newShape = newRandShape();
            }
            resultShape.combine(newShape);
        }
        return resultShape;
    }

    public static PenDownCrdinalFontShape temp()
    {
        // Given a new character
        // Mash it into the existing character

        PenDownCrdinalFontShape baseShape = newUpRight();
        PenDownCrdinalFontShape newShape = newRandShape();

        // TODO: Make the offset based on the shape not hardcoded
        Vector2Int offset = new Vector2Int(1, 1);
        int tries = 0;
        while (!CardionalFontValidator.validate(baseShape.getPoints, newShape.getPoints, offset))
        {
            // While the new shape is invalid
            if (tries >= 5) { break; }
            tries++;
            newShape = newRandShape();
        }
        return baseShape.combine(newShape);

    }
}




public static class FontUtility {

    public static CardinalFontShape newUpRight() {
        /**
         * (0,0) U=T
         * (0,1) D=T R=T
         * (1,1) L=T
         */
        CardinalFontPoint p1 = new CardinalFontPoint(new Vector2Int(0,0));
        p1.pointsUp();

        CardinalFontPoint p2 = new CardinalFontPoint(new Vector2Int(0,1));
        p2.pointsDown().pointsRight();
        
        CardinalFontPoint p3 = new CardinalFontPoint(new Vector2Int(1,1));
        p3.pointsLeft();

        return new CardinalFontShape(new HashSet<CardinalFontPoint>() {p1, p2, p3 });
    }

    public static CardinalFontShape newUpLeft() {
        /**
         * (0,0) U=T
         * (0,1) D=T L=T
         * (-1,1) R=T
         */
        CardinalFontPoint p1 = new CardinalFontPoint(new Vector2Int(0, 0));
        p1.pointsUp();

        CardinalFontPoint p2 = new CardinalFontPoint(new Vector2Int(0, 1));
        p2.pointsDown().pointsLeft();

        CardinalFontPoint p3 = new CardinalFontPoint(new Vector2Int(-1, 1));
        p3.pointsRight();

        return new CardinalFontShape(new HashSet<CardinalFontPoint> { p1, p2, p3 });
    }

    public static CardinalFontShape newDownRight() {
        /**
         * (0,0)  D=T
         * (0,-1) U=T R=T
         * (1,-1) L=T
         */
        CardinalFontPoint p1 = new CardinalFontPoint(new Vector2Int(0, 0));
        p1.pointsDown();

        CardinalFontPoint p2 = new CardinalFontPoint(new Vector2Int(0, -1));
        p2.pointsUp().pointsRight();

        CardinalFontPoint p3 = new CardinalFontPoint(new Vector2Int(1, -1));
        p3.pointsLeft();

        return new CardinalFontShape(new HashSet<CardinalFontPoint> { p1, p2, p3 });
    }

    public static CardinalFontShape newDownLeft()
    {
        /**
         * (0,0)   D=T
         * (0,-1)  U=T L=T
         * (-1,-1) R=T
         */
        CardinalFontPoint p1 = new CardinalFontPoint(new Vector2Int(0, 0));
        p1.pointsDown();

        CardinalFontPoint p2 = new CardinalFontPoint(new Vector2Int(0, -1));
        p2.pointsUp().pointsLeft();

        CardinalFontPoint p3 = new CardinalFontPoint(new Vector2Int(-1, -1));
        p3.pointsRight();

        return new CardinalFontShape(new HashSet<CardinalFontPoint> { p1, p2, p3 });
    }

    public static CardinalFontShape newRandShape()
    {
        float randomNumber = Random.value;
        if (randomNumber < 0.25)
            { return newUpLeft(); }
        if (randomNumber < 0.5)
            { return newUpRight(); }
        if (randomNumber < 0.75)
            { return newDownLeft(); }
        return newDownRight();
    }
    


}



