using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TecTreeUtility : MonoBehaviour
{

    PreRecTecController tecController;

    // Start is called before the first frame update
    void Start()
    {

    }


    public static List<ATecTreeNode> buildTestTree() {
        List<ATecTreeNode> result = new List<ATecTreeNode>();
        ATecTreeNode n1 = new TecTreeNode(name: "tec1", era: 0, researchPointCost: 10);
        ATecTreeNode n2 = new TecTreeNode("tec2", 0, 20);
        ATecTreeNode n3 = new TecTreeNode("tec3", 0, 30);
        ATecTreeNode n4 = new TecTreeNode("tec4", 0, 40);
        result.Add(n1);
        result.Add(n2);
        result.Add(n3);
        result.Add(n4);

        n1.pointsTo(n2);  // n2.addPreRec(n1); 
        n1.pointsTo(n3);  // n3.addPreRec(n1);
        n2.pointsTo(n4);  // n4.addPreRec(n2); 

        return result;
    }

}

public class TecTreeProcessing
{
    Dictionary<string, ATecTreeNode> allNodes;
    Dictionary<string, HashSet<ATecTreeNode>> preRecs;   // TODO: Consider making this a set
    Dictionary<string, HashSet<ATecTreeNode>> pointsTo;  // TODO: Consider making this a set
    Dictionary<string, ResearchBar> researchProgress;

    Dictionary<int, HashSet<ATecTreeNode>> techsInEra;  // Group all the technolgies into their respective eras
    HashSet<ATecTreeNode> techFrontier;   // What tech is currently avaliable for research? 

    TecTreeProcessing(List<ATecTreeNode> nodes) {
        allNodes = new Dictionary<string, ATecTreeNode>(nodes.Count);
        preRecs = new Dictionary<string, HashSet<ATecTreeNode>>(nodes.Count);
        pointsTo = new Dictionary<string, HashSet<ATecTreeNode>>(nodes.Count);
        researchProgress = new Dictionary<string, ResearchBar>(nodes.Count);

        this.techsInEra = new Dictionary<int, HashSet<ATecTreeNode>>();
        this.techFrontier = new HashSet<ATecTreeNode>();

        foreach(ATecTreeNode ttn in nodes)
        {

            // TODO: Make sure this function still works after the ATecTreeNode chances
            // IE, an ATTN class object is just an authoring tool, and this is the converter from authroing tool to dictionary stuff. 
            //  Also, make things HashSets instead of Lists
            //  Also, impliment Eras as a usefull data metric
            //  Also, add all era 0 objects w/ no pre recs to the current tech horizon
            //    which means that we need to impliment the data storage of a tech horizon
            /**
             * Let's talk, what is an era?
             * Well... that's a good and important question
             * I can think of two examples of eras:
             *  1) Civ style where the era's don't really have any kind of impact but are nice to have arround
             *  2) Endless Legends style where a technology in an advanced era can't be researched untill a certain number
             *     of technologies in the previous era have been unlocked.
             *     
             *  Ok, so let's only consider option 2 for now.
             *  What are the possible interesting things we want to gate an era on?
             *      - % of techs finished in the previous era
             *      - % of techs finished in any previous eras
             *  So, we'll need another container class object! Yay
             *  
             *  An Era class object represents an era n.
             *  In order to research something in era n, first look u p the Era object for the value n.
             *  This Era object will provide a list of requirements 
             *      This list is in the form of a map (era number(int) -> % tech required to be researched (double in the range[0,1]))
             *      The keys of the map must be less than or equal to the era number n of the Era instantiation
             *      The value of the map is the % of technologies in that earlier era the must be researched before allowing any technology
             *          in the era n to begin research
             *          
             *  Who cares about the Era class object? Really the controller is the only thing that cares about the pre recs
             *  But authorable nodes need to know their era (but not nessicarialy the rules about the era? TODO: Potential design seperation here)
             *  
             *  
             */

            // Check if we need to initalize this node

            // Add ttn to allNodes
            if (allNodes.ContainsKey(ttn.getName)) {
                Debug.LogError("This tec tree already contains the node \"" + ttn.getName + "\"");
                // TODO Better error printing here, make it print out the entire atectree dictionary
                throw new System.Exception("Technology does NOT have a unique name: \"" + ttn.getName + "\"");
            }
            allNodes.Add(ttn.getName, ttn);

            // Find the pre-recs
            preRecs.Add(ttn.getName, ttn.getPreRecs);

            // For each preRec, have it point to this node
            foreach (ATecTreeNode preRecNode in ttn.getPreRecs)
            {
                if( ! pointsTo.ContainsKey(preRecNode.getName)) {
                    // If we've never seen this preRecNode before
                    pointsTo[preRecNode.getName] = new HashSet<ATecTreeNode>();
                }
                pointsTo[preRecNode.getName].Add(ttn);
            }

            // Set all research progress to 0
            researchProgress.Add(ttn.getName, new ResearchBar(ttn.getRequiredRP));

            // Sort it into it's era
            // TODO: Do we need to validate eras? make sure no negatives or like no era gaps? 
            if ( ! this.techsInEra.ContainsKey(ttn.getEra)) {
                this.techsInEra.Add(ttn.getEra, new HashSet<ATecTreeNode>());
            }
            this.techsInEra[ttn.getEra].Add(ttn);

            // Build up the tech frontier
            // TODO: Should we require every tech tree to start at era 0? Probably....
            if (ttn.getPreRecs.Count == 0 && ttn.getEra == 0) {
                // If there's no pre recs and the era is 0
                this.techFrontier.Add(ttn);
            }
        }
    }
}

public class ResearchBar
{
    public int currentResearch;
    public int requiredResearch;
    public ResearchBar(int requiredResearch) // TODO: Consider a constructor that also provides current Research too
    {
        this.currentResearch = 0;
        this.requiredResearch = requiredResearch;
    }
}
