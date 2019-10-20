using System;
using System.Collections;
using System.Collections.Generic;
using LightJson;
using UnityEngine;


// TODO: Pipes should NOT be INocabNamable objects. I can't see any reason for an other
// object to need a refrence to a Pipe instance. If anything, Pipes themselves need a 
// refrence to other things (like mobs or the weather or something), so the other thing
// should have/be a INocabNameable so the pipe can find it later.
// The only reason why Pipe is INocabNamable is because I wanted to practice
// inplimenting the interface along with the JsonConvertable 
public interface Pipe<T> : IExpire<Pipe<T>>, IFlagable, JsonConvertable, INocabNameable {

    T pump(T incomingValue);

    // TODO: Activation conditions
    // TODO: Pipe self clean up
}


public abstract class AbstractPipe<T> : Pipe<T> {

    protected IExpire<Pipe<T>> myExpire;
    protected IFlagable myFlagable;
    protected INocabNameable myNocabName; 

    public AbstractPipe(IExpire<Pipe<T>> e, IFlagable f) {
        this.myExpire = e;
        this.myFlagable = f;

        // TODO, consider providing this object with a better NocabName
        // than the default uuid from a NocabNamable. 
        this.myNocabName = new NocabNameable(this);
        
    }

    public AbstractPipe(JsonObject jo) {
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

    public string myType() { return _MyType; }

    public JsonObject toJson() {
        JsonObject result = new JsonObject();

        // Pack this class
        result["NocabName"] = myNocabName.getNocabName();
        result["Flags"] = myFlagable.toJson();
        result["Expire"] = myExpire.toJson();

        result["Type"] = _MyType;
        return result;
    }

    public void loadJson(JsonObject jo) {
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

public class PipeSum : AbstractPipe<int> {



    private int delta;

    public PipeSum(IExpire<Pipe<int>> e, IFlagable f, int delta, string flag = FlagConstants.NO_FLAG) : base(e, f) {

        // NOTE: Generaly a Pipe should always have flags. However
        //       the provided IFlagable may be preloaded with flags.
        //       You SHOULD either:
        //    1) Provide a flag in the optional flag field
        //    2) Use an IFlagable object that already has flags
        //       Failure to do either may cause problems of lost pipes/ memory leaks
        //
        //       The flags are used by Pipeline objects to sort and manage them. 
        //       If you really don't want to use the flag parameter, pass in the FlagConstans.NO_FLAG const
        if (flag != FlagConstants.NO_FLAG) { this.addFlag(flag); } 
        this.delta = delta;
    }

    public PipeSum(IExpire<Pipe<int>> e, IFlagable f, int delta, IEnumerable<string> flags) : base(e, f) {
        this.addFlags(flags);
        this.delta = delta;
    }

    public PipeSum(JsonObject jo) : base(jo["Base"]) {
        /**
         * Please use PipeFactory<T>().fromJson(JsonObject jo) instead of this constructor!
         *
         * NOTE: Using this constructor is frowned upon because it does NOT validate 
         * the incoming jo object. The JO could be malformed => Errors.
         */ 

        // NOTE, the base class takes care of loding itself using the JsonObject constructor
        // therefore we only need to load the data specific to this object.
        this.loadJsonThis(jo);
    }


    public override int pump(int incomingValue) {
        return incomingValue + delta;
    }

    #region Json Saving/ Loading
    public const string _MyJsonType = "PipeSum"; // Version 1
    public new string myType() { return _MyJsonType; }

    public JsonObject toJson(PipeSum pipe) {
        JsonObject result = new JsonObject();

        // Pack the base class
        result["Base"] = base.toJson();

        // Pack this class data
        result["Delta"] = this.delta;
        result["Type"] = _MyJsonType;

        return result;
    }

    public new void loadJson(JsonObject jo) {
        if (!jo.ContainsKey("Type")) { throw new InvalidLoadType("Missing Type field, this is not valid json object"); }
        if (jo["Type"] != _MyType) { throw new InvalidLoadType("JsonObject has invalid type: " + jo["Type"]); }

        // Unpack the base class
        loadJsonBase(jo["Base"]);

        // Unpack this class data
        loadJsonThis(jo);
    }

    private void loadJsonBase(JsonObject baseJO) { base.loadJson(baseJO); }
    private void loadJsonThis(JsonObject jo) {
        // TODO: the base class creates a NocabNamable obj, and therefor the refrence points to that.
        // Should the ref point to base or this? 
        // Can I override myNocabNamable? 
        this.delta = jo["Delta"];
    }


    #endregion

}

public class PipeAction : AbstractPipe<PMAction> {

    public PipeAction(IExpire<Pipe<PMAction>> e,
                      IFlagable f,
                      string actionTypeFilter="", 
                      string targetSkill="",
                      string drainOrAdd="",
                      string dmgType="") : base(e, f) {
        if (actionTypeFilter != "") { this.addFlag(actionTypeFilter); }
        if (targetSkill      != "") { this.addFlag(targetSkill); }
        if (drainOrAdd       != "") { this.addFlag(drainOrAdd); }
        if (dmgType          != "") { this.addFlag(dmgType); }
    }

    public PipeAction(IExpire<Pipe<PMAction>> e, IFlagable f, IEnumerable<string> flags) : base(e, f) {
        // Construct a Pipe for actions with the given flags.
        // Only use this constructor if you know what the above sentence means.
        // Reccomended use the explicit flag assignment constructor. 
        this.addFlags(flags);
    }

    public PipeAction(JsonObject jo): base(jo["Base"]) {
        /**
         * Please use PipeFactory<T>().fromJson(JsonObject jo) instead of this constructor!
         *
         * NOTE: Using this constructor is frowned upon because it does NOT validate 
         * the incoming jo object. The JO could be malformed => Errors.
         */
        loadJsonThis(jo);
    }

    public override PMAction pump(PMAction incomingAction) {
        // TODO: what would a pipe do to an action? 
        //       Perhapse this can only take in specific implimentations of actions? 
        return incomingAction;
    }

    #region Json Saving/ Loading
    public const string _MyJsonType = "PipeAction"; // Version 1
    public new string myType() { return _MyJsonType; }

    public JsonObject toJson(PipeSum pipe) {
        JsonObject result = new JsonObject();

        // Pack the base class
        result["Base"] = base.toJson();

        // Pack this class data
        result["Type"] = _MyJsonType;

        return result;
    }

    public new void loadJson(JsonObject jo) {
        if (!jo.ContainsKey("Type")) { throw new InvalidLoadType("Missing Type field, this is not valid json object"); }
        if (jo["Type"] != _MyType) { throw new InvalidLoadType("JsonObject has invalid type: " + jo["Type"]); }

        // Unpack the base class
        loadJsonBase(jo["Base"]);

        // Unpack this class data
        loadJsonThis(jo);
    }

    private void loadJsonBase(JsonObject baseJO) { base.loadJson(baseJO); }
    private void loadJsonThis(JsonObject jo) { }


    #endregion

}
