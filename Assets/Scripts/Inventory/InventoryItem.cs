using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryItem {
    /**
     * An Inventory Item is a thing that represents an object/ item and can
     * be drawn onto a Unity UI framework (namely, the inventory screen)
     */

    abstract public Sprite getSprite();
}
