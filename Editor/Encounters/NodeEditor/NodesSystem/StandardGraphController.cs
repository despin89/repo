namespace GD.EditorExtentions
{
    public class StandardGraphController
    {
        #region Constructors

        #endregion

        public void Register()
        {
            EventManager.OnCreateGraph += this.OnCreate;
            EventManager.OnFocusGraph += this.OnFocus;
            EventManager.OnCloseGraph += this.OnClose;
            EventManager.OnLinkEdge += this.OnLink;
            EventManager.OnUnLinkSockets += this.OnUnLink;
            EventManager.OnUnLinkedSockets += this.OnUnLinked;
            EventManager.OnAddedNode += this.OnNodeAdded;
            EventManager.OnNodeRemoved += this.OnNodeRemoved;
            EventManager.OnChangedNode += this.OnNodeChanged;
            EventManager.OnFocusNode += this.OnFocusNode;
            EventManager.OnEditorWindowOpen += this.OnWindowOpen;
        }

        private void OnWindowOpen()
        {
        }

        public void OnCreate(Graph graph)
        {
        }

        public void OnFocus(Graph graph)
        {
            Log.Info("OnFocus " + graph);
        }

        public void OnClose(Graph graph)
        {
            Log.Info("OnClose " + graph);
        }

        // ======= Events =======
        public void OnLink(Graph graph, Connection connection)
        {
            Log.Info("OnLink: Node " + connection.Output.Parent.Id + " with Node " + connection.Input.Parent.Id);
            graph.UpdateDependingNodes(connection.Output.Parent);
        }

        public void OnUnLink(Graph graph, AbstractSocket s01, AbstractSocket s02)
        {
            // Log.Info("OnUnLink: Node " + s01.Connection.Output.Parent.Id + " from Node " + s02.Connection.Input.Parent.Id);
        }

        public void OnUnLinked(Graph graph, AbstractSocket s01, AbstractSocket s02)
        {
            Log.Info("OnUnLinked: Socket " + s02 + " and Socket " + s02);
            AbstractSocket input = s01.IsInput() ? s01 : s02;
            graph.UpdateDependingNodes(input.Parent);
        }

        public void OnNodeAdded(Graph graph, Node node)
        {
            Log.Info("OnNodeAdded: Node " + node.GetType() + " with id " + node.Id);
        }

        public void OnNodeRemoved(Graph graph, Node node)
        {
            Log.Info("OnNodeRemoved: Node " + node.GetType() + " with id " + node.Id);
        }

        public void OnNodeChanged(Graph graph, Node node)
        {
            Log.Info("OnNodeChanged: Node " + node.GetType() + " with id " + node.Id);
            graph.UpdateDependingNodes(node);
        }

        public void OnFocusNode(Graph graph, Node node)
        {
            Log.Info("OnFocus: " + node.Id);
        }
    }
}