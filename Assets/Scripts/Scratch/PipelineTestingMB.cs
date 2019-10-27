using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightJson;

public class PipelineTestingMB : MonoBehaviour {


    // Start is called before the first frame update
    void Start() {
        // Ok, what do I want to test?

        /**
         * 
         * 1) Make a pipe
         * 2) pump a the pipe
         * 3) jsonify the pipe
         * 4) write the json string
         * 5) read the json string
         * 6) load the json obj into a new pipe
         * 7) pump the pipe
         * 
         * 
         * 
         * 
         * 1) Create a pipeline 
         * 2) pump the pipeline
         * 3) jsonify the pipeline
         * 4) write the json string
         * 5) read the json string
         * 6) load the json obj into a new pipeline
         * 7) pump the pipeline
         * 
         * 
         * 
         */


        test();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void test() {

        // 1 make a Pipe
        IExpire<Pipe<int>> expire = new ExpireNever<Pipe<int>>();
        IFlagable flags = new Flagable();
        Pipe<int> p = new PipeSum(expire, flags, 10);

        // 2 Pump the pipe
        assert(p.pump(5) == 15, "Pump produced the wrong value during pump operation!");

        // 3 Jsonify
        JsonObject jo = p.toJson();
        assert(jo["Type"] == "PipeSum");


        string directory = "TempDirectory";
        string fileName = "TestFile";
        // NOTE: PersistantFilePath =  C:\Users\Arthur\AppData\LocalLow\DefaultCompany\StandAloneCon(plex)versation
        // 4 write to disk
        JsonOrigamist jsonOrigamist = new JsonOrigamist(directory, fileName);
        jsonOrigamist.add(jo);
        jsonOrigamist.writeToDisk();

        // 5 read from disk
        JsonArray ja = jsonOrigamist.readFromDisk();
        JsonObject joFromDisk = ja[0];
        assert(joFromDisk.ToString() == jo.ToString());

        // 6 Load the new pipe
        Pipe<int> pFromDisk = new PipeSum(joFromDisk);

        // 7 pump the new pipe
        assert(p.pump(5) == pFromDisk.pump(5));
}


    private void assert(bool b) {
        if (!b) { Debug.LogError("Assertion failed"); }
    }

    private void assert(bool b, string msg) {
        if (!b) { Debug.LogError("Assertion Failed with the following message: " + msg); }
    }

}
