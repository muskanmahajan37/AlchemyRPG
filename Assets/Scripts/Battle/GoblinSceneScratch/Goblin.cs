using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : StatContainer {
    
    public Goblin() {
        this.setStat("HP", 100);
        this.setStat("Attack", 5);
        this.setStat("Defence", 3);
    }


    public PMAction planAction() {
        return new ActionAttack(this.getStat("Attack"));
    }
    

}

/*
 * On a given character's turn, the battle controller prompts for the following info in this order
 * 
 * 1) Action to be used
 *     -- The Mob/PC decides what action it can do. 
 *     -- The Battle Controller (BC) dosen't care/ validate the action
 * 2) The BC provides the Mob/ player with a list of valid targets
 * 2.1) The Mob/ Player picks the target of the action
 * 3) The BC merges the target into the Action
 * 4) The action is activated (probably by the BC)
 *    -- Remember, the Action is a slef contained script, it probably won't need to talk to the BC
 *    -- At creation time (step 1) all the important data/ refrences should of been loaded into the action
 *    
 * 5) The action resolves, the BC turn order advances and the cycle repeats for the next mob
 */

 /*
  * So, I need an interface for the BC to talk to. This interface will have a 
  * function for each of the steps in the above list.
  * Namely:
  *     1) Get action
  *     2) Get target
  *     3?) Provide list of valid targets
  *     
  * The BC may also need a few key functions
  *   - Get list of all current targets
  *   - is target dead?
  *   - what is the current turn order?
  *   - Give me a read-only refrence to a target so I can plan
  */
