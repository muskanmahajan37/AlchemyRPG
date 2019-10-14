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


public abstract class ANocabNameable : INocabNameable{

    private readonly string NocabName;

    public ANocabNameable(object objectToRegister) {
        this.NocabName = System.Guid.NewGuid().ToString();
        this.register(objectToRegister);
    }

    public ANocabNameable(string name, object objectToRegister) {
        this.NocabName = name;
        this.register(objectToRegister);
    }

    public void register(object objToRegister) { CentralRegistry.register(NocabName, objToRegister); }
    public bool deregister()     { return CentralRegistry.deregister(NocabName); }
    public string getNocabName() { return this.NocabName; }

}
