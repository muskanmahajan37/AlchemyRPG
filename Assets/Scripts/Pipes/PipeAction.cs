using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightJson;


public class PipeAction : AbstractPipe<PMAction>
{

    public PipeAction(IExpire<Pipe<PMAction>> e,
                      IFlagable f,
                      string actionTypeFilter = "",
                      string targetSkill = "",
                      string drainOrAdd = "",
                      string dmgType = "") : base(e, f)
    {
        if (actionTypeFilter != "") { this.addFlag(actionTypeFilter); }
        if (targetSkill != "") { this.addFlag(targetSkill); }
        if (drainOrAdd != "") { this.addFlag(drainOrAdd); }
        if (dmgType != "") { this.addFlag(dmgType); }
    }

    public PipeAction(IExpire<Pipe<PMAction>> e, IFlagable f, IEnumerable<string> flags) : base(e, f)
    {
        // Construct a Pipe for actions with the given flags.
        // Only use this constructor if you know what the above sentence means.
        // Reccomended use the explicit flag assignment constructor. 
        this.addFlags(flags);
    }

    public PipeAction(JsonObject jo) : base(jo["Base"])
    {
        /**
         * Please use PipeFactory<T>().fromJson(JsonObject jo) instead of this constructor!
         *
         * NOTE: Using this constructor is frowned upon because it does NOT validate 
         * the incoming jo object. The JO could be malformed => Errors.
         */
        loadJsonThis(jo);
    }

    public override PMAction pump(PMAction incomingAction)
    {
        // TODO: what would a pipe do to an action? 
        //       Perhapse this can only take in specific implimentations of actions? 
        return incomingAction;
    }

    #region Json Saving/ Loading
    public const string _MyJsonType = "PipeAction"; // Version 1
    public override string myType() { return _MyJsonType; }

    public override JsonObject toJson()
    {
        JsonObject result = new JsonObject();

        // Pack the base class
        result["Base"] = base.toJson();

        // Pack this class data
        result["Type"] = _MyJsonType;

        return result;
    }

    public override void loadJson(JsonObject jo)
    {
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
