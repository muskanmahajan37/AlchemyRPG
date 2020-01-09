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
    
    
    public Node<NPayload> inboundConnection;
    public Node<NPayload> outboundConnection;

    public SubGraph(List<Node<NPayload>> nodes) : base() {
        this.addNode(nodes[0]);
        this.addNode(nodes[1]);
        this.addNode(nodes[2]);

        this.createEdgeOneWay(nodes[0].nodeName, nodes[1].nodeName, default(EPayload));
        this.createEdgeOneWay(nodes[1].nodeName, nodes[2].nodeName, default(EPayload));

        this.inboundConnection = nodes[0];
        this.outboundConnection = nodes[2];
    }

    /**
     * Subgraphs need a list of their dangling inbound and outbound edges.
     * This is because, when an edge is created check if it's one of these in/outbounds that
     * is being filled. Then remove it from the dangling list. 
     * Outside the interface only sees the inbound and outbound connections options.
     * If a graph connects it's own outbound with it's own inbound, the count will drop
     * and nothing will be able to connect. 
     * 
     * Perhapse if you really wanted to you could force a connection.
     */

    public SubGraph<NPayload, EPayload> connectSubGraphs(SubGraph<NPayload, EPayload> otherGraph, EPayload connectionPayload) {
        /**
         * Merge the given "otherGraph" into this graph
         * A dangling output from this graph will be connected to a dangling input from the "otherGraph"
         * The otherGraph's dangling edges will be logged into this subGraph
         * 
         * NOTE: The returned graph IS this graph. Either should be used, while the otherGraph should
         * be discarded.
         */
        this.mergeGraphs(otherGraph);
        this.createEdgeOneWay(this.outboundConnection, otherGraph.inboundConnection, connectionPayload);
        this.outboundConnection = otherGraph.outboundConnection;
        return this;
    }

}


public class SuperGraph {


    public SuperGraph() {

        SubGraph<int, int> sub1 = new SubGraph<int, int>(new List<Node<int>>() {
            new Node<int>(5),
            new Node<int>(6),
            new Node<int>(7),
        });

        SubGraph<int, int> sub2 = new SubGraph<int, int>(new List<Node<int>>{
            new Node<int>(-11),
            new Node<int>(-12),
            new Node<int>(-13)
        });


        sub1.connectSubGraphs(sub2, 555);
    }



}