using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemFactory: MonoBehaviour {

    public static InventoryItemFactory factory;
    public Sprite tempSprite;

    public void Start() {
        if (factory == null) { factory = this; }
        else { Destroy(this); }
    }

    public SimpleInventoryItem buildSimpleItemTest() {
        return new SimpleInventoryItem(tempSprite);
    }

}
