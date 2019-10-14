using System;
using System.Collections;
using System.Collections.Generic;
using LightJson;
using UnityEngine;


public interface Pipe<T> : IExpire<Pipe<T>>, IFlagable, JsonConvertable, INocabNameable {

    T pump(T incomingValue);

    // TODO: Activation conditions
    // TODO: Pipe self clean up
}


public abstract class AbstractPipe<T> : Pipe<T> {

    protected IExpire<Pipe<T>> myExpire;
    protected IFlagable myFlagable;



    public AbstractPipe(IExpire<Pipe<T>> e, IFlagable f) {
        this.myExpire = e;
        this.myFlagable = f;
    }

    #region INocabNameable functions
    public string getNocabName() { throw new NotImplementedException(); asfasdfasdf  }

    public bool deregister() { throw new NotImplementedException(); }
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

    public string myType() {
        throw new NotImplementedException();
    }

    public JsonObject toJson() {
        throw new NotImplementedException();
    }

    public void fromJson(JsonObject jo) {
        throw new NotImplementedException();
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
        //       If you really don't want to use the flag parameter, pass in the FlagConstans.NO_FLAG const
        if (flag != FlagConstants.NO_FLAG) { this.addFlag(flag); } 
        this.delta = delta;
    }

    public PipeSum(IExpire<Pipe<int>> e, IFlagable f, int delta, IEnumerable<string> flags) : base(e, f) {
        this.addFlags(flags);
        this.delta = delta;
    }


    public override int pump(int incomingValue) {
        return incomingValue + delta;
    }

    #region Json Saving/ Loading
    public const string _MyJsonType = "PipeSum"; // Version 1

    public JsonObject toJson(PipeSum pipe) {
        JsonObject result = new JsonObject();
        result["Type"] = _MyJsonType;
        result["delta"] = this.delta;
        result["Flagable"] = this.myFlagable.toJson();
        return result;
    }

    /*
    public PipeSum fromJson(JsonObject jo) {
        // WARNING: This function updates itself even if it has some other
        // delta value/ use already.
        // The best way to load a JsonPipeSum is to make a new empty PipeSum
        // object and load the JsonObject into it...

        if(!jo.ContainsKey("Type")) {
            // TODO make a better Error type for this.
            throw new InvalidLoadType("When loading a PipeSum, the provided JasonObject did not have a type field.");
        }
        if (jo["Type"] != _MyJsonType) {
            throw new InvalidLoadType("Can't load PipeSum, expected vs recieved type: " + _MyJsonType + " != " + jo["Type"].ToString());
        }

        PipeSum result = new PipeSum();
        return result;
    }
    */

    public string myType() {
        return _MyJsonType;
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

    public override PMAction pump(PMAction incomingAction) {
        // TODO: what would a pipe do to an action? 
        //       Perhapse this can only take in specific implimentations of actions? 
        return incomingAction;
    }

}
