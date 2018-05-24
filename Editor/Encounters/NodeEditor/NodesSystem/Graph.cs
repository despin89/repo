namespace GD.EditorExtentions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Game.EncountersSystem;
    using UnityEditor;
    using UnityEngine;

    [Serializable]
    public class Graph
    {
        #region Fields

        private bool _invalidating;

        [NonSerialized]
        public bool TriggerEvents = true;

        private List<Node> _nodes = new List<Node>();

        [SerializeField]
        public string Name;

        #endregion

        public string GetEncounterTag()
        {
            var name = this._nodes.OfType<StartNode>().First().Encounter.Name;
            return string.IsNullOrEmpty(name) ? "NEW_ENCOUNTER" : name;
        }

        /// <summary>Returns an id for a Node that is unique for this Graph.</summary>
        /// <returns> An id for a Node that is unique for this Graph.</returns>
        public int ObtainUniqueNodeId()
        {
            int tmpId = 0;
            while (this.GetNode(tmpId) != null) tmpId++;

            return tmpId;
        }

        /// <summary>
        ///     Creates a Node of the given type. Type must inherit from Node.
        ///     Does not add the Node to the Graph.
        /// </summary>
        /// <returns>The created Node of the given Type.</returns>
        public Node CreateNode<T>()
        {
            return this.CreateNode<T>(this.ObtainUniqueNodeId());
        }

        /// <summary>
        ///     Creates a Node of the given type with the assigned id. Type must inherit from Node.
        ///     Does not add the Node to the Graph.
        /// </summary>
        /// <returns>The created Node of the given Type with the assigned id.</returns>
        public Node CreateNode<T>(int id)
        {
            return this.CreateNode(typeof(T), id);
        }

        /// <summary>
        ///     Creates a Node of the given type. Type must inherit from Node.
        ///     Does not add the Node to the Graph.
        /// </summary>
        /// <param name="type">The Type of the Node to create.</param>
        /// <returns>The created Node of the given Type.</returns>
        public Node CreateNode(Type type)
        {
            return this.CreateNode(type, this.ObtainUniqueNodeId());
        }

        /// <summary>
        ///     Creates a Node of the given type with the assigned id. Type must inherit from Node.
        ///     Does not add the Node to the Graph.
        /// </summary>
        /// <param name="type">The Type of the Node to create.</param>
        /// <param name="id">The id of the Node to create.</param>
        /// <returns>The created Node of the given Type with the assigned id.</returns>
        public Node CreateNode(Type type, int id)
        {
            if (type == null) return null;
            try
            {
                return (Node) Activator.CreateInstance(type, id, this);
            }
            catch (Exception exception)
            {
                Debug.LogErrorFormat("Node {0} could not be created " + exception.Message, type.FullName);
            }

            return null;
        }

        /// <summary>Returns the Node with the assigned id or null.</summary>
        /// <param name="nodeId">The id of the Node to get.</param>
        /// <returns>The Node with the assigned id or null.</returns>
        public Node GetNode(int nodeId)
        {
            if (this._nodes == null) return null;

            foreach (Node node in this._nodes) if (node.Id == nodeId) return node;

            return null;
        }

        /// <summary>Returns the count of Nodes in this Graph.</summary>
        /// <returns>The count of Nodes in this Graph.</returns>
        public int GetNodeCount()
        {
            return this._nodes.Count;
        }

        /// <summary>Returns the Node at the assigned index.</summary>
        /// <param name="index">The index of the Node to get.</param>
        /// <returns>The Node at the assigned index.</returns>
        public Node GetNodeAt(int index)
        {
            if (index >= this._nodes.Count) return null;

            return this._nodes[index];
        }

        /// <summary>
        ///     Adds a node to the Graph. Does not add Nodes with an id that is already taken.
        ///     Triggers a 'AddedNode' event.
        /// </summary>
        /// <param name="node">The Node to add.</param>
        /// <returns>True if the node was added.</returns>
        public bool AddNode(Node node)
        {
            if (this.GetNode(node.Id) != null) return false;
            this._nodes.Add(node);
            if (this.TriggerEvents) EventManager.TriggerOnAddedNode(this, node);
            return true;
        }

        /// <summary>Removes the assigned Node from this Graph.</summary>
        /// <param name="node">The Node to remove.</param>
        /// <returns>True if the Node was removed.</returns>
        public bool RemoveNode(Node node)
        {
            if (node == null) return false;

            foreach (AbstractSocket socket in node.Sockets) this.UnLink(socket);

            bool removed = this._nodes.Remove(node);
            if (this.TriggerEvents) EventManager.TriggerOnNodeRemoved(this, node);
            return removed;
        }

        /// <summary>Removes the Node with the assigned id from this Graph.</summary>
        /// <param name="id">The id of the Node to remove.</param>
        /// <returns>True if the Node was removed.</returns>
        public bool RemoveNode(int id)
        {
            return this.RemoveNode(this.GetNode(id));
        }

        public bool AreConected(InputSocket inputSocket, OutputSocket outputSocket)
        {
            if ((inputSocket == null) || (outputSocket == null) || (inputSocket.Connection == null) ||
                (outputSocket.Connections.Count == 0)) return false;

            return outputSocket.Connections.Contains(inputSocket.Connection);
        }

        /// <summary>Unlinkes the assigned sockets. Triggeres 'Unlink' events.</summary>
        public void UnLink(InputSocket inputSocket, OutputSocket outputSocket)
        {
            if ((inputSocket == null) || (outputSocket == null) || !inputSocket.IsConnected() ||
                !outputSocket.IsConnected()) return;
            if (!this.AreConected(inputSocket, outputSocket)) return;

            if (this.TriggerEvents)
            {
                EventManager.TriggerOnUnLinkSockets(this, inputSocket, outputSocket);
            }

            var outEnc = outputSocket.Parent as EncounterNodeBase;
            var inEnc = inputSocket.Parent as EncounterNodeBase;
            outEnc.Encounter.Outputs.Remove(inEnc.Encounter.Id);

            int index = outputSocket.Connections.IndexOf(inputSocket.Connection);
            if (index > -1)
            {
                outputSocket.Connections[index].Input = null;
                outputSocket.Connections[index].Output = null;
                outputSocket.Connections.RemoveAt(index);
            }

            inputSocket.Connection.Input = null;
            inputSocket.Connection.Output = null;
            inputSocket.Connection = null;

            if (this.TriggerEvents)
            {
                EventManager.TriggerOnUnLinkedSockets(this, inputSocket, outputSocket);
            }
        }

        public void UnLink(AbstractSocket socket)
        {
            if ((socket == null) || !socket.IsConnected()) return;


            if (socket.IsInput())
            {
                InputSocket inputSocket = (InputSocket) socket;
                if (inputSocket.Connection != null) this.UnLink(inputSocket, inputSocket.Connection.Output);
            }

            if (socket.IsOutput())
            {
                OutputSocket outputSocket = (OutputSocket) socket;
                Connection[] connectionCopy = new Connection[outputSocket.Connections.Count];
                outputSocket.Connections.CopyTo(connectionCopy);
                foreach (Connection edge in connectionCopy)
                {
                    this.UnLink(edge.Input, outputSocket);
                }
            }
        }

        public bool Link(InputSocket inputSocket, OutputSocket outputSocket)
        {
            if (!this.CanBeLinked(inputSocket, outputSocket))
            {
                Debug.LogWarning("Sockets can not be linked.");
                return false;
            }

            if (inputSocket.Type == outputSocket.Type)
            {
                Connection connection = new Connection(outputSocket, inputSocket);
                inputSocket.Connection = connection;
                outputSocket.Connections.Add(connection);

                var outEnc = outputSocket.Parent as EncounterNodeBase;
                var inEnc = inputSocket.Parent as EncounterNodeBase;
                outEnc.Encounter.Outputs.Add(inEnc.Encounter.Id);

                if (this.TriggerEvents)
                {
                    EventManager.TriggerOnLinkEdge(this, connection);
                }
            }

            return true;
        }

        private void StartVisitRun()
        {
            if (this._invalidating)
            {
                throw new UnityException("Graph is already invalidating");
            }

            this._invalidating = true;
            this.ResetVisitCount();
        }

        public void ResetVisitCount()
        {
            foreach (Node node in this._nodes)
            {
                node.VisitCount = 0;
            }
        }

        private void EndVisitRun()
        {
            this._invalidating = false;
        }

        public void UpdateDependingNodes(Node node)
        {
            this.StartVisitRun();
            this.UpdateOutputPath(node);
            this.EndVisitRun();
        }

        /// <summary>
        ///     This method follows the ouput path of the node and updates all visited nodes.
        ///     Reset the 'VisitCount' counter of each node before calling!
        /// </summary>
        /// <param name="node">The node to update and its ouput path nodes</param>
        private void UpdateOutputPath(Node node)
        {
            if (node.VisitCount > 0) return; // already updated

            node.VisitCount++;

            foreach (AbstractSocket socket in node.Sockets) // follow the OutputSockets of the node
            {
                if (socket.IsOutput() && socket.IsConnected())
                {
                    OutputSocket outputSocket = (OutputSocket) socket;
                    foreach (Connection edge in outputSocket.Connections)
                    {
                        this.UpdateOutputPath(edge.Input.Parent);
                    }
                }
            }
        }

        /// <summary> Returns true if the sockets can be linked.</summary>
        /// <param name="inSocket"> The input socket</param>
        /// <param name="outSocket"> The output socket</param>
        /// <returns>True if the sockets can be linked.</returns>
        public bool CanBeLinked(InputSocket inSocket, OutputSocket outSocket)
        {
            return (inSocket != null) && (outSocket != null) && (outSocket.Type == inSocket.Type);
        }

        public void LogCircleError()
        {
            Debug.LogError("The graph contains ciclyes.");
        }

        public static bool Save(string fileName, Graph graph)
        {
            var asset = ScriptableObject.CreateInstance<EncounterAsset>();

            CustomEditorUtils.SaveAsset(asset, "Assets/Resources/Encounters/", graph.GetEncounterTag());

            for (int i = 0; i < graph.GetNodeCount(); i++)
            {
                var node = graph.GetNodeAt(i) as EncounterNodeBase;
                asset.AddNode(node.Encounter);
                CustomEditorUtils.AddAssetToAsset(node.Encounter, asset);

                foreach (var allower in node.Encounter.Allowers)
                {
                    CustomEditorUtils.AddAssetToAsset(allower, asset);
                }

                foreach (var @event in node.Encounter.Events)
                {
                    CustomEditorUtils.AddAssetToAsset(@event, asset);
                }
            }

            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog("Saved", "Encounter Saved!", "OK");

            return true;
        }

        public static Graph Load(string fileName)
        {
            return null;
        }
    }
}