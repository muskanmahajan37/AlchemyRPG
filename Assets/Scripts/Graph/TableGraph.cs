using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class TableGraph<NPayload, EPayload> {

    /**
     * So a few notes about the design problems with this TableGraph
     * First off, I can imagine graphs that don't have/ need an Edge payload.
     * However, I'm currently ignoring these types of graphs. Yes, a more
     * efficent system can be built with the assumption that the edges
     * don't cary objects, but that assumption is also restricting and I want
     * to be general right now.
     * 
     * Seccondly, This is a table representation of graphs. It's a centralized
     * data store, not a collection of nodes and edges. So, treat this 
     * more like a dictionary than a true graph. To get a node you need 
     * the node name or key.
     * 
     * Third, internally there is a mapping of nodes to edges. NOT node to
     * neighbor nodes. An edge will know both the source node and the dest
     * node. This means that a Node itself dosen't directly know it's 
     * neighbors.
     * Node -> Edge -> Other Node.
     * 
     * 
     * Nodes have their payload and their uuid name
     * 
     * Edges have two node UUIDs and a payload
     */

    // A mapping of Node uuid to edges
    private DictionarySet<string, Edge<NPayload, EPayload>> outboundEdges;
    private DictionarySet<string, Edge<NPayload, EPayload>> inboundEdges;

    private Dictionary<string, Node<NPayload>> allNodes;

    public TableGraph() {
        this.outboundEdges = new DictionarySet<string, Edge<NPayload, EPayload>>();
        this.inboundEdges = new DictionarySet<string, Edge<NPayload, EPayload>>();
        this.allNodes = new Dictionary<string, Node<NPayload>>();
    }

    #region Adders
    public Node<NPayload> addNode(Node<NPayload> newNode) {
        this.assertNONExistance(newNode.nodeName);
        this.allNodes.Add(newNode.nodeName, newNode);
        return newNode;
    }

    public Node<NPayload> addNode(NPayload payload, string nodeName) {
        Node<NPayload> newNode = new Node<NPayload>(payload, nodeName);
        return this.addNode(newNode);
    }

    public Node<NPayload> addNode(NPayload payload) {
        return addNode(payload, Guid.NewGuid().ToString());
    }

    public Edge<NPayload, EPayload> createEdgeOneWay(Node<NPayload> sourceNode, Node<NPayload> destNode, EPayload edgePayload) {
        /**
         * If the provided nodes don't exist in this graph, they will be added.
         * An edge will be created between the two.
         */
        if ( ! this.containsNode(sourceNode.nodeName)) { this.addNode(sourceNode); }
        if ( ! this.containsNode(destNode.nodeName))   { this.addNode(destNode); }
        // Else, the given nodes are already contained in this graph

        Edge<NPayload, EPayload> newEdge = new Edge<NPayload, EPayload>(sourceNode, destNode, edgePayload);
        this.outboundEdges.Add(sourceNode.nodeName, newEdge);
        this.inboundEdges.Add(destNode.nodeName, newEdge);
        return newEdge;
    }

    public Edge<NPayload, EPayload> createEdgeOneWay(string sourceName, string destName, EPayload edgePayload) {
        /**
         * An edge can only be created between two existing nodes.
         * If the node dosen't exist already this method will throw an error.
         */

        assertExistance(sourceName);
        assertExistance(destName);
        Node<NPayload> sourceNode = this.getNode(sourceName);
        Node<NPayload> destNode = this.getNode(destName);
        return this.createEdgeOneWay(sourceNode, destNode, edgePayload);
    }

    public TableGraph<NPayload, EPayload> mergeGraphs(TableGraph<NPayload, EPayload> otherGraph) {
        /**
         * Merge the given "otherGraph" into this graph
         * NOTE: The returned graph IS this graph. Either should be used, while the "otherGraph"
         * should be discarded. 
         */
        // Add all the KeyValuePairs from otherGraph into this graph
        otherGraph.allNodes.ToList().ForEach(kvp => this.allNodes.Add(kvp.Key, kvp.Value));

        otherGraph.inboundEdges.ToList().ForEach(kvp => this.inboundEdges.Add(kvp.Key, kvp.Value));
        otherGraph.outboundEdges.ToList().ForEach(kvp => this.outboundEdges.Add(kvp.Key, kvp.Value));
        return this;
    }

    #endregion


    #region Removers
    public bool removeNode(string nodeName) {
        // Removes the given node name. Return true => node removed sucessfully
        if ( ! this.containsNode(nodeName)) { return false; }

        bool result = true;
        result = result && this.allNodes.Remove(nodeName);
        result = result && this.inboundEdges.Remove(nodeName);
        result = result && this.outboundEdges.Remove(nodeName);
        return result;
    }

    // TODO: This function is slightly inefficent, try and improve it
    public bool removeEdge(string sourceNodeName, string destNodeName) {
        /**
         * Remove the edge between the two privided nodes. 
         * 
         * If the nodes do NOT exist, then an error is thrown.
         * If the nodes do exist, but the edge dose not, then false if returned.
         * If the nodes and edge do exist, the edge will be removed and true returned. 
         * 
         * NOTE: This function is O(n) where n is proportional to the number of edges 
         *       in the given NodeName (smallest n between the two)
         */

        assertExistance(sourceNodeName);
        assertExistance(destNodeName);

        HashSet<Edge<NPayload, EPayload>> outEdge = this.outboundEdges.safeGet(sourceNodeName);
        HashSet<Edge<NPayload, EPayload>> inEdge  = this.inboundEdges.safeGet(destNodeName);

        bool outIsLarger = outEdge.Count > inEdge.Count;

        if (outIsLarger) {
            // If the smaller set is less than the small threshold
            return findAndRemoveCommonEdge(outEdge, inEdge);
        } else {
            // If the smaller set is less than the small thershold
            return findAndRemoveCommonEdge(inEdge, outEdge);
        } /*else {
            // Else both sets are fairly large
            return findAndRemoveCommonEdgeConstTime(outEdge, inEdge, sourceNodeName, destNodeName);
        }*/

    }

    private bool findAndRemoveCommonEdge(HashSet<Edge<NPayload, EPayload>> smallSet, HashSet<Edge<NPayload, EPayload>> largeSet) {
        // For smaller sized sets, itterating through may be faster than the ConstTime approapch
        foreach(Edge<NPayload, EPayload> edge in smallSet) {
            if (largeSet.Contains(edge)) {
                largeSet.Remove(edge);
                smallSet.Remove(edge);
            }
        }
        return false;
    }


    /*
     * Function removed b/c the GetHashCode() for an edge was changed. The cheese employed no longer works :( 
     * 
    private bool findAndRemoveCommonEdgeConstTime(HashSet<Edge<NPayload, EPayload>> setA, 
                                                  HashSet<Edge<NPayload, EPayload>> setB,
                                                  string sourceNodeName,
                                                  string destNodeName) {
        // There's a little cheese here. 
        // Because I know how the Edge GetHashCode() function works, I can leverage that to instantly look up/ remove an other
        // edge from the HashSet. If you're reading this, it means that I did something wrong and I am deeply sorry. 
        // If this dosen't work as I thought it would, use the more honest "findAndRemoveCommonEdge(...)" function above.
        // Basically, for ultra dense graphs this function will probably be faster. For more sparse graphs, this may be slower. 
        Edge<NPayload, EPayload> tempEdge = new Edge<NPayload, EPayload>(this.getNode(sourceNodeName), this.getNode(destNodeName), default(EPayload));
        bool result = setA.Remove(tempEdge);
        result |= setB.Remove(tempEdge);
        return result;
    }
    */
    #endregion


    #region Getters

    // NOTE: making a getNeighbors function will be confusing (down stream vs up stream neighbors)
    // and inefficent (because everythign is stored as edges, not direct connections).

    public bool containsNode(string nodeName) {
        return this.allNodes.ContainsKey(nodeName);
    }
    public bool containsNode(Node<NPayload> n) { return this.containsNode(n.nodeName); }

    public Node<NPayload> getNode(string nodeName) {
        assertExistance(nodeName);
        return this.allNodes[nodeName];
    }
    
    public HashSet<Edge<NPayload, EPayload>> getOutboundEdges(Node<NPayload> node) { return this.getOutboundEdges(node.nodeName); }
    public HashSet<Edge<NPayload, EPayload>> getOutboundEdges(string nodeName) {
        assertExistance(nodeName);
        return this.outboundEdges.safeGet(nodeName);
    }

    public HashSet<Edge<NPayload, EPayload>> getInboundEdges(Node<NPayload> node) { return this.getInboundEdges(node.nodeName); }
    public HashSet<Edge<NPayload, EPayload>> getInboundEdges(string nodeName) {
        assertExistance(nodeName);
        return this.inboundEdges.safeGet(nodeName);
    }

    public Node<NPayload> entryPoint() {
        /**
         * Pull a "random" node from this.allNodes
         * NOTE, if there are multiple dis-connected regions in this graph then a single entryPoint may not suffice.
         * TODO: Solve the above problem.
         */
        if (this.allNodes.Count == 0) { throw new IndexOutOfRangeException("A graph that has no nodes has no entry point"); }
        return this.allNodes.ElementAt(0).Value;
    }

    #endregion




    private void assertNONExistance(Node<NPayload> node) { this.assertNONExistance(node.nodeName); }
    private void assertNONExistance(string nodeName) {
        if (this.containsNode(nodeName)) {
            throw new System.Exception("This graph DOSE contain the given node name: \"" + nodeName + "\". NOTE: The assertNONExistance() function was called. ");
        }
    }

    private void assertExistance(Node<NPayload> node) { this.assertExistance(node.nodeName); }
    private void assertExistance(string nodeName) {
        if ( ! this.containsNode(nodeName)) {
            throw new System.Exception("This graph DOSE contain the given node name: \"" + nodeName + "\".");
        }
    }

}

// TODO: Consider making this a poolable object
public class Node<NPayload> {
    /**
     * This is just a helpful package of (payload, nodeUUID)
     */

    public NPayload payload;

    // The UUID name of this node, default is memory location
    public readonly string nodeName;


    public Node(NPayload payload) {
        this.nodeName = Guid.NewGuid().ToString();
        this.payload = payload;
    }

    public Node(NPayload payload, string nodeName) {
        this.nodeName = nodeName;
        this.payload = payload;
    }

    public override int GetHashCode() {
        // Two Nodes are equal if they have the same nodeName uuid
        return this.nodeName.GetHashCode();
    }

    public override bool Equals(object obj) {
        // Two Nodes are equal if they have the same nodeName uuid 
        Node<NPayload> item = obj as Node<NPayload>;
        if (item == null) { return false; }
        return this.nodeName.Equals(item.nodeName);
    }

    public override string ToString() {
        return this.nodeName;
    }

}

public class Edge<NPayload, EPayload> {
    /**
     * This is just a helpful package that contains {
     *    Edge payload
     *    Source node
     *    Dest node
     *    isOneWay
     * }
     */

    // Should the edge know what is the payload type of the Node?
    // Should the node know the payload type of the Edge?

    // What is the purpose of this class?
    // Should the main class just have generic types?
    // The dictionary class uses an internal KeyValuePair type, should I 
    // have a Node and Edge helper internal type? 

    public readonly Node<NPayload> source;
    public readonly Node<NPayload> destination;
//    public bool isOneWay;

    public EPayload payload;

    public readonly string uuid;

    public Edge(Node<NPayload> source, Node<NPayload> dest, EPayload payload) {
        this.payload = payload;
        this.source = source;
        this.destination = dest;

        this.uuid = Guid.NewGuid().ToString();
    }

    public override int GetHashCode() {
        return this.uuid.GetHashCode();
    }

    public override bool Equals(object obj) {
        // Two edges are equal if they have the same source and destination

        // TODO: Consider having multiple edges between two nodes
        Edge<NPayload, EPayload> item = obj as Edge<NPayload, EPayload>;
        if (item == null) { return false; }
        return this.uuid.Equals(item.uuid);
    }
}
