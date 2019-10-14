using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMBattleController {


    /**
     * A pokemon style battle is a battle with 2 characters only
     * Each character takes a turn doing an action
     * 
     * If one character reaches 0 hp, they die
     * 
     * The Battle Controller does NOT care about what is happening,
     * IE it does NOT have any logic about what each action does or
     * what a character is thinking about. 
     * 
     * The Battle Controller is just the referee, owns the turn
     * order and win loose logic. 
     * 
     */
     

    private Dictionary<PMMob, int> mobToTeam;
    private HashSet<int> teams;
    private List<PMMob> turnOrder;


    PMMob ch1;
    PMMob ch2;

    private int turnNumber = 0;

    public PMBattleController(PMMob team1, PMMob team2) {
        this.ch1 = team1;
        this.ch2 = team2;

        mobToTeam = new Dictionary<PMMob, int>(2) {
            { team1, 1 },
            { team2, 2 }
        };

        turnOrder = new List<PMMob>();
        turnOrder.Add(ch1);
        turnOrder.Add(ch2);
    }
    

    public void doTurn(bool team1Turn) {
        PMAction nextAction;
        PMMob target;

        // Ask the current team for an action
        if (team1Turn) {
            nextAction = ch1.getNextAction(this);
            target = ch2;
        } else {
            nextAction = ch2.getNextAction(this);
            target = ch1;
        }

        // Do the action
        this.processAction(nextAction, target);
        this.checkForWin();
    }
    
    public void processAction(PMAction action, PMMob target) {
        // Each action is responsable for itself
        // The battle controller just tells the action when 
        // to do it's thing.
        action.activate(target);
    }

    public bool checkForWin() {
        // TODO: Why does this return a bool? Just have it start the
        //       tear down procedure? 
        //       Would anyone want to stay in a battle scene after 
        //        the battle ends? 
        redrawHealth();
        if (ch1.isDead()) { Debug.Log("CH1 dead => CH2 wins"); return true; }
        if (ch2.isDead()) { Debug.Log("CH2 dead => CH1 wins"); return true; }
        return false;
    }

    public void redrawHealth() {
        Debug.Log("=======================");
        Debug.Log("Turn number: " + turnNumber);
        Debug.Log("CH1: " + ch1.getStat(BattleStats.CURRENT_HEALTH) + "/" + ch1.getStat(BattleStats.MAX_HEALTH));
        Debug.Log("CH2: " + ch1.getStat(BattleStats.CURRENT_HEALTH) + "/" + ch1.getStat(BattleStats.MAX_HEALTH));
    }
    

    public int getMyTeam(PMMob m) {
        return this.mobToTeam[m];
    }

    public HashSet<int> getEnemyTeam(PMMob m) {
        int myTeam = getMyTeam(m);
        HashSet<int> result = new HashSet<int>(this.teams);
        result.Remove(myTeam);
        return result;
    }

    public PMMob getEnemy(PMMob m) {
        if   (getMyTeam(m) == 1) { return ch2; }
        else                     { return ch1; }
    }
}
