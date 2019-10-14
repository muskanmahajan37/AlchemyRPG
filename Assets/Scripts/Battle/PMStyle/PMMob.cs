using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface Mob {

    PMAction getNextAction(PMBattleController battleController);


    void equip(IEquipable newEquipment);

    IEquipable peekEquipmentSlot(string targetEquipmentSlot);

    bool unequip(string slot);

    // TODO: There is a functional difference betwen AddStat functunality and DrainStat
    //       For instance, with just the simple AddOutPipe(..) workflow, there is no real
    //       way to make a pipe that can differientiate betwen damage reduction (all incomeing
    //       dmg is reduced by 10%) or healing boosts (all incoming healing is reduced by 10%)
    //       In summary: the stat change direction is important and needs a specific pipeline
 
    // TODO: Why even have an inPipe? Like "Reduce incoming dmg by 3" vs "Increase def stat by 3"
    // Adds a pipe to the incoming new stat pipeline (probably you want to use addOutPipe)
    //void addInPipe(Pipe<int> newPipe);
    
        
        // Adds a pipe to the normal stat pipeline
    void addOutPipe(Pipe<int> newPipe);

    bool removeOutPipe(Pipe<int> targetPipe);

    // TODO: Consider having a subStat and addStat function
    //       Really, this is motiviated by making common actions easier
    //       Specifically, dealing x dmg to a target. 
    //       Perhapse I should just have a subHP() and addHP() funcs
    //       But what about mana and stamina and literally any other thing?
    //       Ok, Perhapse having an addStat() and subStat() func are important
    // TODO: Also, how do stats regenerate? After battle? After time delay? Never? 
    //       Should every stat have a max and current value? What about a min value? 
    //       This seems to be a design question of "what is a stat and how do you use it"
    //       Which is a question that is kicked to the game designer. 
    //       So the code should enable/ support all the possibilities by not designing
    //       for a specific solution. 
    // TODO: Dmg types. For example, consider the usecase: "Deal 5 dmg of Slash dmg" when
    //       a target has pipe "incoming Slash dmg reduced by 2"
    //void setStat(string targetStat, int newValue);

    int getStat(string targetStat);

    void modifyStat(string targetStat, int newValue, ICollection<string> otherFlags=null);

    bool isDead();
}


public class PMMob : Mob {

    private Dictionary<string, int> myStats;
    private Dictionary<string, IEquipable> myEquipment;
    private List<PMAction> myActions; // TODO: Consider storing actions as some kind of sorted list based on likelyhood of activation

    PipeLine<int> inPipes;
    PipeLine<int> outPipes;

    // public bool isDead { get { return health <= 0; } }
    public bool isDead() { return getStat(BattleStats.CURRENT_HEALTH) <= 0; }
    

    public PMMob(Dictionary<string, int> myRawStats) {
        myStats = myRawStats;

        outPipes = new PipeLine<int>();
        inPipes = new PipeLine<int>();
        this.myEquipment = new Dictionary<string, IEquipable>();
        this.myActions = new List<PMAction>();
    }
    
    public PMAction getNextAction(PMBattleController battleController) {
        PMMob enemy = battleController.getEnemy(this);
        if (this.myActions.Count == 0) { return new ActionAttack(1); }
        return myActions[0];
    }

    #region Actions
    /**
     * Actions are used by IEquipable items to provide attacks/ spells.
     * More generally, actions are things that this mob can do during battle
     * and doesn't have to come form an IEquipable
     */

    public void addAction(PMAction newAction) {
        myActions.Add(newAction);
    }

    public void removeAction(PMAction targetAction) {
        this.myActions.Remove(targetAction);
    }

    #endregion


    #region Equipment
    public void equip(IEquipable newEquipment) {
        string targetSlot = newEquipment.targetSlot();
        if (this.peekEquipmentSlot(targetSlot) != null) { this.unequip(targetSlot); }

        this.myEquipment[targetSlot] = newEquipment;
        newEquipment.onEquip(this);
    }

    public IEquipable peekEquipmentSlot(string targetEquipmentSlot) {
        if ( ! this.myEquipment.ContainsKey(targetEquipmentSlot)) { return null; }
        return this.myEquipment[targetEquipmentSlot];
    }

    public bool unequip(string targetEquipmentSlot) {
        if (this.peekEquipmentSlot(targetEquipmentSlot) == null) {
            // If the slot is empty (key not found), return false
            return false;
        }
        this.myEquipment[targetEquipmentSlot].onUnequip();
        return this.myEquipment.Remove(targetEquipmentSlot);
    }


    #endregion


    #region Pipes

    /**
     * TODO: Why not just make the internal data structures public? 
     *       Is it likely that there will be a pipeline arround pipes? 
     */

    /*
    public void addInPipe(Pipe<int> newPipe) {
        this.inPipes.addPipe(newPipe);
    }
    */


    public void addOutPipe(Pipe<int> newPipe) {
        this.outPipes.addPipe(newPipe);
    }

    public bool removeOutPipe(Pipe<int> targetPipe) {
        return this.outPipes.removePipe(targetPipe);
    }
    #endregion


    #region Stat Values

    public void addNewStat(string newStatName, int newValue) {
        // This function adds a new stat->value pair to a dictionary of stats
        // Not to be confused with setStat(...), this addNewStat(..) function
        // adds a new key to this.myStats dictionary
        if (this.myStats.ContainsKey(newStatName)) {
            string e = "Can not add new stat because it already exists. Please use the setStats(...) function instead.";
            e += "Trying to add: ";
            e += "\n newStatName: " + newStatName;
            e += "\n newValue: " + newValue;
            e += "\n oldValue: " + myStats[newStatName];
            throw new System.InvalidOperationException(e);
        }

        // TODO: Pipes and stuff may be relevant here?
        newValue = this.inPipes.pump(newStatName, newValue);
        this.myStats.Add(newStatName, newValue);
    }

    /**
    public void setStat(string targetStat, int newValue) {

        // TODO: Make clear the difference between setStat and modifyStat

        // This function modifies an existing stat->value pair in this mob's stat dictionary
        // Not to be confused with addNewStat(...), this setStat(..) function only works
        // on stats that  already exist inside this this mob
        if ( ! this.myStats.ContainsKey(targetStat)) {
            string e = "Can not set a stat, it dosen't exist. If trying to add a new stat to the mob please use addNewStat(...) function instead.";
            e += "\n targetStat: " + targetStat;
            e += "\n newValue: " + newValue;
            throw new System.InvalidOperationException(e);
        }

        // TODO: Add the StatFlag constant for "set"
        newValue = this.inPipes.pump(targetStat, newValue);
        this.myStats[targetStat] = newValue;
    }
        */

    public int getStat(string targetStat) {
        /**
         * All entities that wish to get the stats owned by this mob
         * should come through this function. 
         * 
         * Reason: This function takes care of all the pipes associated
         * with the targetStat. 
         */
        // TODO: Consider giving public access to raw stats? 
        // TODO: Consider this strange case:
        //       There is an out pipe that says +10 current HP
        //       then our raw hp drops to 0, but our piped getStat(...) 
        //       function returns +10. So... the only way to kill
        //       this mob is to dmg it to -10 HP? 
        //       Perhapse this case is fixed with some kind of post battle reset ability? 

        // This should be one of the only places that
        // the myStats dictionary is accessed directly. 
        int result = myStats[targetStat];
        // Pump the result through all the pipes
        result = this.outPipes.pump(targetStat, result);
        return result;
    }

    public void modifyStat(string targetStat, int delta, ICollection<string> otherFlags = null) {
        // TODO: Make clear the difference between setStat and modifyStat

        if (!this.myStats.ContainsKey(targetStat)) {
            string e = "Can not modify a stat, it dosen't exist. If trying to add a new stat to the mob please use addNewStat(...) function instead.";
            e += "\n targetStat: " + targetStat;
            e += "\n delta: " + delta;
            throw new System.InvalidOperationException(e);
        }

        HashSet<string> flags = (otherFlags == null) ? 
                                    new HashSet<string>() : 
                                    new HashSet<string>(otherFlags);
        flags.Add(targetStat);
        // TODO: should I use this modify flag? 
        //flags.Add(FlagConstants.MODIFY);

        delta = this.inPipes.pumpIntersection(flags, delta);
        this.myStats[targetStat] += delta;
        
        /**
         * TODO: I need to consider if pipes should be responsible for knowing
         * if they themselves are an "AND" or "OR" kind of flag pipe
         * 
         * Would someone ever set a pipe that only works for Skill AND Modify
         *                                                   Skill OR Modify?
         *                                                   
         * Also, in the PipeLine class do pipes get applied twice if it has 2 flags?
         * Like a pipe with flags ("speed" and "modify") and a pump with flags ("speed" and "modify")
         * Does the pipe get applied twice? or are all pumps put into a set of some kind? 
         * I'm pretty sure the correct behavior happens. 
         * 
         */

    }

    #endregion
}




/**
 * TODO: Battle reset:
 *  reset certain stats after a battle has happened
 *   (think stamina/ current speed/ hp etc)
 *  
 * 
 * 
 */
