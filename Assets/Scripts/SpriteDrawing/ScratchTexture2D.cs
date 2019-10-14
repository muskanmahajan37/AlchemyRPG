using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScratchTexture2D : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //this.updateSpriteRandon();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateSpriteRandon()
    {
        SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
        sr.sprite = this.drawCharacter(PenDownFontUtility.newRandShape(3), new Vector2Int(7, 7));
        this.transform.localScale = new Vector3(6, 6, 1);
        Texture2D lineTex = new Texture2D(32, 32);
        drawLineLow(lineTex, new Vector2Int(10,10), new Vector2Int(20, 11));
        sr.sprite = packTexture(lineTex);
    }

    private Sprite testNewSprite()
    {
        Texture2D newTex = new Texture2D(32, 32);

        for(int y = 0; y < newTex.height; y++) {
            for (int x = 0; x < newTex.width; x++) {
                if (y == x)
                {
                    newTex.SetPixel(x, y, Color.black);
                    // 0,0 is in the bottom left
                }
                //newTex.SetPixel(x, y, new Color(y/16f, x/16f, (y+x)/32f));
            }
        }
        newTex.filterMode = FilterMode.Point;
        //newTex.wrapMode = TextureWrapMode.Clamp;
        newTex.Apply(); // NOTE Apply function actually uploads the change pixes to hardware => very expensive

        Rect rect = new Rect(0, 0, newTex.width, newTex.height);
        Vector2 piviot = new Vector2(0.5f, 0.5f); // The "center" of the new sprite is the center of the rect box
        Sprite newSprite = Sprite.Create(newTex, rect, piviot);
        return newSprite;
    }


    private static readonly int unitLen = 6;

    public Sprite drawCharacter(PenDownCrdinalFontShape shape, Vector2Int offset) {
        // TODO: We're currently assuming that shape contains one point at(0,0)
        // TODO: Multiplying by unitLen can be code recycling/ reduction

        Texture2D newTex = new Texture2D(64, 64);

        // Draw start point
        drawPointAndDirection(newTex, shape.getStartPoint, offset);

        foreach (CardinalFontPoint p in shape.getPoints) {
            drawPointAndDirection(newTex, p, offset);
        }

        // Draw end point
        drawPointAndDirection(newTex, shape.getEndPoint, offset);


        newTex.filterMode = FilterMode.Point;
        newTex.Apply();

        Rect rect = new Rect(0, 0, newTex.width, newTex.height);
        Vector2 piviot = new Vector2(0.5f, 0.5f);
        return Sprite.Create(newTex, rect, piviot);
    }

    private static void drawPointAndDirection(Texture2D texture, CardinalFontPoint point, Vector2Int offset) {
        // NOTE: offset is applied befor unitLen multiplication => offset of (1,1) actually translates the point up unitLen and over unitLen 
        int xPos = (point.pos.x + offset.x) * unitLen;
        int yPos = (point.pos.y + offset.y) * unitLen;

        texture.SetPixel(xPos, yPos, Color.black);
        if (point.rightFull) {
            // TODO: a draw line function for squares
            for (int i = 0; i < unitLen; i ++) {
                texture.SetPixel(xPos + (i + 1), yPos, Color.black);
            }
        }
        if (point.downFull)  {
            for (int i = 0; i < unitLen; i++) {
                texture.SetPixel(xPos, yPos - (i + 1), Color.black);
            }
        }
        if (point.leftFull) {
            for (int i = 0; i < unitLen; i++) {
                texture.SetPixel(xPos - (i + 1), yPos, Color.black);
            }
        }
        if (point.upFull) {
            for (int i = 0; i < unitLen; i++) {
                texture.SetPixel(xPos, yPos + (i + 1), Color.black);
            }
        }

    }

    private static void drawHalfCircle(Texture2D texture) {

    }

    private static Sprite packTexture(Texture2D texture) {
        Rect rect = new Rect(0, 0, texture.width, texture.height);
        Vector2 piviot = new Vector2(0.5f, 0.5f);
        return Sprite.Create(texture, rect, piviot);
    }

    private static void drawLineLow(Texture2D texture, Vector2Int p1, Vector2Int p2) {
        int deltaX = p2.x - p1.x;
        int deltaY = p2.y - p1.y;

        int plotY = p1.y;
        int eps = 0;

        for (int plotX = p1.x; plotX <= p2.x; plotX++) {
            texture.SetPixel(plotX, plotY, Color.black);
            eps += deltaY;
            if ((eps << 1) >= deltaX) {
                plotY++;
                eps -= deltaX;
            }
        }
        texture.filterMode = FilterMode.Point;
        texture.Apply(); // Expensive :(
    }
}
