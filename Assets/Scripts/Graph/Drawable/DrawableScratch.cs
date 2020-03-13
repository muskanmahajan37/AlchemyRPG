using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawableScratch : MonoBehaviour {

    public GameObject objForNode;

    void Start() {

        float y0 = Mathf.Sin(0);
        float x0 = Mathf.Cos(0);

        for (int i = 0; i < 4; i++) {
            float radians = (Mathf.PI / 2) * i;
            Debug.Log($"90deg * {i} :  ({Mathf.Cos(radians)} ,{Mathf.Sin(radians)} )");
        }


        DrawableSubGraph<PositionUtility, int> graph = createTypeOneAtom();



        HashSet<Node<PositionUtility>> seenNodes = new HashSet<Node<PositionUtility>>();
        Queue<Node<PositionUtility>> frontieer = new Queue<Node<PositionUtility>>();
        frontieer.Enqueue(graph.entryPoint());
        while (frontieer.Count > 0) {
            Node<PositionUtility> currentNode = frontieer.Dequeue();
            if (seenNodes.Contains(currentNode)) { continue; }

            seenNodes.Add(currentNode);

            Transform obj = GameObject.Instantiate(objForNode).GetComponent<Transform>();
            obj.position = currentNode.payload.position();
            Debug.Log($"Drawing current node: {currentNode.nodeName}");

            HashSet<Edge<PositionUtility, int>> allEdges = graph.getOutboundEdges(currentNode);
            allEdges.UnionWith(graph.getInboundEdges(currentNode));

            foreach (Edge<PositionUtility, int> edge in allEdges) {
                frontieer.Enqueue(edge.destination);
            }
        }
    }





    public DrawableSubGraph<PositionUtility, int> createTypeOneAtom() {
        DrawableSubGraph<PositionUtility, int> typeOneAtom = new DrawableSubGraph<PositionUtility, int>();
        Node<PositionUtility> origion = typeOneAtom.addNode(new PositionUtility(0, 0));

        // TODO: Better random numbers here
        float radians = Random.Range(0, 4) * (Mathf.PI / 2.0f);
        float xDelta = Mathf.Cos(radians);
        float yDelta = Mathf.Sin(radians);

        Node<PositionUtility> neighbor = typeOneAtom.addNode(new PositionUtility(xDelta, yDelta));

        typeOneAtom.createEdgeOneWay(origion, neighbor, 0);
        typeOneAtom.createEdgeOneWay(neighbor, origion, 0);

        typeOneAtom.markInbound(origion);
        typeOneAtom.markOutbound(neighbor);

        return typeOneAtom;
    }










    private void example() {
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
        while (frontieer.Count > 0)
        {
            Node<PositionUtility> currentNode = frontieer.Dequeue();
            if (seenNodes.Contains(currentNode)) { continue; }

            seenNodes.Add(currentNode);

            Transform obj = GameObject.Instantiate(objForNode).GetComponent<Transform>();
            obj.position = currentNode.payload.position();
            Debug.Log($"Drawing current node: {currentNode.nodeName}");

            HashSet<Edge<PositionUtility, int>> allEdges = testingGraph.getOutboundEdges(currentNode);
            allEdges.UnionWith(testingGraph.getInboundEdges(currentNode));

            foreach (Edge<PositionUtility, int> edge in allEdges)
            {
                Node<PositionUtility> neighborNode = edge.destination;
                frontieer.Enqueue(neighborNode);
            }
        }
    }
}
