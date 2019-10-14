using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightJson;

public class SavingMonobehaviorScratch : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        Debug.Log(Application.persistentDataPath);

        /*
        JsonObject jo = new JsonObject();
        jo.Add("Key", "String Value");
        jo.Add("Lives", 1);
        jo.Add("HitChance", 0.75);
        jo.Add("Array", new JsonArray().Add("k1").Add("v2").Add(31));

        string stringifiedJson = jo.ToString();

        Debug.Log(stringifiedJson);

        JsonObject result = JsonValue.Parse(stringifiedJson);
        Debug.Log(result["Key"]);
        Debug.Log(result["Array"][2]);
        */



        //SavingScratch.WriteStringToFile();
        //SavingScratch.WriteStringToFile("NocabMyDirectory", "other.Json");
    }

}
