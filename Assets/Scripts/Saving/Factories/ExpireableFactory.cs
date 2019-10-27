using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightJson;


public class ExpireableFactory<T> {

    public IExpire<T> fromJson(JsonObject jo) {
        switch(jo["Type"].AsString) {

            case "AbstractExpireable": // Version 1
                Debug.Log("Error: Attempting to create an AbstractExpireable object is not allowed.");
                break;

            case "ExpireCountCycle": { // Version 1
                    ExpireCountCycle<T> result = new ExpireCountCycle<T>(-1);
                    result.loadJson(jo);
                    return result;
                }


            case "ExpireNever": { // Version 1
                    ExpireNever<T> result = new ExpireNever<T>();
                    result.loadJson(jo);
                    return result;
                }

            default: {
                    string detailedErrMsg = "Error: When attempting to create an IExpireable from Json, the provided ";
                    detailedErrMsg += "JsonObject[\"Type\"] was unexpected/ incorrect. Bad type: " + jo["Type"];
                    Debug.LogError(detailedErrMsg);
                    break;
                }
        }
        string errMsg = "Error, invalid Json[\"Type\"] when attempting to re-create/ load an IExpireable.";
        throw new InvalidLoadType(errMsg);
    }

}