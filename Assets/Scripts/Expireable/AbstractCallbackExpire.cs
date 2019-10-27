using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using LightJson;
using System;

public abstract class AbstractCallBackExpire<T> : IExpire<T>
{

    protected List<KeyValuePair<string, Action<T>>> refrencedActions; // A list of <NocabName, Action> callbacks. 
                                                                      // Basically, tag the Actions with the instance refrence they came from
    protected List<KeyValuePair<string, string>> toBeLookedUp; // A mapping of NocabName -> method name to look up instance
                                                               // and method later. Used mostaly after load event


    public AbstractCallBackExpire()
    {
        this.refrencedActions = new List<KeyValuePair<string, Action<T>>>(4); // I don't anticipate more than 4 callbacks

        // NOTE: toBeLookedUp List obj is only instantiated at 'load' time. 
        //       i.e., this object must first be saved to disk, then loaded before
        //       the toBeLookedUp list comes into play
    }

    public AbstractCallBackExpire(string NocabName, Action<T> callback)
    {
        this.refrencedActions = new List<KeyValuePair<string, Action<T>>>(4);
        this.addCallback(NocabName, callback);

        // NOTE: toBeLookedUp List obj is only instantiated at 'load' time. 
        //       i.e., this object must first be saved to disk, then loaded before
        //       the toBeLookedUp list comes into play
    }

    /*
    public AbstractCallBackExpire(List<Action<T>> callbackList) {
        this.callbackList = new List<Action<T>>(callbackList);
    }
    */

    private static void defaultCallback(T arg) { } // Default empty callback function

    public void runCallbacks(T arg)
    {
        // TODO: Should this really be a public function? 

        // First run all the 'regular' actions
        foreach (KeyValuePair<string, Action<T>> kvp in refrencedActions)
        {
            Action<T> act = kvp.Value;
            act(arg);
        }

        // Then run all the 'lazy lookup' actions
        foreach (KeyValuePair<string, string> kvp in toBeLookedUp)
        {
            string NocabName = kvp.Key;
            string methodName = kvp.Value;
            lazyLookupAndRunCallback(NocabName, methodName, arg);
        }
    }

    public static void lazyLookupAndRunCallback(string NocabName, string FuncName, T arg)
    {
        object instanceRef = CentralRegistry.getObject(NocabName);
        if (instanceRef == null)
        {
            Debug.Log("Callback target could not be found. Target NocabName: " + NocabName);
            return;
        }
        Type t = instanceRef.GetType();
        MethodInfo method = t.GetMethod(FuncName);
        if (method == null)
        {
            Debug.LogError("Warning! An Expire object attempted to look up a callback but couldn't find it. " +
                "The instance could be found with name: \"" + NocabName + "\". " +
                "The method name could NOT be found in the object's type. " +
                "Method name: \"" + FuncName + "\". " + " Obj type: \"" + t.Name + "\"");
            return;
        }
        Action<T> action = (Action<T>)Delegate.CreateDelegate(typeof(Action<T>), instanceRef, method);
        action.Invoke(arg);
    }

    public void addCallback(string NocabName, Action<T> callback)
    {
        this.refrencedActions.Add(new KeyValuePair<string, Action<T>>(NocabName, callback));
    }

    public abstract bool cycle();
    public abstract int strength();


    private string _myType = "AbstractExpireable"; // Version 1
    public string myType() { return _myType; }

    public JsonObject toJson()
    {
        JsonObject result = new JsonObject();
        JsonArray NocabNameLists = new JsonArray();
        JsonArray MethodNameList = new JsonArray();

        foreach (KeyValuePair<string, Action<T>> callback in this.refrencedActions)
        {
            string NocabName = callback.Key;
            Action<T> _delegate = callback.Value;
            string methodName = _delegate.Method.Name;

            NocabNameLists.Add(NocabName);
            MethodNameList.Add(methodName);
        }

        result["Type"] = _myType;

        result["InstanceNocabNames"] = NocabNameLists;
        result["MethodNames"] = MethodNameList;

        return result;
    }

    public void loadJson(JsonObject jo)
    {
        if (!jo.ContainsKey("Type")) { throw new InvalidLoadType("Missing Type field, this is not valid json object"); }
        if (jo["Type"] != _myType) { throw new InvalidLoadType("JsonObject has invalid type: " + jo["Type"]); }

        JsonArray NocabNameList = jo["InstanceNocabNames"];
        JsonArray MethodNameList = jo["MethodNames"];
        this.toBeLookedUp = new List<KeyValuePair<string, string>>(NocabNameList.Count);

        for (int i = 0; i < NocabNameList.Count; i++)
        {
            string NocabName = NocabNameList[i];
            string methodName = MethodNameList[i];
            this.toBeLookedUp.Add(new KeyValuePair<string, string>(NocabName, methodName));
        }
        // TODO: assert NocabNameLists.count == MethodNameList.count
    }
}