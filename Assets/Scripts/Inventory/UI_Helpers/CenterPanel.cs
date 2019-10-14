using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterPanel : MonoBehaviour {
    /**
     * Puts the UI object in the center of the parent ui object
     */

    void Start() {
        center();
    }


    public void center() {
        RectTransform parentTrans = this.transform.parent.GetComponent<RectTransform>();
        Vector2 targetCenter = parentTrans.position;
        this.transform.position = targetCenter;
    }
}
