using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightJson;

public class JsonOrigamist  {
    /**
     * An Origamist is a person who preforms the art of origami.
     * 
     * In effect, think of this class like a StringBuilder; Load it
     * up with JsonObjects, then when you're ready have it write 
     * everything to disk all at once.
     * 
     * This class unfolds JsonObjects into strings and writes them to 
     * disk, then later reads the strings and folds them back into
     * their unique JsonObject representations.
     * 
     */

    private string relativeDirectory;
    private string fileName;
    private JsonArray toBeFolded;

    private const string ROOT = "root";

    public JsonOrigamist(string relativeDirectory, string fileName) {
        this.relativeDirectory = relativeDirectory;
        this.fileName = fileName;
        this.toBeFolded = new JsonArray();
    }



    public void add(JsonObject jo) {
        toBeFolded.Add(jo);
    }
 
    public void writeToDisk() {
        string jsonStr = toBeFolded.ToString();
        Debug.Log("WriteToDisk: " + jsonStr);
        NocabDiskUtility.WriteStringToFile(this.relativeDirectory, this.fileName, jsonStr);
    }


    public static JsonArray readFromDiskStatic(string relativeDirectory, string fileName) {
        string jsonStr = NocabDiskUtility.ReadStringFromFile(relativeDirectory, fileName);
        Debug.Log("ReadFromDisk: " + jsonStr);
        return JsonValue.Parse(jsonStr);
    }

    public JsonArray readFromDisk() {
        return readFromDiskStatic(this.relativeDirectory, this.fileName);
    }
    



    /**a
     * 
     * Ok, so I have a type string, then what? 
     * Making a super factory is a problem because each
     * thing has a unique return type :( 
     * 
     * So.... What's the best strategy to convert from JsonObject
     * to real object? 
     * 
     * 
     * TODO: 
     * make a recursive string->JsonObject folder, 
     * or at least some interface/ somethign that allows me to load
     * an actual Object from file
     * 
     * Should the reader vs writer be different objects? 
     * 
     */

}
