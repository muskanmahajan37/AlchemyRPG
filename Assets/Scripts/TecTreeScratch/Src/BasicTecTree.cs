using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTecTree
{
    // A Civ style tech tree is a tree.
    // Edges => pre-requisets for a tec
    //      All pre-requisets must be "touched" or meet to start working on a node
    // Nodes => Technology to unlock
    //      Technology unlocks simply modify perminate game variables
    //      Technology unlocks run an arbitrary code/ script when finished


}

public class TecTreeNode : ATecTreeNode {
    
    public TecTreeNode(string name, int era, HashSet<ATecTreeNode> preRecs, int researchPointCost) : 
        base(name: name, era: era, preRecs: preRecs, requiredRP: researchPointCost) { }

    public TecTreeNode(string name, int era, int researchPointCost) : 
        base(name: name, era: era, preRecs: new HashSet<ATecTreeNode>(), requiredRP: researchPointCost) { }

    override public bool otherRequirements() {
        /**
         * Check other non-standard precursor technology requirements
         * For example, check that the player has enough "creddits" to start this
         *           or check that the game is in a certain state
         *           
         * TODO: How to make this work?
         *       Should we build a controller "var look up table" for TecController and GameController?
         *          How would be ensure this is read only? 
         *       Should we codify more complex behavior in the controllers?
         *          IE: Count creddits != var lookup, so this would just call TecController.creddits() > 40
         *          
         *       Those would work, and don't really have any obvious major problems that I can see now
         *       However, consider would it be easier to let everything have access to everything?
         *       
         *       No. Consider Isaac, or Slay the Spire. Slay the Spire has a bunch of artifacts that 
         *       modify things in specific ways at specific times. Each artifact has it's own state. 
         *       Each card draw/ damage delt/ strength gained/ action in general doesn't look at every
         *       single artifact, the action probably goes through a modifier pipeline of artifacts.
         *       At each stage of the pipeline (ie, each artifact), the base action gets modified 
         *       
         *       So, in a perfect world I need to build a pipeline for actions. I can see problems if 
         *       there are conflicting things in the pipeline (like turn card draw into card discard, then 
         *       later a card discard back into a card draw. Order is clearly important)
         */

        // Basic TecTreeNode objects don't have other requirements. 
        return true;
    }

}
