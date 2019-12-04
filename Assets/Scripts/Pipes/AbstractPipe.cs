using System.Collections;
using System.Collections.Generic;
using LightJson;
using System;
using UnityEngine;

public abstract class AbstractPipe<T> : Pipe<T>
{

    protected IExpire<Pipe<T>> myExpire;
    protected IFlagable myFlagable;
    protected INocabNameable myNocabName;

    public AbstractPipe(IExpire<Pipe<T>> e, IFlagable f)
    {
        this.myExpire = e;
        this.myFlagable = f;

        // TODO, consider providing this object with a better NocabName
        // than the default uuid from a NocabNamable. 
        this.myNocabName = new NocabNameable(this);

    }

    public AbstractPipe(JsonObject jo)
    {
        // Used for loading/ saving
        // NOTE: don't use this.loadJson() because it's virtual and the implimenting
        //       class SHOULD be overriding it with the "override" keyword
        this.abstractLoadJson(jo);
    }

    #region INocabNameable functions
    public string getNocabName() { return this.myNocabName.getNocabName(); }

    public bool deregister() { return this.myNocabName.deregister(); }
    #endregion

    #region Expire functions
    public bool cycle() { return this.myExpire.cycle(); }
    public int strength() { return this.myExpire.strength(); }
    public void addCallback(string NocabName, Action<Pipe<T>> callback) { this.myExpire.addCallback(NocabName, callback); }
    public void runCallbacks(Pipe<T> arg) { this.myExpire.runCallbacks(arg); }
    #endregion

    #region IFlagable functions
    public ICollection<string> getFlags()
    { return this.myFlagable.getFlags(); }

    public void addFlag(string newFlag)
    { this.myFlagable.addFlag(newFlag); }

    public void addFlags(IEnumerable<string> newFlags)
    { this.myFlagable.addFlags(newFlags); }

    public void removeFlag(string flagToRemove)
    { this.myFlagable.removeFlag(flagToRemove); }

    public void removeFlags(IEnumerable<string> flagsToRemove)
    { this.myFlagable.removeFlags(flagsToRemove); }

    #endregion


    public abstract T pump(T incomingValue);




    #region JsonConvertable

    public const string _MyType = "AbstractPipe"; // V1

    public virtual string myType() { return _MyType; }

    public virtual JsonObject toJson()
    {
        JsonObject result = new JsonObject();

        // Pack this class
        result["Type"] = _MyType;

        result["NocabName"] = myNocabName.getNocabName();
        result["Flags"] = myFlagable.toJson();
        result["Expire"] = myExpire.toJson();

        return result;
    }

    protected void abstractLoadJson(JsonObject jo) {
        if (!jo.ContainsKey("Type")) { throw new InvalidLoadType("Missing Type field, this is not valid json object"); }
        if (jo["Type"] != _MyType) { throw new InvalidLoadType("JsonObject has invalid type: " + jo["Type"]); }

        // TODO: this is an abstract class. Should the CentralRegistry of NocabNamables 
        // have a refrence to this abstraction? I guess, what happens if you try to cast
        // an abstract class as the (hopefully) correct base class? I guess everything should
        // still work out ok... Maybe?
        this.myNocabName = new NocabNameable(jo["NocabName"], this);

        this.myFlagable = new Flagable();
        this.myFlagable.loadJson(jo["Flags"]); // Load the JsonObject into myFlagable

        ExpireableFactory<Pipe<T>> expireFactory = new ExpireableFactory<Pipe<T>>();
        this.myExpire = expireFactory.fromJson(jo["Expire"]);

    }

    public virtual void loadJson(JsonObject jo)
    {
        // Well, because this kina acts as a constructor for an abstract class it should never really be called.
        // However, because it's virtual the base class should use the override keyword when defining its own loadJson()
        // function and thus cut this loadJson() function out of the call time hierarchy 
        Debug.LogError("Warning: an AbstractPipe.loadJson() fuction was called. This should never happen because it should be overriden by an implimenting child class.");
        abstractLoadJson(jo);

        /*
         * Order of events
         * 1) Implimenting class is constructed with a JO
         * 2) Implimenting class runs this constructor with a JO
         * 2.1) this constructor runs the abstractLoadJson() function
         * 3) Implimenting class runs it's own loadJson() function
         */
         // TODO considering the above order of events, does AbstractPipe class need to have a loadJson() function?
         //      Should AbstractPipe have an empty loadJson() function?
    }


    #endregion JsonConvertable

}


