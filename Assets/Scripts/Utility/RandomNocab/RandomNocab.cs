using System.Collections;
using System.Collections.Generic;
using System;

public class RandomNocab {

    private Random rng;
    private Random otherRNG = new Random(); // A seccond RNG for "off seed" numbers

    public RandomNocab() { this.rng = new Random(); }

    public RandomNocab(int seed) { this.rng = new Random(seed); }

    public RandomNocab(string seed) { this.rng = new Random(seed.GetHashCode()); }

    public int randomInclusive(int min, int max) { return this.rng.Next(min, max + 1); }
    public int randomInclusiveDontAdvance(int min, int max) { return this.otherRNG.Next(min, max + 1); }



}
