using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CentralRegistry {

    private static Dictionary<string, object> centralDictionary = new Dictionary<string, object>();

    public static bool register(string newObjNocabName, object newObj) {
        if (centralDictionary.ContainsKey(newObjNocabName)) {
            Debug.LogError("CentralRegistry is trying to register an object but it's NocabName has already been registered!: Name: " + newObjNocabName);
            return false;
        }

        centralDictionary.Add(newObjNocabName, newObj);
        return true;
    }

    public static bool deregister(string targetNocabName) {
        // TODO: Consider making a deregister function that takes in string NocabName 
        if ( ! centralDictionary.ContainsKey(targetNocabName)) {
            Debug.LogError("CentralRegistry is trying to De-Register an object, but it's NocabName is not registered! Name: " + targetNocabName);
            return false;
        }
        return centralDictionary.Remove(targetNocabName);
    }



    public static object getObject(string nocabName) {
        if ( ! centralDictionary.ContainsKey(nocabName)) { return null; }
        return centralDictionary[nocabName];
    }

}
