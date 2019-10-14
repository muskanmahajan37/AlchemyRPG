using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTester : MonoBehaviour {

    private PMMob m1;
    private PMMob m2;

    PMBattleController bc;

    private bool turn;
    private int loopN;

    private void Start() {
        Dictionary<string, int> m1Stats = new Dictionary<string, int> { { BattleStats.CURRENT_HEALTH, 10 }, { BattleStats.MAX_HEALTH, 10} };
        Dictionary<string, int> m2Stats = new Dictionary<string, int>(m1Stats);
        m1 = new PMMob(m1Stats);
        m2 = new PMMob(m2Stats);
        this.bc = new PMBattleController(m1, m2);
        turn = true;
        loopN = 0;

    }

    private void Update() {
        if (loopN % 100 == 0) {
            bc.doTurn(turn);
            turn = !turn;
        }
        loopN++;
    }

}
