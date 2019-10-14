using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleToParentSize : MonoBehaviour {
    /**
     * Scale the ui object so it's a relative size comapred to the 
     * parent ui object
     */

    public RectTransform targetOverride; // Default is parent transform
    public float xScale = 1.0f;
    public float yScale = 1.0f;

    void Start() {
        if (targetOverride == null)
            { targetOverride = this.GetComponentInParent<RectTransform>(); }
        scale();
    }
    
    public void scale() {
        float targetXSize = targetOverride.rect.width;
        float targetYSize = targetOverride.rect.height;

        RectTransform myRectTrans = this.GetComponent<RectTransform>();
        float newXSize = targetXSize * xScale;
        float newYSize = targetYSize * yScale;
        myRectTrans.sizeDelta = new Vector2(newXSize, newYSize);
    }

    public void scale(float xScale, float yScale) {
        this.xScale = xScale;
        this.yScale = yScale;
        scale();
    }

    public void scale(float xScale, float yScale, RectTransform targetOverride) {
        this.targetOverride = targetOverride;
        scale(xScale, yScale);
    }
}
