using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITecTreeController
{

    int hasResearched(string tecName);
    int forceDoResearch(string tecName, int researchPoints);

    IReadOnlyCollection<ATecTreeNode> getPreRecs(string tecName);
    IReadOnlyCollection<ATecTreeNode> getPointsTo(string tecName);


    // Danger: 
    // If you're using these functions you're probably doing it wrong. Please setup entire tech tree using the TecTreeUtility.cs file/ classes
    // Only use these functions if you want a dynamic tech tree that changes with game play (NOT ADVISED!, but also not impossible) 
    void addPointsTo(string n1, string n2);  // n1 -> n2   or "n1 is now a pre rec for n2"
    void addPreRec(string tecName, string newPreRec);  //  newPreRec -> tecName  or "newPreRec is now a pre rec for tecName"

    // TODO: This might get weird if the node is a prerec of itself or if n1 <-> n2
    bool disconnectNodes(string n1, string n2); // If there's a relationship between the given technologies, cut it regardless of who points to who
                                                // True result => there was a relationship and it's gone now
                                                // False result => there was NOT a relationship, so it's still gone


}
