using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentTesting : MonoBehaviour {


    // Start is called before the first frame update
    void Start() {
        testEquipableAction();
        testEquipablePipe();
        testMultiPipe();

    }

    private void testEquipableAction()
    {
        Dictionary<string, int> m1Stats = new Dictionary<string, int> { { BattleStats.CURRENT_HEALTH, 10 }, { BattleStats.MAX_HEALTH, 10 } };
        Dictionary<string, int> m2Stats = new Dictionary<string, int>(m1Stats);
        PMMob m1 = new PMMob(m1Stats);
        PMMob m2 = new PMMob(m2Stats);

        PMAction swordSlash = new ActionAttack(5);
        IEquipable attackSword = new ActionEquipable(EquipSlots.MAIN_HAND, swordSlash);
        m1.equip(attackSword);


        IEquipable e = m1.peekEquipmentSlot(EquipSlots.MAIN_HAND);
        if (e.targetSlot() == EquipSlots.MAIN_HAND)
        {
            Debug.Log("Nocab test 1.1 passed");
        }
        else { Debug.Log("Nocab test 1.1 fail"); }

        PMBattleController bc = new PMBattleController(m1, m2);

        PMAction a = m1.getNextAction(bc);
        a.activate(m2);

        if (m2.getStat(BattleStats.CURRENT_HEALTH) == 5) {
            Debug.Log("Nocab test 1.2 passed");
        }
        else { Debug.Log("Nocab test 1.2 failed"); }

        m1.unequip(EquipSlots.MAIN_HAND);

    }


    private void testEquipablePipe() {
        Dictionary<string, int> m1Stats = new Dictionary<string, int> { { BattleStats.CURRENT_HEALTH, 10 }, { BattleStats.MAX_HEALTH, 10 }, { BattleStats.SPEED, 0 } };
        Dictionary<string, int> m2Stats = new Dictionary<string, int>(m1Stats);
        PMMob m1 = new PMMob(m1Stats);
        PMMob m2 = new PMMob(m2Stats);

        Pipe<int> speedUp = new PipeSum(new ExpireNever<Pipe<int>>(), new Flagable(), 5);
        speedUp.addFlag(BattleStats.SPEED);
        IEquipable helmOfSpeed = new EquipablePipe(EquipSlots.HELM, speedUp);

        m1.equip(helmOfSpeed);


        IEquipable e = m1.peekEquipmentSlot(EquipSlots.HELM);
        if (e.targetSlot() == EquipSlots.HELM)
        {
            Debug.Log("Nocab test 2.1 passed");
        }
        else { Debug.Log("Nocab test 2.1 fail"); }
        
        if (m1.getStat(BattleStats.SPEED) == 5) {
            Debug.Log("Nocab test 2.2 passed");
        }
        else { Debug.Log("Nocab test 2.2 failed"); }

        m1.unequip(EquipSlots.HELM);

        if (m1.getStat(BattleStats.SPEED) == 0) {
            Debug.Log("Nocab test 2.3 passed");
        } else { Debug.Log("Nocab test 2.3 failed: Equipable pipe did not reset value after unequip"); }
    }

    private void testMultiPipe() {
        Dictionary<string, int> m1Stats = new Dictionary<string, int> { { BattleStats.CURRENT_HEALTH, 10 }, { BattleStats.MAX_HEALTH, 10 }, { BattleStats.SPEED, 0 } };
        Dictionary<string, int> m2Stats = new Dictionary<string, int>(m1Stats);
        PMMob m1 = new PMMob(m1Stats);
        PMMob m2 = new PMMob(m2Stats);

        Pipe<int> speedUp = new PipeSum(new ExpireNever<Pipe<int>>(), new Flagable(), 5);
        speedUp.addFlag(BattleStats.SPEED);
        IEquipable helmOfSpeed = new EquipablePipe(EquipSlots.HELM, speedUp);
        PMAction swordSlash = new ActionAttack(5);
        IEquipable attackSword = new ActionEquipable(EquipSlots.MAIN_HAND, swordSlash);
        HashSet<IEquipable> subEquips = new HashSet<IEquipable>() {
            helmOfSpeed,
            attackSword
        };
        IEquipable multiItem = new MultiBehaviorEquipable(EquipSlots.HELM, subEquips);
        m1.equip(multiItem);

        IEquipable e = m1.peekEquipmentSlot(EquipSlots.HELM);
        if (e.targetSlot() == EquipSlots.HELM) {
            Debug.Log("Nocab test 3.1 passed");
        }
        else { Debug.Log("Nocab test 3.1 fail"); }

        PMBattleController bc = new PMBattleController(m1, m2);
        PMAction a = m1.getNextAction(bc);
        a.activate(m2);

        if (m2.getStat(BattleStats.CURRENT_HEALTH) == 5) {
            Debug.Log("Nocab test 3.2 passed");
        }
        else { Debug.Log("Nocab test 3.2 failed"); }

        if (m1.getStat(BattleStats.SPEED) == 5) {
            Debug.Log("Nocab test 3.3 passed");
        }
        else { Debug.Log("Nocab test 3.3 failed"); }



        m1.unequip(EquipSlots.HELM);



        if (m1.getStat(BattleStats.SPEED) == 0) {
            Debug.Log("Nocab test 3.4 passed");
        }
        else { Debug.Log("Nocab test 3.4 failed"); }
        if (m2.getStat(BattleStats.CURRENT_HEALTH) == 5) {
            Debug.Log("Nocab test 3.5 passed");
        }
        else { Debug.Log("Nocab test 3.5 failed"); }


    }

}
