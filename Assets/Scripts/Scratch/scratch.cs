using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scratch : MonoBehaviour {


    public SpriteRenderer sr;


    public void Start() {
        test();
    }

    public void nocabTest() {
        Vector2Int start = new Vector2Int(16, 16);
        Vector2Int end = new Vector2Int(15, 15);
        Vector2Int otherEnd = new Vector2Int(13, 6);
        Texture2D tex = new Texture2D(32, 32);
        PixelDrawer.MidpointCircleAlgorithim(tex, start, 10, Color.black);
        //PixelDrawer.BresenhamDownRight(tex, start, new Vector2Int(15,8), Color.black);

        tex.filterMode = FilterMode.Point;
        tex.Apply(); // Expensive :(
        sr.sprite = PixelDrawer.packTexture(tex);
    }



    public void test() {
        IFontCharacter c = FontCharacterFactory.buildBoundedCharacter(12, 12);

        IPenDownScribble ur = ScribbleFactory.buildRightHookScribble(true, true, 4);
        IPenDownScribble ul = ScribbleFactory.buildRightHookScribble(true, false, 4);
        //c.addScribble(ur);
        //c.addScribble(ul);

        Texture2D nocabTex = new Texture2D(32, 32);
        c.draw(nocabTex, new Vector2Int(16, 16));

        nocabTex.filterMode = FilterMode.Point;
        nocabTex.Apply();
        sr.sprite = PixelDrawer.packTexture(nocabTex);
    }

}
