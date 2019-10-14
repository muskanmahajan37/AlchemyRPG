using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour {

    public InventorySlot slotPrefab;
    public int initialSize = 16;

    private float minHeight; // The minimum height this container can have. 
                             // Used to ensure the scroll view doesn't do strange things with small inventories
                             // Value is generated at Start() time and shouldn't be changed
    private List<InventorySlot> childSlots;

    // Used for resizings
    private GridLayoutGroup _GridLayout;
    private RectTransform _MyRectTrans;

    // Start is called before the first frame update
    void Start() {
        childSlots = new List<InventorySlot>(initialSize);

        this._GridLayout = this.GetComponent<GridLayoutGroup>();
        if (this._GridLayout == null) { Debug.Log("Nocab flag fail"); }
        this._MyRectTrans = this.GetComponent<RectTransform>();

        // Find the min height required to fill the entire inventory scroller
        this.GetComponent<ScaleToParentSize>().scale();
        this.minHeight = _MyRectTrans.sizeDelta.y;

        for (int i = 0; i < initialSize; i++) {
            buildNewSlot();
        }
    }
    
    private InventorySlot buildNewSlot() {
        // Build a new slot and add it to the childSlots list.
        // Returns a new, set up Inventory Slot ready for a new item.
        
        // TODO: impliemnt max inventory size
        // TODO: Efficent dynamic inventory sizing
        
        InventorySlot newSlot = GameObject.Instantiate<InventorySlot>(slotPrefab);
        newSlot.transform.SetParent(this.transform);
        childSlots.Add(newSlot);

        // Update the size of this pannel container
        // Needed to get the scroll functions to work
        // TODO: Make this more efficent instead of every time we add a slot
        updateVerticalSize();

        return newSlot;
    }

    #region Size Adjustment

    private void updateVerticalSize() {
        int idealRowCount = this.currentRowCount();
        float cellHeight = _GridLayout.cellSize.y;
        float cellPaddingY = _GridLayout.spacing.y;
        float idealHeight = idealRowCount * (cellHeight + cellPaddingY) - cellPaddingY;
        float newHeight = Mathf.Max(idealHeight, minHeight);

        Vector2 size = this._MyRectTrans.sizeDelta;
        size.y = newHeight;
        _MyRectTrans.sizeDelta = size;
    }

    private int currentRowCount() {
        // Mathmagic from stack overflow for integer division that rounds up
        // Note, this may have strange behavior if childSlots.count == 0
        // Otherwise it's equiv to Math.Ceil(childSlots.count / maxSlotsPerRow());
        return ((childSlots.Count - 1) / maxSlotsPerRow()) + 1;  
    }
    

    private int maxSlotsPerRow() {
        //((cellWidth + padding) * n) - padding = this.width
        // n = (this.width + paddig) / (cellWidth + padding)
        // Explination: n grid cells cells can fit into this width. 
        // The actual size of a grid cell is (cellWidth + padding)
        // But unit UI is smart enough to not count the padding on the last cell in a row
        // so we subtract it back out, re-arrange and solve for n
        
        float cellWidth = _GridLayout.cellSize.x;
        float cellPaddingX = _GridLayout.spacing.x;
        float myWidth = _MyRectTrans.sizeDelta.x;
        return (int)((myWidth + cellPaddingX) / (cellWidth + cellPaddingX));
    }

    #endregion

    public void addItem(InventoryItem newI) {

        // TODO: Consider a hashSet or some kind of priority queue for 
        //       "Open" slots instead of looping through all children

        foreach(InventorySlot s in this.childSlots) {
            if (s.isEmpty()) {
                s.fill(newI);
                return;
            }
        }

        // else every item slot if full
        buildNewSlot().fill(newI);
    }
    
}
