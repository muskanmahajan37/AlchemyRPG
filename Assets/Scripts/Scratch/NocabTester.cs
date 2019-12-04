using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightJson;

public class NocabTester : MonoBehaviour {

    private void Start() {

        ExampleNocabChild testA = new ExampleNocabChild(5);
        JsonObject jo = testA.toJson();
        Debug.Log(jo.ToString());
        ExampleNocabChild testB = new ExampleNocabChild(jo);
        Debug.Log(testB.toJson().ToString());
        
    }

}
