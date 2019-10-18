using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INocabNameable {
    /*
     * Once a NocabNameable has been named, it can never change.
     * The NocabName is (read "should be") constant even through 
     * save and load events. 
     * 
     * If a Nocabnameable entity/ object "dies" or needs to be 
     * destroyed the deregister() function must be called (if not,
     * then memory leaks will happen because at least the CentralRegistry
     * will hold one refrence to this INocabNameable)
     */


    string getNocabName();

    bool deregister();

}


public class NocabNameable : INocabNameable{

    private readonly string NocabName;

    public NocabNameable(object objectToRegister) {
        this.NocabName = System.Guid.NewGuid().ToString();
        this.register(objectToRegister);
    }

    public NocabNameable(string name, object objectToRegister) {
        this.NocabName = name;
        this.register(objectToRegister);
    }

    public void register(object objToRegister) { CentralRegistry.register(NocabName, objToRegister); }
    public bool deregister()     { return CentralRegistry.deregister(NocabName); }
    public string getNocabName() { return this.NocabName; }

}


// TODO: Do I ever even need this? 
public class LazyNocabNamable : INocabNameable {
    /**
     * A LazyNocabNamable object is a INocabNamable that dose NOT register itself
     * with the CentralRegistry at construction time.
     * 
     * Any down stream object that has-a LazyNocabNamable must call the register(obj)
     * fuction at the down stream object's construction/ start time. 
     */


    private string NocabName;
    private bool isRegistered;

    public LazyNocabNamable() { this.isRegistered = false; }

    public LazyNocabNamable(string name) {
        this.NocabName = name;
        this.isRegistered = false;
    }

    public string setNocabName(string newName) {
        if (this.isRegistered) {
            string errMsg = "Error, LazyNocabNamable can't change its name once set.";
            errMsg += "\n Current actual name: " + NocabName;
            errMsg += "\n newName: " + newName;
            Debug.LogError(errMsg);
            return this.NocabName;
        }
        this.NocabName = newName;
        return this.NocabName;
    }

    public string getNocabName() {
        return this.NocabName;
    }

    public bool register(object obj) {
        this.isRegistered = CentralRegistry.register(this.NocabName, obj);
        return this.isRegistered;
    }

    public bool deregister() {
        if ( ! this.isRegistered) {
            string errMsg = "Error, NocabNamable can't deregister itself if it's never been registerd";
            Debug.LogError(errMsg);
            return false;
        }
        return CentralRegistry.deregister(this.NocabName);
    }
}
