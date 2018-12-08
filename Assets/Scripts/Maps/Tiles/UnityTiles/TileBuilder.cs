using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileBuilder : MonoBehaviour {


    public static GameObject tilePrefab;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    /**
     * 
     * Ok, what does the tile builder need? 
     * 
     * It needs to generate an arbitrary "square" map of tile objects.
     * 
     * What are the data structures?
     * A 2x2 grid => List of lists
     * These lists will be accessed in cartisean plane order:
     *      For example  the result list[x][y] will work
     *      
     * 0,0 is in top left
     * 
     */

    // static List<List<IModelTile>> buildTileGrid(int width, int height, bool full) {
    //     // TODO: Impliment building unfull girds of target height.
    //     // these will be used when making nonsquare maps


    //     List<List<IModelTile>> colHolder = new List<List<IModelTile>>(width);
    //     for (int x = 0; x < width; x++) {
    //         List<IModelTile> col = new List<IModelTile>(height);
    //         for(int y = 0; y < height; y++) {
    //             IModelTile t = new IModelTile(x, y);
    //             col.Add(t);
    //         }
    //         colHolder.Add(col);
    //     }

    //     return colHolder;
    // }

    // public static List<List<ModelTile>> generateModelTielMap(int width, int height, bool full) {
    //     // todo: impliment doing the math to build a non square tile map
    //     return buildTileGrid(width, height, full);
    // }


    public static List<List<GameObject>> objectMap(List<List<GameObject>> modelMap) {
        /**
         * Returns a new object map that parellels the provied model
         */
        int width = modelMap.Count;
        int height = modelMap[0].Count;

        var colHolder = new List<List<GameObject>>(width);
        for (int x = 0; x < width; x++) {
            var col = new List<GameObject>(height);
            for (int y = 0; y < height; y++) {
                GameObject t = Instantiate(tilePrefab);
                col.Add(t);
            }
            colHolder.Add(col);
        }

        return colHolder;
    }
}
