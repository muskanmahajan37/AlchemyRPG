using System.Collections;
using System.Collections.Generic;
using LightJson;
using UnityEngine;

public static class FlagConstants {
    public const string NO_FLAG = "_noFlag_";

    public const string MODIFY = "_modify_";
}

public interface IFlagable : JsonConvertable {

    ICollection<string> getFlags();

    void addFlag(string newFlag);

    void addFlags(IEnumerable<string> newFlags);

    void removeFlag(string flagToRemove);

    void removeFlags(IEnumerable<string> flagsToRemove);

}

// There is only one kind of "Flagable" and that's a HashSet flagable
// Duplite flags don't make sense.
public class Flagable : IFlagable {

    private HashSet<string> flags = new HashSet<string>();

    public Flagable() { }
    public Flagable(string flag) { this.addFlag(flag); }
    public Flagable(IEnumerable<string> flags) { this.addFlags(flags); }

    public void addFlag(string newFlag) {
        if (newFlag == FlagConstants.NO_FLAG) {
            return;
        }
        this.flags.Add(newFlag);
    }

    public void addFlags(IEnumerable<string> newFlags) {
        foreach(string f in newFlags) {
            this.addFlag(f);
        }
    }

    public ICollection<string> getFlags() {
        return this.flags;
    }



    public void removeFlag(string flagToRemove) {
        this.flags.Remove(flagToRemove);
    }

    public void removeFlags(IEnumerable<string> flagsToRemove) {
        foreach(string f in flagsToRemove) {
            this.removeFlag(f);
        }
    }


    #region Json Saving/ Loading

    public const string _MySavingType = "Flagable"; // Version 1 

    public string myType() {
        return _MySavingType;
    }

    public void loadJson(JsonObject jo) {
        if (!jo.ContainsKey("Type")) { throw new InvalidLoadType("Missing Type field, this is not valid json object"); }
        if (jo["Type"] != _MySavingType) { throw new InvalidLoadType("JsonObject has invalid type: " + jo["Type"]); }

        HashSet<string> importedFlags = JsonFactory.fromJsonHashSet(jo["Flags"]);
        this.flags = importedFlags;
    }


    public JsonObject toJson() {
        JsonObject result = new JsonObject();
        result["Flags"] = JsonFactory.toJson(this.flags);
        return result;
    }
    #endregion

}
