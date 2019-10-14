using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightJson;
using System;


public interface JsonConvertable {
    // TODO: Seperate out the load and save functionality?

    string myType();  // TODO: Do I really need this? 

    JsonObject toJson();

    void fromJson(JsonObject jo);
}

public class InvalidLoadType : Exception {
    /**
     * This error is generally thrown when something has gone wrong 
     * when loading an object. As the name implies, it's most only 
     * thrown when converting from a JsonObject to a real object, but the
     * JsonObject is not the correct type. 
     * 
     * TODO: Make a unique Error Type InvalidLoad
     */

    public InvalidLoadType() : base() { }
    public InvalidLoadType(string message) : base(message) { }
    public InvalidLoadType(string message, Exception innerException) : base(message, innerException) { }

}

public class InvalidSave : Exception {
    /**
     * This error is generally thrown when the object that is trying to be saved 
     * is in an invalid state and therefore cannot be saved. 
     */

    public InvalidSave() : base() { }
    public InvalidSave(string message) : base(message) { }
    public InvalidSave(string message, Exception innerException) : base(message, innerException) { }

}

