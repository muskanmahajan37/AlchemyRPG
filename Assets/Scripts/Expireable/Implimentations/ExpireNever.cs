using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using LightJson;

public class ExpireNever<T> : AbstractCallBackExpire<T>
{
    /**
     * An implementation of the expireable interface that
     * never expires. 
     * 
     * Potentially used for long term but removable stat 
     * modifiers such as armor bonuses, or weather effects. 
     */

    public override bool cycle() { return false; }

    public override int strength() { return 1; }


    #region Json saving/ loading

    public const string _MySaveType = "ExpireNever";
    public new string myType() { return _MySaveType; }

    public new JsonObject toJson()
    {
        JsonObject result = new JsonObject();

        // Pack the base class
        JsonObject baseCallBack = base.toJson();
        result["Base"] = baseCallBack;

        // Pack this class
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

        // This class has no data to unpack
    }

    #endregion

}