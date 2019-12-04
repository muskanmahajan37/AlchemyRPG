using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightJson;
using System;

public static class JsonFactory {
    /**
     * This factory is a simple way to extend the JsonConverter interface
     * to common objects like Dictionaries and Hashsets.
     * 
     * Although, it's kinda ugly and not the nicest to maintain, namely
     * the problem is that each type needs a dedicated save/ load function
     */

    // TODO: Consider making a factory of factories
    // That way, complex generic type T objects can be de-seralized 
    // relativly dynamically. Bonus points for making a pooling system of Factories



    #region Dictionary
    public const string _DictionaryStringString = "Dict<String,String>"; // Version 1
    public const string _DictionaryStringListString = "Dict<String,List<String>>"; // Version 1
    //public const string _DictionaryStringHashSetPipeT = "_DictionaryStringHashSetPipeT"; // Version 1

    #region Dictionary<string, string>
    public static JsonObject toJson(Dictionary<string, string> dictionary) {
        JsonObject result = new JsonObject();
        result["Type"] = _DictionaryStringString;

        // a nested JsonObject because what if the privide dict has a key == "Type"?
        JsonObject data = new JsonObject();
        foreach(KeyValuePair<string, string> kvp in dictionary) {
            data[kvp.Key] = kvp.Value;
        }
        result["Data"] = data;
        return result;
    }

    public static Dictionary<string, string> fromJsonDictionary(JsonObject jo) {
        if (jo["Type"] != _DictionaryStringString) {
            throw new Exception("Trying to unfold a json object into a dictionary but the JO is of type: " + jo["Type"]);
        }
        Dictionary<string, string> result = new Dictionary<string, string>();
        JsonObject data = jo["Data"];
        foreach (KeyValuePair<string, JsonValue> kvp in data) {
            result.Add(kvp.Key, kvp.Value);
        }
        return result;
    }

    #endregion


    #region Dictionary<string, int>
    public const string _DictionaryStringInt = "Dict<String,Int>"; // Version 1
    public static JsonObject toJson(Dictionary<string, int> dictionary) {
        JsonObject result = new JsonObject();
        result["Type"] = _DictionaryStringInt;

        // a nested JsonObject b/c what if the provided dict has a key == "Type"?
        JsonObject data = new JsonObject();
        foreach(KeyValuePair<string, int> kvp in dictionary) {
            data[kvp.Key] = kvp.Value;
        }
        result["Data"] = data;
        return result;
    }

    public static Dictionary<string, int> fromJsonDictionaryStringInt(JsonObject jo) {
        if (jo["Type"] != _DictionaryStringInt) {
            throw new Exception("Trying to unfold a json object into a dictionary but the JO is of type: " + jo["Type"]);
        }
        Dictionary<string, int> result = new Dictionary<string, int>();
        JsonObject data = jo["Data"];
        foreach(KeyValuePair<string, JsonValue> kvp in data) { result.Add(kvp.Key, kvp.Value); }
        return result;
    }
    #endregion

    #region Dictionary<string, List<string>>

    public static JsonObject toJson(Dictionary<string, List<string>> dictionary) {
        JsonObject result = new JsonObject();
        result["Type"] = _DictionaryStringListString;

        /*
         * {
         *  Type: "_DictionaryStringListString"
         *  Data: {
         *      dictionary.key : [value, value, value...]
         *      dictionary.key : [value, value, value...]
         *      dictionary.key : [value, value, value...]
         *      .
         *      .
         *      .
         *  }
         * }
         */

        // a nested JsonObject because what if the privided dict has a key == "Type"?
        JsonObject data = new JsonObject();
        foreach (KeyValuePair<string, List<string>> kvp in dictionary) {
            JsonArray list = new JsonArray();
            foreach (string s in kvp.Value) {
                list.Add(s);
            }
            data[kvp.Key] = list;
        }
        result["Data"] = data;
        return result;
    }

    public static Dictionary<string, List<string>> fromJsonDictionaryStringListString(JsonObject jo) {
        if (jo["Type"] != _DictionaryStringListString) {
            throw new Exception("Trying to unfold a json object inot a dictionary but the JO is of type: " + jo["Type"]);
        }
        /*
         * {
         *  Type: "blah"
         *  Data: {
         *      dictionary.key : [value, value, value...]
         *      dictionary.key : [value, value, value...]
         *      dictionary.key : [value, value, value...]
         *      .
         *      .
         *      .
         *  }
         * }
         */
        Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();
        JsonObject data = jo["Data"];
        foreach(KeyValuePair<string, JsonValue> kvp in data) {
            // kvp =>  "dictionary.key : [value, value, value...]"
            List<string> dictValueList = new List<string>();
            JsonArray jsonList = kvp.Value;
            foreach(JsonValue v in jsonList) {
                dictValueList.Add(v);
            }
            result.Add(kvp.Key, dictValueList);
        }
        return result;
    }
    #endregion

    #endregion


    #region List
    public const string _ListString = "ListString";

    #region List<String>
    public static JsonObject toJson(List<string> list) {
        JsonObject jo = new JsonObject();
        jo["Type"] = _ListString;

        JsonArray jsonList = new JsonArray();
        foreach(string s in list) { jsonList.Add(s); }
        jo["Data"] = jsonList;

        return jo;
    }

    public static List<string> fromJsonListString(JsonObject jo) {
        if (jo["Type"] != _ListString) {
            throw new InvalidLoadType("Expected " + _ListString + ", But actually got type of: " + jo["Type"]);
        }

        List<string> result = new List<string>();
        JsonArray data = jo["Data"];
        foreach(JsonValue value in data) {
            result.Add(value);
        }

        return result;
    }

    #endregion

    #endregion


    #region Hashset
    public const string _HashSetString = "HashSet<String>"; // Version 1

    #region JsonSave
    public static JsonObject toJson(HashSet<string> hashSet) {
        JsonObject jo = new JsonObject();
        jo["Type"] = _HashSetString;
        JsonArray array = new JsonArray();
        foreach (string element in hashSet) {
            array.Add(element);
        }
        jo["Elements"] = array;
        return jo;
    }

    #endregion JsonSave

    #region JsonLad
    public static HashSet<string> fromJsonHashSet(JsonObject jo) {
        if (jo["Type"] != _HashSetString) {
            throw new Exception("Trying to unfold a json object into a hashset but the JO is of type: " + jo["Type"]);
        }
        JsonArray array = jo["Elements"];
        HashSet<string> result = new HashSet<string>();
        foreach (JsonValue element in array) {
            result.Add(element);
        }
        return result;
    }

    #endregion JsonLoad

    #endregion Hashset


}
