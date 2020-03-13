using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubGraph<NPayload, EPayload> : TableGraph<NPayload, EPayload>{
    /*
     * A subgraph is a TableGraph that allows the downstream users to only consider
     * the "dangeling" inputs and outputs to the graph. 
     * 
     * A dangling edge is an edge that only has one node. For example, a dangling
     * input is an edge that only has a destination node but no source node. 
     * NOTE: These dangling edges don't actually exist, as in there is no Edge<..> 
     * object that represents them. They are simply logically considered and become
     * instantiated once the dangling has been resolved. 
     * 
     * SubGraphs are often made up of other SubGraphs that have been merged/ connected.
     */

    // For now, just a Type1 gate with 3 walkable areas
    // >--O--O--O-->
    
    // TODO: Consider making a set of dangling inbound Edge types not just nodes
    //       For instance, would someone want to author a map atom that has a specific 
    //       incoming/ outgoing edge? Probably not b/c edges are load zones?
    private List<string> danglingInboundNames = new List<string>();
    private List<string> danglingOutboundNames = new List<string>();

    public SubGraph() {

    }

    #region merging subgraphs

    public SubGraph<NPayload, EPayload> outputToInputRandom(SubGraph<NPayload, EPayload> otherGraph, EPayload connectionPayload) {
        /**
         * Merge the given "otherGraph" into this graph
         * A random dangling output from this graph will be connected to a random dangling input from the "otherGraph"
         * The otherGraph's dangling edges will be logged into this subGraph
         * 
         * NOTE: The returned graph IS this graph. Either should be used, while the otherGraph should
         * be discarded.
         */

        this.mergeGraphs(otherGraph);

        string outboundName = popRandomName(ref this.danglingOutboundNames);
        string inboundName = popRandomName(ref otherGraph.danglingInboundNames);

        this.createEdgeOneWay(outboundName, inboundName, connectionPayload);

        this.danglingInboundNames.AddRange(otherGraph.danglingInboundNames);
        this.danglingOutboundNames.AddRange(otherGraph.danglingOutboundNames);

        return this;
    }

    public SubGraph<NPayload, EPayload> inputToOutputRandom(SubGraph<NPayload, EPayload> otherGraph, EPayload connectionPayload) {
        /**
         * Merge the given "otherGraph" into this graph
         * A dangling input from this graph will be connected to a dangling output from the "otherGraph"
         * The otherGraph's dangling edges will be logged into this subGraph
         * The dangling "edges" that were used will be pruned from both the other graph and the merged lists of this graph
         * 
         * NOTE: The returned graph IS this graph. Either should be used, while the otherGraph should (probably)
         * be discarded.
         */
        this.mergeGraphs(otherGraph);

        string inboundName = popRandomName(ref this.danglingInboundNames);
        string outboundName = popRandomName(ref otherGraph.danglingOutboundNames);

        this.createEdgeOneWay(outboundName, inboundName, connectionPayload);

        this.danglingInboundNames.AddRange(otherGraph.danglingInboundNames);
        this.danglingOutboundNames.AddRange(otherGraph.danglingOutboundNames);

        return this;
    }

    private static string popRandomName(ref List<string> names) {
        /**
         * Returns a random element from the list of strings. The returned element is also removed
         * from the provided names list. 
         */

        // TODO: Hand roll a serilizable RNG and use that here instead of this one.
        int randomIndex = Random.Range(0, names.Count);
        string result = names[randomIndex];
        names.RemoveAt(randomIndex);
        return result;

    }

    #endregion

    #region Modify

    public bool markInbound(Node<NPayload> n) {
        if (! this.containsNode(n)) { return false; }
        this.danglingInboundNames.Add(n.nodeName);
        return true;
    }
    public bool markOutbound(Node<NPayload> n) { 
        if (! this.containsNode(n)) { return false; }
        this.danglingOutboundNames.Add(n.nodeName);
        return true;
    }
    public bool unmarkInbound(Node<NPayload> n) { return this.containsNode(n) && this.danglingInboundNames.Remove(n.nodeName); }
    public bool unmarkOutbound(Node<NPayload> n) { return this.containsNode(n) && this.danglingOutboundNames.Remove(n.nodeName); }

    #endregion

    #region Getters

    public bool isInbound(Node<NPayload> n) { return this.containsNode(n) && this.danglingInboundNames.Contains(n.nodeName); }
    public bool isOutbound(Node<NPayload> n) { return this.containsNode(n) && this.danglingOutboundNames.Contains(n.nodeName); }

    public int countDanglingInbound() { return this.danglingInboundNames.Count; }
    public int countDanglingOutbound() { return this.danglingOutboundNames.Count; }

    #endregion

}



// TODO: remove this? 
public class SuperGraph {


    public SuperGraph() {

        Debug.LogError("Depreciated code nocab");

        /*SubGraph<int, int> sub1 = new SubGraph<int, int>(new List<Node<int>>() {
            new Node<int>(5),
            new Node<int>(6),
            new Node<int>(7),
        });

        SubGraph<int, int> sub2 = new SubGraph<int, int>(new List<Node<int>>{
            new Node<int>(-11),
            new Node<int>(-12),
            new Node<int>(-13)
        });*/


        //sub1.outputToInput(sub2, 555);
    }



}