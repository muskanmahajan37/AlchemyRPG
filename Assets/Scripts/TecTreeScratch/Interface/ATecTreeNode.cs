using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ATecTreeNode 
{
    /**
     * Just the data required to build the relationships between technologies.
     * 
     * Any data related to the research difficulty, or progress of a technology is only stored in the TecController
     * 
     * Well.... This is really an authoring tool
     * And authoring can require setting the dificulty of a technology
     * But not the progress!
     */

    // TODO: make sure this is in line with the hash function
    protected string name;
    public string getName { get { return name.ToLower(); } }
    // Every technology should have a name. Unique is required (TODO: Enforce this in TecTreeUtility.cs)

    protected int era;
    public int getEra { get { return this.era; } }
    
    protected int requiredRP; // How much total research points need to be spent on this?
    public int getRequiredRP { get { return requiredRP; } }
    

    // TODO: Should we use this or points to?
    //       This class of objects is used most only during the authoring stages
    //       (TecControllers care about preRecs and pointsTo relationships)
    //       But we can take memory shortcuts? Only need one of these I think.
    //       But can build a pontsTo() function also no prob
    protected HashSet<ATecTreeNode> preRecs;
    public HashSet<ATecTreeNode> getPreRecs { get { return this.preRecs; } } 
                                           // What must be unlocked before this node
                                           // Also could be considered the "parent" nodes to this node
                                           // NOTE: This can be an empty list
                                           // TODO: Should we use preRecs or pointsTo? 


    protected ATecTreeNode(string name, int era, HashSet<ATecTreeNode> preRecs, int requiredRP) {
        this.name = name;
        this.era = era;
        this.requiredRP = requiredRP;
        this.preRecs = preRecs;
    }


    public void addPreRec(ATecTreeNode newPreRec)
        { this.preRecs.Add(newPreRec); }

    public void addPreRec(List<ATecTreeNode> newPreRecs)
        { this.preRecs.UnionWith(newPreRecs); }

    public void pointsTo(ATecTreeNode otherNode)
        { otherNode.addPreRec(this); }

    public void pointsTo(List<ATecTreeNode> otherNodes) {
        foreach(ATecTreeNode otherNode in otherNodes) {
            otherNode.addPreRec(this);
        }
    }
   
    public override int GetHashCode()
        { return this.name.ToLower().GetHashCode(); }

    public override bool Equals(object obj)
        { return this.GetHashCode() == ((ATecTreeNode)obj).GetHashCode(); }

    public abstract bool otherRequirements();
}
