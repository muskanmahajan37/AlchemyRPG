using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using LightJson;

public class ExpireCountCycle<T> : AbstractCallBackExpire<T>
{
    /**
     * An implementation of the expireable interface that
     * counts the number of cycle calls and "dies" when the 
     * count reaches 0
     * 
     * This also inherits the AbstractSelfCleaningExpire so it's got
     * callback on death functunality. 
     */

    private int _cycleCount;
    public ExpireCountCycle(int cycleCount) { this._cycleCount = cycleCount; }

    public override bool cycle()
    {
        this._cycleCount -= 1;
        return this._cycleCount <= 0;
    }

    public override int strength() { return this._cycleCount; }

    #region Json saving/ loading

    public const string _MySaveType = "ExpireCountCycle";
    public new string myType() { return _MySaveType; }

    public new JsonObject toJson()
    {
        JsonObject result = new JsonObject();

        // Pack the base class
        JsonObject baseCallBack = base.toJson();
        result["Base"] = baseCallBack;

        // Pack this class
        result["CycleCount"] = this._cycleCount;
        result["Type"] = _MySaveType;

        return result;
    }

    public new void loadJson(JsonObject jo)
    {
        if (!jo.ContainsKey("Type")) { throw new InvalidLoadType("Missing Type field, this is not valid json object"); }
        if (jo["Type"] != _MySaveType) { throw new InvalidLoadType("JsonObject has invalid type: " + jo["Type"]); }

        // Unpack the base class 
        JsonObject baseCallBack = jo["Base"];
        base.loadJson(baseCallBack);

        // Unpack this class 
        this._cycleCount = jo["CycleCount"];
    }

    #endregion
}
