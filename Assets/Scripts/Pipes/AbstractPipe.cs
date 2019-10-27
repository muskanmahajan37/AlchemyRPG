using System.Collections;
using System.Collections.Generic;
using LightJson;
using System;

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
        this.loadJson(jo);
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

    public virtual void loadJson(JsonObject jo)
    {
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


    #endregion JsonConvertable

}


