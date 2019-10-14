using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem : Genetic<ConsumableItem> {
    // Simple consumables provide boosts to target

    private EStat target;
    private int change;

    public ConsumableItem(EStat target, int change) {
        this.target = target;
        this.change = change;
    }

    public ConsumableItem merge(ConsumableItem other) {
        EStat childStat = (Random.value > 0.5) ? this.target : other.target;
        int childChange = (this.change < other.change) ? 
                          Random.Range(this.change, other.change + 1) : // range is max-exclusive so +1
                          Random.Range(other.change, this.change + 1);

        return new ConsumableItem(childStat, childChange);
    }
}
