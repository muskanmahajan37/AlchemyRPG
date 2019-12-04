using System.Collections;
using System.Collections.Generic;
using LightJson;
using UnityEngine;

public interface IStatContainer : JsonConvertible {

    bool hasStat(string statName);

    int getStat(string statName);

    int setStat(string statName, int newValue);

}


public class StatContainer : IStatContainer {

    private Dictionary<string, int> allStats;


    public StatContainer() {
        this.allStats = new Dictionary<string, int>();
    }

    public StatContainer(Dictionary<string, int> allStats) {
        this.allStats = allStats;
    }

    public StatContainer(JsonObject jo) {
        // TODO: What happens if the json is invalid? Should the program crash?
        this.loadJson(jo);
    }

    public bool hasStat(string statName) {
        return this.allStats.ContainsKey(statName);
    }

    public int getStat(string statName) {
        if ( ! this.hasStat(statName)) {
            Debug.LogError("Warning: this container does NOT contain the requested stat: " + statName);
            return 0;
        }
        return this.allStats[statName];
    }

    public int setStat(string statName, int newValue) {
        return this.allStats[statName] = newValue;
    }

    #region Json conversion
    private const string _myType = "StatContainer";  // Version 1
    public string myType() { return _myType; }

    public JsonObject toJson() {
        JsonObject result = new JsonObject();
        result["Type"] = _myType;
        result["allStats"] = JsonFactory.toJson(this.allStats);
        return result;
    }

    public void loadJson(JsonObject jo) {
        JsonUtilitiesNocab.assertValidJson(jo, _myType);
        this.allStats = JsonFactory.fromJsonDictionaryStringInt(jo["allStats"]);
    }
    #endregion
}
