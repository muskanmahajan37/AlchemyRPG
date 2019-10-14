using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticItem {

    private Dictionary<string, bool> genes;


}

public interface Genetic<T> { 
    T merge(T other);
}



public static class GeneticItemHelper {

    public static GeneticItem merge(GeneticItem a, GeneticItem b) {
        // Take some aspects of a and b, and melds them into a third item

        GeneticItem result = new GeneticItem();

        throw new System.NotImplementedException();
    }


}

public class Trait {

}
