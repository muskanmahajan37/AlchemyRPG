using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreRecTecController : ITecTreeController
{
    /**
     * A class that oversees all technological progress
     * 
     * This implimentation stores a preRec style of nodes => each node only cares about what it needs to have to unlock itself
     *      (Contrasted to a pointsTo style of nodes => each node knows what it helps to unlock)
     * 
     * This implimentation makes it easy to change the requirements of unlocking a technology for initial research on the fly
     */

    private Dictionary<string, ATecTreeNode> allNodes;
    private Dictionary<ATecTreeNode, TecTreeEntry> data;  // TODO: find a better name for this field 


    public PreRecTecController(List<ATecTreeNode> nodes) {
        /**
         * A tec controller needs a list of all ATecTreeNodes
         * These nodes should already have their pre-recs/ pointint to structure set up already. 
         * This list should contain the entire network
         */
         

    }

    // TODO: Is tecName enough when dealing with hash override ATecTreeNodes? Probably not... 
    //       But think of the use case of this function: Any point of code has a human readable tech name to use for programming. 
    public int hasResearched(string tecName) {
        /**
         * Returnes the number of times the given tech name has been researched
         */
        ATecTreeNode ttn = this.allNodes[tecName.ToLower()];
        return data[ttn].researchCount;
    }

    // TODO: Make this work with things that have the same string name
    public int forceDoResearch(string tecName, int researchPoints) {
        /**
         * Do research on the tecName TecTreeNode, regardless of the pre-recs for the node
         * 
         * Returns the amound of overflow research points. Overflow is the number of RP over the requiredResearch
         * A non 0 result impliess the tech is finished researching
         * 
         * // TODO: Move this functunality into the ResearchBar maybe? 
         * This will never cause the ResearchBar.currentResearch to go over the requiredResearch
         */
        ATecTreeNode ttn = this.allNodes[tecName.ToLower()];
        ResearchBar rb = this.data[ttn].progress;
        int overflow = Math.Max(0, (rb.currentResearch + researchPoints) - rb.requiredResearch);
        rb.currentResearch += researchPoints - overflow;
        if (overflow > 0)
        {
            finishedResearch(tecName);
        }
        return overflow;
    }
    
    private void finishedResearch(string tecName)
    {
        ATecTreeNode ttn = this.allNodes[tecName.ToLower()];
        ResearchBar researchProgress = this.data[ttn].progress;
        if (researchProgress.currentResearch < researchProgress.requiredResearch) {
            // Not enough research has been done
            return;
        }

        // Remove the newly finished research from the avaliable pool
        this.data[ttn].avaliableToResearch = false;
        



    }

    public IReadOnlyCollection<ATecTreeNode> getPreRecs(string tecName)
    {
        ATecTreeNode ttn = this.allNodes[tecName.ToLower()];
        return this.data[ttn].preRecs.AsReadOnly();
    }

    public IReadOnlyCollection<ATecTreeNode> getPointsTo(string tecName)
    {
        ATecTreeNode ttn = this.allNodes[tecName.ToLower()];
        return this.data[ttn].pointsTo.AsReadOnly();
    }

    public void addPointsTo(string n1, string n2)
    {
        // n1 now points to n2
        // n2 is now a prerec of n1

        // TODO: Better error handeling here. 
        Debug.Log("Danger: Adding a pointsTo relationship between nodes during gameplay");
        ATecTreeNode ttn1 = this.allNodes[n1.ToLower()];
        ATecTreeNode ttn2 = this.allNodes[n2.ToLower()];

        this.data[ttn1].pointsTo.Add(ttn2);
        this.data[ttn2].preRecs.Add(ttn1);
    }

    public void addPreRec(string tecName, string newPreRec)
    {
        // tecName is now has newPreRec as a pre requiset technology
        // newPreRec now points to tecName

        // TODO: Better error handeling here. 
        Debug.Log("Danger: Adding a preRec relationship between nodes during gameplay");
        ATecTreeNode ttnTecName = this.allNodes[tecName.ToLower()];
        ATecTreeNode ttnNewPreRec = this.allNodes[newPreRec.ToLower()];

        this.data[ttnTecName].preRecs.Add(ttnNewPreRec);
        this.data[ttnNewPreRec].pointsTo.Add(ttnTecName);
    }

    public bool disconnectNodes(string n1, string n2)
    {
        // Removes any reltionship between these two nodes

        Debug.Log("Danger: Removing relationship between nodes during gameplay");
        ATecTreeNode ttn1 = this.allNodes[n1.ToLower()];
        ATecTreeNode ttn2 = this.allNodes[n2.ToLower()];

        bool result = false;
        if (this.data[ttn1].preRecs.Contains(ttn2)) {
            // If n1 contains n2 as a prerec
            //  IE: if n2 points to n1
            this.data[ttn1].preRecs.Remove(ttn2);
            this.data[ttn2].pointsTo.Remove(ttn1);
            result = true;
        }

        // NOTE: the nodes could be pointing to eachother
        //       so we can't return after the above if logic
        if (this.data[ttn2].preRecs.Contains(ttn1)) {
            // If n2 contains n1 as a pre rec
            //  IE: if n1 points to n2
            this.data[ttn2].preRecs.Remove(ttn1);
            this.data[ttn1].pointsTo.Remove(ttn2);
            result = true;
        }

        return result;
    }

    private class TecTreeEntry
    {
        // A class to represent all the important values related to a node
        public int researchCount; // How many times has this node/entry been researched

        // TODO: These two fields are redundant, how to ensure that every node has the same idea of who their neighbors are? 
        public List<ATecTreeNode> preRecs; // What are all the preRecs for this node?
        public List<ATecTreeNode> pointsTo; // What does this tech help to unlock        

        public bool avaliableToResearch; // Is this node currently in the research pool? 
        public ResearchBar progress; // How much progress has been done to this node? 
    }



}
