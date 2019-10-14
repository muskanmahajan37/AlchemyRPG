using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleInventoryItem : InventoryItem {

    private Sprite _sprite;
    public Sprite sprite { get { return this._sprite; } }

    public SimpleInventoryItem(Sprite sprite) {
        this._sprite = sprite;
    }

    public override Sprite getSprite() { return this._sprite; }  
    
}