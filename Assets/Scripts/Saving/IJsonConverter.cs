using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightJson;
using System;


public interface JsonConvertable {
    /**
     * A JsonConvertable object is an object that can convert itself into a JsonObject,
     * and ingest a well-formatted JsonObject and update itself to match.
     * 
     * In other words, the JsonConvertable interface allows objects to save and load themselves.
     * 
     * In general, the output of toJson() should capture 100% of the state stored in the object.
     * The format of the outputed JsonObject from toJson(), should be valid input for the loadJson(...)
     * function. That is to say, this.loadJson(this.toJson()) should produce no errors and result
     * in no state changes (a no-op).
     */
    // TODO: Seperate out the load and save functionality?

    string myType();  // TODO: Do I really need this? 

    JsonObject toJson();

    void loadJson(JsonObject jo);
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

