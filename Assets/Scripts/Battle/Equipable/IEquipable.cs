using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipable {
    void onEquip(PMMob holder);

    void onUnequip();

    string targetSlot();

    // TODO: Either make Mob interface have a set pipe function or something
    //       OR make an IEquipTarget interface that Mob inherents

}


public class MultiBehaviorEquipable : IEquipable {

    private HashSet<IEquipable> behaviors;
    private string _targetSlot;

    public MultiBehaviorEquipable(string targetSlot, HashSet<IEquipable> behaviors) {
        // TODO: Do i care if the sub behaviors have conflicting targetSlot values? 

        this.behaviors = behaviors;
        this._targetSlot = targetSlot;
    }

    public void onEquip(PMMob holder) {
        foreach(IEquipable childBehavior in this.behaviors) {
            childBehavior.onEquip(holder);
        }
    }

    public void onUnequip() {
        foreach(IEquipable childBehavior in this.behaviors) {
            childBehavior.onUnequip();
        }
    }

    public string targetSlot() {
        return this._targetSlot;
    }

}

public class ActionEquipable : IEquipable {
    /**
     * This is an IEquipable that, on dequip, removes all the actions
     * associated with it. 
     */

    private string equipmentSlot;
    protected PMAction providedAction;

    protected PMMob holder;

    public ActionEquipable(string equipmentSlot, PMAction providedAction) {
        this.equipmentSlot = equipmentSlot;
        this.providedAction = providedAction;
    }

    public virtual void onEquip(PMMob holder) {
        //Debug.Log("ActionEquipable onEquip() called");
        this.holder = holder;
        this.holder.addAction(providedAction);
    }

    public void onUnequip() {
        //Debug.Log("ActionEquipable onUnequip() called");
        this.holder.removeAction(providedAction);
    }

    public string targetSlot() {
        return this.equipmentSlot;
    }
}

public class EquipablePipe : IEquipable {

    private string equipmentSlot;
    private Pipe<int> _pipe;

    private PMMob holder;

    public EquipablePipe(string equipmentSlot, Pipe<int> pipe) {
        this.equipmentSlot = equipmentSlot;
        if (pipe.getFlags().Count == 0) {
            throw new System.InvalidOperationException("Attempted to add a pipe with no flags into an IEquipable."
                + " Equipable pipes should always have at least one target flag. Pipe<int>: " + pipe.GetType());
        }
        this._pipe = pipe;
    }

    public void onEquip(PMMob holder) {
        this.holder = holder;
        holder.addOutPipe(this._pipe);
    }

    public void onUnequip() {
        this.holder.removeOutPipe(_pipe);
    }

    public string targetSlot() {
        return this.equipmentSlot;
    }

}


public static class testEquipableFactory {
    
    public static IEquipable makeSword() {

        HashSet<IEquipable> behaviors = new HashSet<IEquipable>();

        Flagable f = new Flagable();
        f.addFlag(BattleStats.ATTACK);
        Pipe<int> attackup = new PipeSum(new ExpireNever<Pipe<int>>(), f, 2);
        IEquipable addAttachBehavior = new EquipablePipe(EquipSlots.MAIN_HAND, attackup);

        PMAction slashAction = new ActionAttack(3);
        IEquipable slashBehavior = new ActionEquipable(EquipSlots.MAIN_HAND, slashAction);

        behaviors.Add(addAttachBehavior);
        behaviors.Add(slashBehavior);

        MultiBehaviorEquipable sword = new MultiBehaviorEquipable(EquipSlots.MAIN_HAND, behaviors);
        return sword;
    }
}