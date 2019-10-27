using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using LightJson;

public class PipeFactory<T> {

    public Pipe<T> fromJson(JsonObject jo) {
        /**
         * To use this fromJson() function, you need to know a little bit about the JsonObject you
         * are trying to convert. Namely, you need to know the output type of the newly loaded pipe
         * type. 
         * 
         * The generic type of this PipFactory represents the generic type of the pipe that is being 
         * loaded. Certain types of Pipe Objects have a required output generic type. For instance,
         * the PipeSum pipe is a Pipe<int>, and no other generic type will work. 
         * 
         * Use:
         * 1) Create a PipeFactory<T> of the correct T (where T is the output type of the pipe to be loaded)
         * 2) Pass the json object into fromJson() funciton.
         * 3) The returned pipe will be a new object with the correct generic type casted as a Pipe<T>
         * 4) End user can (if they need to) cast the pipe into the more specific base class if needed
         */
        switch (jo["Type"].AsString) {

            case "AbstractPipe": // Version 1
                Debug.Log("Error: Attempting to create an AbstractExpireable object is not allowed.");
                break;

            case "PipeSum": { // Version 1
                    if (typeof(T) != typeof(int)) { Debug.Log(writeError("int")); }
                    PipeSum result = new PipeSum(jo);
                    result.loadJson(jo);
                    return (Pipe<T>)result;
                }

            case "PipeAction": { // Version 1
                    if (typeof(T) != typeof(PMAction)) { Debug.Log(writeError("PMAction")); }
                    PipeAction result = new PipeAction(jo);
                    result.loadJson(jo);
                    return (Pipe<T>)result;
                }


            default: {
                    string detailedErrMsg = "Error: When attempting to create an Pipe<T> from Json, the provided ";
                    detailedErrMsg += "JsonObject[\"Type\"] was unexpected/ incorrect. Bad type: " + jo["Type"];
                    Debug.LogError(detailedErrMsg);
                    break;
                }
        }


        string errMsg = "Error, invalid Json[\"Type\"] when attempting to re-create/ load a Pipe<T>.";
        throw new InvalidLoadType(errMsg);
    }

    private static string writeError(string typeName) {
        StringBuilder sb = new StringBuilder("ERROR: A PipeFactory of some non-\"");
        sb.Append(typeName);
        sb.Append("\" generic type ");
        sb.Append("is trying to make a PipeSum, which is of type Pipe<");
        sb.Append(typeName);
        sb.Append("> only! ");
        sb.Append("Please use a PipeFactory<");
        sb.Append(typeName);
        sb.Append("> to create a PipeSum object.");
        return sb.ToString();
    }

}
