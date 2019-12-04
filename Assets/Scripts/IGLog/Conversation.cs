using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TestConversation
{

    public void test()
    {
        ConvoNode n1 = new ConvoNode("1");
        ConvoNode n2 = new ConvoNode("2");
        ConvoNode n3 = new ConvoNode("3");

        n1.addEdge(new ConvoEdge("edge 1", n2));  // An option for n1 is e1, which points to n2
        n2.addEdge(new ConvoEdge("edge 2", n3));
    }
}


public class Conversation
{
    /*
     * A conversation is a map with edges and nodes.
     * 
     * A node represents information being delivered to the player.
     * An edge represents options for the player to choose.
     * 
     * Edges are one way and lead to a node.
     * Nodes have a list of edges
     * 
     * Nodes are only aware of edges pointing out of itself.
     *   Nodes do NOT know of edges leading into itself.
     *   
     * Edges can have requirements. IE: They can be blocked depending on the current state of the game
     * 
     */



}

class ConvoEdge
{
    // Represents a dialogue option for a player to pick

    string text;
    // TODO: How to represent pre-requisets for conversation options.
    // TODO: The first of these pre-reqs should be mood. Assume a conversation is a 1v1 action (currently don't have to track multiple moods in 1 convo)

    ConvoNode nextNode; // The node this edge is pointing to

    public ConvoEdge(string text, ConvoNode nextNode)
    {
        this.text = text;
        this.nextNode = nextNode;
    }

    public void updateNextNode(ConvoNode n)
    {
        this.nextNode = n;
    }

}


class ConvoNode
{
    string text;
    List<ConvoEdge> edges = new List<ConvoEdge>(5);

    public ConvoNode(string text)
    {
        this.text = text;
    }

    public void addEdge(ConvoEdge e)
    { this.edges.Add(e); }

    public void addEdge(List<ConvoEdge> edges)
    { this.edges.AddRange(edges); }

}
