using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * TODO: Merge this into Scripts/CentralRegistry/CentralRegistry.cs
public class CentralRegistry {

    private Dictionary<string, object> allEntities;

    CentralRegistry() {
        allEntities = new Dictionary<string, object>();
    }


    bool checkExists(string targetUUID) {
        return this.allEntities.ContainsKey(targetUUID);
    }

    object getRefrence(string targetUUID) {
        if (!checkExists(targetUUID))
            { return null; }
        return allEntities[targetUUID];
    }


    bool registerSelf(string myUUID, object myself) {
        if (checkExists(myUUID)) {
            // If this uuid is already taken
            throw new System.Exception("Provided uuid is already in use! If you wish to update a centralRegistry uuid please use the updateRegistry(...) function.");
            //return false;
        }
        this.allEntities.Add(myUUID, myself);
        return true;
    }

    bool updateRegistry(string myUUID, object myNewSelf) {
        if (!checkExists(myUUID)) {
            // If this uuid is NOT alrady in use
            throw new System.Exception("Provided uuis is NOT already in use and therefore can't be 'updated'. Please use the RegisterSelf(...) function.");
        }
        this.allEntities[myUUID] = myNewSelf;
        return true;
    }
}
*/