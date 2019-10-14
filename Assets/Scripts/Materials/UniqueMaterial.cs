using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueMaterial : IMaterial {
    /**
     * A material is a thing that can be used to build usable items.
     * 
     */


    public readonly string materialName; // The unique identifier for this material

    #region Constructors

    public UniqueMaterial(string materialName) {
        this.materialName = materialName;
    }

    public IMaterial clone() {
        return new UniqueMaterial(materialName);
    }

    #endregion

    public bool sameMaterial(StackableMaterial other) {
        return this.materialName == other.materialName;
    }


}
