using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    /**
     * Main logic/ math associated with moving arround Inventory Item abstract class objects
     * 
     * Each InventoryItem should have this attached to it
     * TODO: Merge this class with InventoryItem class
     */


	// Only let the user drag 1 item at a time
	public static InventoryItem itemBeingDraged;
	public static Vector2 startPos;
	public static Transform startParent;

	// Gets run when you start draging a thing
	public void OnBeginDrag (PointerEventData eventData) {
		itemBeingDraged = gameObject.GetComponent<InventoryItem>();  //TODO: This should only be attached to an InventoryItem object
		startPos = transform.position;
		startParent = transform.parent;
		GetComponent<CanvasGroup> ().blocksRaycasts = false;
	}

	// Gets run every frame while the the mouse is heald down/ draging a thing
	public void OnDrag (PointerEventData eventData) {
		transform.position = Input.mousePosition;
	}

	// Run when the drag is released but not released over a new slot.
	public void OnEndDrag (PointerEventData eventData) {
		itemBeingDraged = null;
		GetComponent<CanvasGroup> ().blocksRaycasts = true;
		transform.position = startPos;
	}

}
