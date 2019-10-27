using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PMMobFactory
{


    public static PMMob makeSimpleAttacker()
    {

        Dictionary<string, int> stats = new Dictionary<string, int>() {
            { BattleStats.MAX_HEALTH, 10 },
            { BattleStats.CURRENT_HEALTH, 10 },
            { BattleStats.SPEED, 3 }
        };

        return new PMMob(stats);

    }





    private static void setHealth(int maxHealth, Dictionary<string, int> stats)
    {


        if (stats.ContainsKey(BattleStats.MAX_HEALTH) ||
            stats.ContainsKey(BattleStats.CURRENT_HEALTH))
        {
            string maxHealthValue = stats.ContainsKey(BattleStats.MAX_HEALTH) ? "(not assigned)" : stats[BattleStats.MAX_HEALTH].ToString();
            string curHealthValue = stats.ContainsKey(BattleStats.MAX_HEALTH) ? "(not assigned)" : stats[BattleStats.CURRENT_HEALTH].ToString();
            throw new System.Exception("ERROR: during PMMob creation, the work in progress stat dictionary " +
                "already has the stats MAX_HEALTH or CURRENT_HEALTH set. The setHealth() function does NOT" +
                "override these values. These values are:    MaxHealth: " + maxHealthValue + "    CurrentHealth: " + curHealthValue);
        }

        stats[BattleStats.MAX_HEALTH] = maxHealth;
        stats[BattleStats.CURRENT_HEALTH] = maxHealth;
    }


    /**
     * TODO:
     * 
     * It feels like the Mob class is a super loose and flexable design allowing for many implimentations/ down stream uses.
     * However, in order to counter this flexable design standardiazation and general rules must now be applied infront of the Mob class.
     * This factory feels like the start of such rules.
     * IE: 
     *  This factory defines a "standard" stat requirements and values
     *  This factory also defines the difference between a "Attack only" mob and a "Tank" mob. In other words, the factory is makes the in game class
     *  
     *  TODO: Consider this:
     *      Should a implimentation of the Mob interface directly be an in game class (like mage vs ranger)? Perhapse the sub mage class points
     *      directly towards this factory to get all it's stat and move data? 
     *      
     * In pokemon, I think each pokemon is directly authored. 
     * TODO: Think about how Pokemon monsters get their stats
     *
     * 
     */



}
