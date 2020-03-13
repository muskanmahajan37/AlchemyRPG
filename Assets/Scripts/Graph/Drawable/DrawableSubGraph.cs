

/**
 * A simple wrapper for the SubGraph object that enforces that Nodes have positions (used for drawing or other operations)
 */
public class DrawableSubGraph<NPayload, EPayload> : SubGraph<NPayload, EPayload> where NPayload : IPositionable {
    // TODO: Problem, I want this class to be castable into a DrawableGraph... 
    public DrawableSubGraph() : base() { }



    public new SubGraph<NPayload, EPayload> outputToInputRandom(SubGraph<NPayload, EPayload> otherGraph, EPayload connectionPayload)
    {
        /**
         * Merge the given "otherGraph" into this graph
         * A random dangling output from this graph will be connected to a random dangling input from the "otherGraph"
         * The otherGraph's dangling edges will be logged into this subGraph
         * 
         * NOTE: The returned graph IS this graph. Either should be used, while the otherGraph should
         * be discarded.
         */

        base.outputToInputRandom(otherGraph, connectionPayload);

    }
}
