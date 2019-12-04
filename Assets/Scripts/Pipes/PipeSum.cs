using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightJson;


public class PipeSum : AbstractPipe<int>
{

    private int delta;

    // TODO: think of making a PipeSum constructor that takes in an IExpire<Pipe<int>> expireable 
    public PipeSum(IExpire<Pipe<int>> e, IFlagable f, int delta) : base(e, f)
    {

        // You SHOULD either:
        // 1) Provide a flag in the optional flag field
        // 2) Use an IFlagable object that already has flags
        //    Failure to do either may cause problems of lost pipes/ memory leaks
        //
        // The flags are used by Pipeline objects to sort and manage them. 
        // If you really don't want to use the flag parameter, pass in the FlagConstans.NO_FLAG const
        this.delta = delta;
    }

    public PipeSum(JsonObject jo) : base(jo["Base"])
    {
        /**
         * Please consider useing PipeFactory<T>().fromJson(JsonObject jo) instead of this constructor.
         *
         * The PipeFactory is centralized and therefor can help with JO validation, but currently (Nov 2019)
         * the PipeFactory does NOT have any validation...
         */

        // NOTE, the base class takes care of loding itself using the JsonObject constructor
        // therefore we only need to load the data specific to this object.
        this.loadJsonThis(jo);
    }


    public override int pump(int incomingValue)
    {
        return incomingValue + delta;
    }

    #region Json Saving/ Loading
    public const string _MyJsonType = "PipeSum"; // Version 1
    public new string myType() { return _MyJsonType; }

    public override JsonObject toJson()
    {
        JsonObject result = new JsonObject();

        // Pack this class data
        result["Type"] = _MyJsonType;
        result["Delta"] = this.delta;

        // Pack the base class
        result["Base"] = base.toJson();

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
    private void loadJsonThis(JsonObject jo)
    {
        // TODO: the base class creates a NocabNamable obj, and therefor the refrence points to that.
        // Should the ref point to base or this? 
        // Can I override myNocabNamable? 
        this.delta = jo["Delta"];
    }


    #endregion

}
