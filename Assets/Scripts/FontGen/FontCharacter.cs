using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IFontCharacter {
    
    void draw(Texture2D tex, Vector2Int offset);

    // TODO: Save a IFontCharacter by hashing it into a unique number
    //      perhapse store it as an array of booleans?

}

public class PenDownCharacter : IFontCharacter {

    List<IPenDownScribble> scribbles;

    public PenDownCharacter() {
        this.scribbles = new List<IPenDownScribble>();
    }

    public PenDownCharacter(List<IPenDownScribble> scribbles) {
        this.scribbles = new List<IPenDownScribble>();

        foreach (IPenDownScribble s in scribbles) { addScribble(s); }
    }


    public void addScribble(IPenDownScribble newS) {
        // Append the provided scribble to the end of this character
        this.scribbles.Add(newS);
    }

    public void draw(Texture2D tex, Vector2Int drawPos) {
        foreach(IPenDownScribble scribble in scribbles) {
            // Draw the scribble at the designated draw position AND with at the 
            // end of the last scribble
            scribble.draw(tex, drawPos);
            
            // The next scribble starts at this scribble's end
            drawPos += scribble.lastPoint();
        }
    }
}
