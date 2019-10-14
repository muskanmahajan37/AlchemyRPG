using System.Collections;
using System.Collections.Generic;
using LightJson;
using UnityEngine;

public static class CryoRegistry  {
    /**
     * 
     */


    private static HashSet<string> frozenObjects; // A list of names of all objects that are written to disk
    private static Dictionary<string, CryoLink> allAwakeObjects; // A mapping of Cryoable UUID -> Actual object

    private static Dictionary<string, List<string>> allMessages; // A store of all messages to be sent. string Key = message reciever
    private static Dictionary<string, List<CryoLink>> allReunions; // A store of all things that are trying to match their link



    public static void sendWakeUpMessage(string targetId, string message) {
        // Send a message to a target ID once the target ID has 
        // been revived from cryo-sleep (IE, once the target has been
        // loaded into a real object)

        if ( ! allMessages.ContainsKey(targetId))
            { allMessages[targetId] = new List<string>(); }

        allMessages[targetId].Add(message);
    }

    public static void allAwake() {
        // The load master should call the allAwake() funciton
        // once all objects have been loaded, then the 
        // CryoRegistry will send off all the stored messages
        // and each Cryoable is responsible for reuniting themselves


        foreach(KeyValuePair<string, List<string>> kvp in allMessages) {
            CryoLink reciever = allAwakeObjects[kvp.Key]; // Convert cryoUUID into object
            List<string> messageStore = kvp.Value;
            foreach(string message in messageStore) {
                reciever.recieveMessage(message);
            }
        }
    }

    /*
    public static void registerForSleep(Cryoable sleepyObject) {
        // Any Cryoable object should check itself in before it 
        // gets written to disk.

        string uuid = sleepyObject.cryoUUID();
        if (frozenObjects.Contains(uuid)) {
            Debug.Log("Registering an object that did not have a unique UUID (uuid was already registered): " + uuid);
        }

        frozenObjects.Add(uuid);
    }
    */

    public static void registerForAwake(CryoLink groggyObject) {
        // Any Cryoable object should notify this CryoRegistry
        // that it has woken up succesfully. 

        string uuid = groggyObject.cryoUUID();
        if (allAwakeObjects.ContainsKey(uuid)) {
            Debug.Log("Registering an object that did not have a UUID (uuid was already registered): " + uuid);
        }

        allAwakeObjects[uuid] = groggyObject;
    }

}
