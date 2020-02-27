using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawableScratch : MonoBehaviour {

    public GameObject objForNode;

    void Start() {

        DrawableSubGraph<PositionUtility, int> testingGraph = new DrawableSubGraph<PositionUtility, int>();

        Node<PositionUtility> node00 = testingGraph.addNode(new PositionUtility(0, 0), "00");
        Node<PositionUtility> node10 = testingGraph.addNode(new PositionUtility(1, 0), "10");
        Node<PositionUtility> node11 = testingGraph.addNode(new PositionUtility(1, 1), "11");
        Node<PositionUtility> node21 = testingGraph.addNode(new PositionUtility(2, 1), "21");

        testingGraph.createEdgeOneWay(node00, node11, 999);
        testingGraph.createEdgeOneWay(node11, node10, 999);
        testingGraph.createEdgeOneWay(node11, node21, 999);



        HashSet<Node<PositionUtility>> seenNodes = new HashSet<Node<PositionUtility>>();
        Queue<Node<PositionUtility>> frontieer = new Queue<Node<PositionUtility>>();
        frontieer.Enqueue(node00);
        while(frontieer.Count > 0) {
            Node<PositionUtility> currentNode = frontieer.Dequeue();
            if (seenNodes.Contains(currentNode)) { continue; }

            seenNodes.Add(currentNode);

            Transform obj = GameObject.Instantiate(objForNode).GetComponent<Transform>();
            obj.position = currentNode.payload.position();
            Debug.Log($"Drawing current node: {currentNode.nodeName}");

            HashSet<Edge<PositionUtility, int>> allEdges = testingGraph.getOutboundEdges(currentNode);
            allEdges.UnionWith(testingGraph.getInboundEdges(currentNode));

            foreach (Edge<PositionUtility, int> edge in allEdges) {
                Node<PositionUtility> neighborNode = edge.destination;
                frontieer.Enqueue(neighborNode);
            }
        }
    }

}
