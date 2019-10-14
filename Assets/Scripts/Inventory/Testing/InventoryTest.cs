using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryTest : MonoBehaviour {


    public Sprite testSprite;
    public InventoryController ic;


    public void Start() {
        this.GetComponent<Button>().onClick.AddListener(delegate { this.onClick(); });
    }


    public void onClick() {

        SimpleInventoryItem newI = InventoryItemFactory.factory.buildSimpleItemTest();
        if (newI == null) { Debug.Log("Nocab flag -1"); }
        ic.addItem(newI);


    }





}
