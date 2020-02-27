
/**
 * A simple wrapper for the TableGraph object that enforces that Nodes have positions (used for drawing or other operations)
 */
public class DrawableGraph<NPayload, EPayload> : TableGraph<NPayload, EPayload> where NPayload : IPositionable {

    public DrawableGraph() : base() { }

}