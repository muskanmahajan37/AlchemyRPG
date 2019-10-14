using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PMAction  {

    /**
     * An action is something that can be done during a turn.
     */

    /**
     * The activate function does the action. For example,
     * an AttackAction.activate() does damage to a target mob directly. 
     * 
     * A result of true means the action did something, however minor.
     * A result of false means the action did not work (miss, or unfuffiled 
     * pre-recs). A fail could be because the caster is dead, or a different
     * earlier attack drained mana etc. 
     */
     // TODO: Have the fail case return some kind of error message explaining
     //       why it failed
    bool activate(PMMob target);

}

public class MultiActionSingleTarget {
    private HashSet<PMAction> behaviors;
    
    public MultiActionSingleTarget(HashSet<PMAction> behaviors) {
        this.behaviors = behaviors;
    }

    public bool activate(PMMob target) {
        bool result = true;
        foreach (PMAction action in this.behaviors) {
            result &= action.activate(target);
        }
        return result;
    }
}

public class ActionAttack : PMAction {

    private int dmg;

    public ActionAttack(int dmg) {
        this.dmg = -1 * Mathf.Abs(dmg);
    }

    public bool activate(PMMob target) {
        //Debug.Log("ActionAttack has been activated");
        target.modifyStat(BattleStats.CURRENT_HEALTH, dmg);
        return true;
    }
}

/**
 * Ok, here's the issue: I want these two distinct behaviors
 * 
 * 1) Enemy drains my mana
 *  - this includes building a pipe that says "All mana drain is reduced by 50%"
 *  
 * 2) I spend mana on my spell
 *  - The defence pipe described above should not apply to this mana drain behavior
 * 
 * So, given two instances of drainStat(mana), a pipe has two different behaviors
 *  - If an enemy drains the mana, the pipe effect happens
 *  - Else, the pipe effect does NOT happen
 * 
 * So, how does the pipe know what to do?
 * Perhapse I should make a Pipe< Action > item 
 * Or perhapse I should force every Action have a cost assocaited with it
 *  and differentiate between Costs and Effects of an action
 * 
 */



/**
 * What can Actions do? 
 * 
 * 1) Change numbers in the stats table
 * 
 * 2) Long lasting effects
 *    - Can't use last action
 *    - All healing dose dmg instead
 *    
 *    - These seem more like pipes...
 *    - Perhapse they are...
 *    
 * Maybe actions are only stat modifiers. 
 * An action is something that can change the state of a PMMob
 * It can't 
 * 
 */



