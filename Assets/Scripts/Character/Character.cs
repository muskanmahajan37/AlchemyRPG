using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character {

    private string name;
    private Dictionary<EStat, int> stats;

    public int hp { get { return stats[EStat.HP]; } }
    public int mp { get { return stats[EStat.MP]; } }
    public int stamina { get { return stats[EStat.Stamina]; } }

    private int _exp;
    public int exp { get { return _exp; } }

    

    public Character(string name, int exp) {
        this.name = name;
        this._exp = exp;

        stats.Add(EStat.HP, 100);
        stats.Add(EStat.MP, 20);
        stats.Add(EStat.Stamina, 50);
    }
    

    public int getStat(EStat targetStat) {
        if ( ! stats.ContainsKey(targetStat)) {
            // No stat => 
            return 0;
        }

        return stats[targetStat];
    }
}
