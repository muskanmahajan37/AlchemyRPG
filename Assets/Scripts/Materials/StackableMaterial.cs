using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackableMaterial : UniqueMaterial {
    /**
     * A stackable material is a more compact version of a unique material. 
     * These types of materials can "collapse" together into one
     * invintory slot. Generally stackable materials are common
     * and only are used in large quantities in recipes. 
     * 
     * Stackable materials are uniquely identified by it's _materialName (string) field
     * 
     * Two instances of a StackableMaterial object with the same identifer can exist.
     */
     
    private int _stackCount;
    public int stackCount { get { return _stackCount; } }

    #region Constructors

    public StackableMaterial(string materialName) : base(materialName) {
        this._stackCount = 0;
    }

    public IMaterial clone() {
        // Cloning a stackable material produces a material of the same type but 
        // empty stack size. 
        return new StackableMaterial(materialName);
    }

    #endregion

    public void addToStack(int count) {
        if (count < 0) { throw new System.Exception("Can't add negative count to a stackable material. Count: " + count); }

        _stackCount += count;
    }

    public int removeFromStack(int count) {
        /**
         * Attempt to remove count number of materials from this stack.
         * The number returned is the actual amount of materials removed,
         * the only time this result is NOT == to the requested amount is
         * when there isn't enough resources to fufill the order.
         * 
         * If this object doesn't have enough stackCount to fufill the order
         * then all of the _stackCount is removed and that size is returned.
         */
        if (count < 0) { count = Mathf.Abs(count); }

        if (_stackCount < count) {
            // If trying to remove more than we currently have
            count = _stackCount;
        }
        _stackCount -= count;
        return count;
    }

}
