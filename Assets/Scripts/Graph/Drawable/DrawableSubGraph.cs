

/**
 * A simple wrapper for the SubGraph object that enforces that Nodes have positions (used for drawing or other operations)
 */
public class DrawableSubGraph<NPayload, EPayload> : SubGraph<NPayload, EPayload> where NPayload : IPositionable {

    public DrawableSubGraph() : base() { }
}
