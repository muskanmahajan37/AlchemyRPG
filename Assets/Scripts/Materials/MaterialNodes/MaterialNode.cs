using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialNode {
    /**
     * A material node is a thing that produces materials.
     * Usually a physical location or object that can be "harvested" or 
     * otherwise collected. 
     */

    private IMaterial outputMaterial;

    public MaterialNode(IMaterial outputMaterial) {
        this.outputMaterial = outputMaterial;
    }


    public IMaterial harvest() {
        return outputMaterial.clone();
    }

}

/**
 * 
 * TODO: Nodes that output a range of things
 *       Nodes that output a stackable resource but variations on the amount
 *       Nodes that output many resources
 *       Nodes that output a mix of resources
 * 
 * 
 * 
 */


