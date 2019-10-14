using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SylableNameGen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public static string genSylableName() {
        // Pick a random number between 2-4
        // Chance to pick 2 80%
        // Chance to pick 3 10%
        // Chance to pick 4 10%
        //List<float> weights = new List<float>(3) { 0.8f, 0.1f, 0.1f };
        //List<int> choices =     new List<int>(3) {    2,    3,    4 };
        Dictionary<int, float> weights = new Dictionary<int, float>(3) { { 2, 0.8f }, { 3, 0.1f }, { 4, 0.1f } };
        
        return genSylableName(randomWeighted(weights));
    }

    public static string genSylableName(int size)
    {
        StringBuilder sb = new StringBuilder(size * 2);
        for (int i = 0; i < size; i ++) {
            sb.Append(buildRandomPair());
        }
        return sb.ToString();
    }
    
    public static char[] buildRandomPair() {
        if (UnityEngine.Random.value > 0.5) { return buildVFirstPair(); }
        else { return buildCFirstPair(); }
    }

    public static char[] buildVFirstPair() {
        return new char[2] { NameGen.getRandVowel(), NameGen.getRandConsonat() };
    }

    public static char[] buildCFirstPair() {
        return new char[2] { NameGen.getRandConsonat(), NameGen.getRandVowel() };
    }

    // TODO: Move this to a utility file
    public static T randomWeighted<T>(Dictionary<T, float> weights)
    {
        if(GameController.DevMode) {
            float totalSum = 0.0f;
            float percision = 0.001f;
            foreach(KeyValuePair<T, float> e in weights) {
                totalSum += e.Value;
            }
            if (totalSum < 1) {
                printDictionary(weights);
                throw new System.Exception("WARNING: provided dictionary's weights added up to less than 1.0f > " + totalSum );
            } 
            if (totalSum - 1.0 > percision) {
                printDictionary(weights);
                throw new System.Exception("WARNING: provided dictionary's weights added up to greater than (1.0f + " + percision + ") < " + totalSum );
            }

        }
        float trueRandom = UnityEngine.Random.value;
        float weightSum = 0.0f;
        foreach (KeyValuePair<T, float> entry in weights) {
            weightSum += entry.Value;
            if (weightSum > trueRandom) {
                return entry.Key;
            }
        }
        // Else, total sum of weights < 1 :(
        Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        Debug.Log("ERROR! randomWeighted() encounted a weight list that did not sum to 1");
        printDictionary(weights);
        Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        return default(T);
    }

    private static void printDictionary<T, U>(Dictionary<T, U> dict)
    {
        Debug.Log("-- Printing Dictionary --");
        foreach (KeyValuePair<T, U> kvp in dict) {
            Debug.Log("Key: \"" + kvp.Key.ToString() + "\"    Value: \"" + kvp.Value.ToString() + "\"");
        }
        Debug.Log("-- End printing Dictionary --");

    }

    // TODO: MOVE THIS INTO A UTILITY FILE
    // TODO: Make this more attractive to use over the other randomWeighted
    public static T randomWeighted<T>(List<float> weights, List<T> values) { return values[randomWeighted(weights)]; }

    // TODO: MOVE THIS SOMEWERE BETTER
    // TODO: Make this less attractive to use, should use the randomWeighted(weights, values) func for protection
    public static int randomWeighted(List<float> weights) {
        /**
         * Provided with a list of weights that must sum to 1.0
         * This method returns the index of the selected value
         * 
         * Effectively, this is a weighted random integer generator with output = [0, weights.count - 1]
         * 
         * WARNING: if the sum of the weights < 1 then the first element is returned
         */

        float trueRandom = UnityEngine.Random.value;
        float weightSum = 0.0f;
        for(int i = 0; i < weights.Count; i++) {
            weightSum += weights[i];
            if(weightSum > trueRandom) {
                return i;
            }
        }
        // Else, total sum of weights < 1
        Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        Debug.Log("ERROR! randomWeighted() encounted a weight list that did not sum to 1");
        Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        return 0;  // Default choice
    }
}
