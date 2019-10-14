using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMaterial {
    /**
     * A material is a thing that is used to build items
     * things like ingredients, or wood, or magic energy etc.
     * 
     * Materials are generally harvested from somewhere and then 
     * refined into a suable object/ item. 
     */


    IMaterial clone(); // Create a copy of the material, usually this is used by MaterialNodes to produce more IMaterial objs
                       // Generally, this is a shallow and "empty" copy, simply keeping the resource identifier and underlying class
                       // The result can then be filled later

}
