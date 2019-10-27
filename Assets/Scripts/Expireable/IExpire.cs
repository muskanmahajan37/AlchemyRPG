using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using LightJson;
using System;

public interface IExpire<T> : JsonConvertable
{
    /**
     * NOTE: Old text might now be true: "Whoever calls strength() is responsable for IExpire clean up."
     */


    /**
     * Age this expireable object by 1 time unit. 
     * Returns true if, the object should be expired after the cycle() call. 
     * 
     * It's reccomended cycle is called imediatly AFTER the object's 
     * action has taken place. 
     * 
     * But that is only a style reccomendation.
     */
    bool cycle();

    /**
     * How potent is this expireable object?
     * A strength of 0 (or less) means this object is expired
     * 
     * Reccomended to return the number of turns remaining for this pipe,
     * or the power assocaited with it's internal pump() function.
     */
    int strength();


    // The callback takes a parameter of type T
    // The NocabName field is a uuid string that allows for the lookup of
    // the callback's owner in the CentralRegistry. When in doubt, use the
    // ANocabNameable class to generate a
    void addCallback(string NocabName, Action<T> callback);

    // TODO: Should this really be a public function? 
    void runCallbacks(T arg);
}
