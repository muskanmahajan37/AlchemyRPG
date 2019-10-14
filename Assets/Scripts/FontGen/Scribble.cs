using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScribble {
    // Every scribble can draw itself
    // An IScribble is (generally) designed to start at 0,0
    // The mark(...) function specifies where the mark should start to be drawn

    void draw(Texture2D tex, Vector2Int offset);


    // The "bound" is the maximum coordinate from (0, 0)
    // For example, a cirlce centered at (0,0) has 
    //   roundBound() = +r  (largest x value)
    //   leftBound()  = -r  (smallest x value)
    //   upBound()    = +r  (largest y value)
    //   downBount()  = -r  (smalest y value)
    int rightBound();
    int leftBound();
    int upBound();
    int downBound();


    void translate(int x, int y);
}

public interface IPenDownScribble : IScribble {

    Vector2Int lastPoint();
}

public class LineScribble : IPenDownScribble {

    private Vector2Int start;
    private Vector2Int end;

    public LineScribble(Vector2Int start, Vector2Int end) {
        this.start = start;
        this.end = end;
    }

    public void draw(Texture2D tex, Vector2Int offset) {
        int x = offset.x;
        int y = offset.y;
        Vector2Int actualStart = new Vector2Int(start.x + x, start.y + y);
        Vector2Int actualEnd =   new Vector2Int(end.x   + x, end.y   + y);
        PixelDrawer.DrawLine(tex, actualStart, actualEnd, Color.black);
    }

    public Vector2Int lastPoint() {
        return this.end;
    }


    public void translate(int x, int y) {
        start.x += x;
        start.y += y;

        end.x += x;
        end.y += y;
    }

    public int rightBound() { return Mathf.Max(start.x, end.x); }
    public int leftBound()  { return Mathf.Min(start.x, end.x); }
    public int upBound()    { return Mathf.Max(start.y, end.y); }
    public int downBound()  { return Mathf.Min(start.y, end.y); }

}



public class MultiLineScribble : IPenDownScribble {

    List<Vector2Int> points; // Must have at least 2 points 

    public MultiLineScribble(List<Vector2Int> points) {
        if (points.Count <= 1) {
            throw new System.Exception("Not enough points for a multi line scribble. Point count: " + points.Count);
        }
        this.points = points;
    }

    public void draw(Texture2D tex, Vector2Int offset) {
        for (int i = 1; i < points.Count; i++) {
            Vector2Int startPoint = points[i - 1];
            Vector2Int endPoint = points[i];
            
            int x = offset.x;
            int y = offset.y;
            Vector2Int actualStart = new Vector2Int(startPoint.x + x, startPoint.y + y);
            Vector2Int actualEnd = new Vector2Int(endPoint.x + x, endPoint.y + y);

            PixelDrawer.DrawLine(tex, actualStart, actualEnd, Color.black);
        }
    }

    public Vector2Int lastPoint() {
        return points[points.Count - 1];
    }

    public void translate(int x, int y) {
        for (int i = 0; i < points.Count; i++) {
            Vector2Int oldPoint = points[i];
            points[i] = new Vector2Int(oldPoint.x + x, oldPoint.y + y);
        }
    }
    
    public int rightBound() {
        int result = int.MinValue;
        foreach(Vector2Int p in points) { result = Mathf.Max(p.x, result); }
        return result;
    }
    public int leftBound() {
        int result = int.MaxValue;
        foreach (Vector2Int p in points) { result = Mathf.Min(p.x, result); }
        return result;
    }
    public int upBound() {
        int result = int.MinValue;
        foreach (Vector2Int p in points) { result = Mathf.Max(p.y, result); }
        return result;
    }
    public int downBound() { 
        int result = int.MaxValue;
        foreach (Vector2Int p in points) { result = Mathf.Min(p.y, result); }
        return result;
    }
}
