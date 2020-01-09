using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTableGraph : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        simpleTest();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public static void simpleTest() {

        TableGraph<int, int> graph = new TableGraph<int, int>();

        string n1UUID = "node 1";
        graph.addNode(10, n1UUID);
        string n2UUID = "node 2";
        graph.addNode(11, n2UUID);

        graph.createEdgeOneWay(n1UUID, n2UUID, 9999);
        
        // N1
        assertTrue(graph.containsNode(n1UUID));

        assertEquals(graph.getNode(n1UUID).payload, 10);
        assertEquals(graph.getNode(n1UUID).nodeName, n1UUID);

        assertEquals(0, graph.getInboundEdges(n1UUID).Count, "inbound edges for n1 should be 0");
        assertEquals(1, graph.getOutboundEdges(n1UUID).Count, "outbound edges for n1 should be 1");

        // N2
        assertTrue(graph.containsNode(n2UUID));

        assertEquals(graph.getNode(n2UUID).payload, 11);
        assertEquals(graph.getNode(n2UUID).nodeName, n2UUID);

        assertEquals(1, graph.getInboundEdges(n2UUID).Count, "inbound edges for n2 should be 1");
        assertEquals(0, graph.getOutboundEdges(n2UUID).Count, "outbound edges for n2 should be 0");


        Debug.Log("Simple Test passed");


        // Remove some stuff
        graph.removeEdge(n1UUID, n2UUID);

        assertEquals(0, graph.getOutboundEdges(n1UUID).Count);
        assertEquals(0, graph.getInboundEdges(n1UUID).Count);

        //////
        // Test const time removal
        Edge<int, int> targetEdge = graph.createEdgeOneWay(n1UUID, n2UUID, -10);
        for (int i = 0; i < 20; i++) {
            Node<int> newNode = graph.addNode(i);
            graph.createEdgeOneWay(graph.getNode(n1UUID), newNode, i * 20);
        }
        assertEquals(21, graph.getOutboundEdges(n1UUID).Count, "outbound edges for n1 should be 21");
        assertEquals(1, graph.getInboundEdges(n2UUID).Count, "inbound edges for n2 should be 1");

        graph.removeEdge(n1UUID, n2UUID);

        assertEquals(20, graph.getOutboundEdges(n1UUID).Count, "One edge should have been removed.");
        assertTrue( ! graph.getOutboundEdges(n1UUID).Contains(targetEdge), "Should not contain target edge");

        Debug.Log("Removal tests pass");
    }


    private static void assertTrue(bool condition, string message = "") {
        if (!condition) {
            throw new System.Exception("Assertion is not true. Message: " + message);
        }
    }


    private static void assertEquals(int left, int right, string message = "") {
        if (left != right) {
            throw new System.Exception("Left != right:" + left + " != " + right + ".  Message: " + message);
        }
    }

    private static void assertEquals(string left, string right, string message = "") {
        if ( 0 != string.Compare(left, right) ) {
            throw new System.Exception("Left != right:" + left + " != " + right + ".  Message: " + message);
        }
    }
}
