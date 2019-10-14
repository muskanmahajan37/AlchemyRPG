using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler {
    /**
     * Represents a single slot in an inventory that can hold and display items
     */

    private InventoryItem item = null;
    
    public void OnDrop (PointerEventData eventData) {
        // Somethign was draged & Droped into this slot
        if (item != null) {
            // If we're free then adopt the item currently being draged
            InventoryItem newChild = InventoryItemDragHandler.itemBeingDraged;
            fill(newChild);
		}
	}

    
    public void fill(InventoryItem item) {
        if (this.item != null) {
            string message = "Inventory slot already full => can't add a new item onto it.";
            message += "\nAttempting to add: " + item;
            message += "\nAlreay have: " + this.item;
            throw new System.Exception(message);
        }
        
        this.item = item;
        this.updateSprite(item.getSprite());
    }

    public void updateSprite(Sprite newS) {
        Debug.Log("Nocab flag 4"); 
        if (newS == null) { Debug.Log("Nocab fail"); }
        this.GetComponent<Image>().sprite = newS;
    }


    public bool isOccupied() { return ! isEmpty(); }
    public bool isEmpty() { return item == null; }
}
