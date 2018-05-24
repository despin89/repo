namespace GD.EditorExtentions
{
    using System;
    using System.Collections.Generic;
    using Game.EncountersSystem;
    using UnityEngine;

    public abstract class Node
    {
        #region Constants

        [NonSerialized]
        public static int LastFocusedNodeId;

        #endregion

        #region Fields

        [NonSerialized]
        public bool Resizable = true;

        [NonSerialized]
        public bool Collapsed;

        [NonSerialized]
        public float SocketTopOffsetInput;

        [NonSerialized]
        public float SocketTopOffsetOutput;

        [NonSerialized]
        public float Height;

        [NonSerialized]
        private Graph _parent;

        [NonSerialized]
        public int Id;

        [NonSerialized]
        public int VisitCount = 0;

        [NonSerialized]
        public List<AbstractSocket> Sockets = new List<AbstractSocket>();

        [NonSerialized]
        public Rect WindowRect;

        [NonSerialized]
        public Rect ContentRect;

        [NonSerialized]
        public Rect ResizeArea;

        [NonSerialized]
        public string Name;

        [NonSerialized]
        public Texture2D TitleColor;

        #endregion

        #region Properties

        /// The x position of the node
        public float X
        {
            get { return this.WindowRect.x; }
            set { this.WindowRect.x = value; }
        }

        /// The y position of the node
        public float Y
        {
            get { return this.WindowRect.y; }
            set { this.WindowRect.y = value; }
        }

        /// The width of the node
        public float Width
        {
            get { return this.WindowRect.width; }
            set { this.WindowRect.width = value; }
        }

        #endregion

        #region Constructors

        protected Node(int id, Graph parent)
        {
            this.ResizeArea = new Rect();
            this.Id = id;
            // default size
            this.Width = 100;
            this.Height = 100;
            this.Name = GetNodeName(this.GetType());
            this._parent = parent;
        }

        #endregion

        public int GetId()
        {
            return this.Id;
        }

        public abstract void OnGUI();

        public static Texture2D GetMonoTex(Color color)
        {
            var result = new Texture2D(2, 2);
            for (int i = 0; i < result.width; i++)
            {
                for (int j = 0; j < result.height; j++)
                {
                    result.SetPixel(i, j, color);
                }
            }
            result.Apply();

            return result;
        }

        /// <summary>Returns true if the node is focused.</summary>
        /// <returns>True if the node is focused.</returns>
        public bool HasFocus()
        {
            return LastFocusedNodeId == this.Id;
        }

        public virtual void OnFocus()
        {
            if (this._parent.TriggerEvents)
            {
                EventManager.TriggerOnFocusNode(this._parent, this);
            }
        }

        public void Collapse()
        {
            this.WindowRect.Set(this.WindowRect.x, this.WindowRect.y, this.WindowRect.width, 18);
            this.Collapsed = true;
        }

        public void Expand()
        {
            this.WindowRect.Set(this.WindowRect.x, this.WindowRect.y, this.WindowRect.width, this.Height);
            this.Collapsed = false;
        }

        /// <summary>Returns true if this assigned position intersects the node.</summary>
        /// <param name="canvasPosition">The position in canvas coordinates.</param>
        /// <returns>True if this assigned position intersects the node.</returns>
        public bool Intersects(Vector2 canvasPosition)
        {
            return this.WindowRect.Contains(canvasPosition);
        }

        /// <summary> Returns true if this node contains the assigned socket.</summary>
        /// <param name="socket"> The socket to use.</param>
        /// <returns>True if this node contains the assigned socket.</returns>
        public bool ContainsSocket(AbstractSocket socket)
        {
            return this.Sockets.Contains(socket);
        }

        public int GetInputSocketCount()
        {
            int count = 0;
            foreach (AbstractSocket socket in this.Sockets) if (socket.IsInput()) count++;

            return count;
        }

        public AbstractSocket GetSocket(string edgeType, string socketType, int index)
        {
            int searchIndex = -1;
            foreach (AbstractSocket socket in this.Sockets)
            {
                if ((socket.Type == edgeType) && (socket.Type == socketType))
                {
                    searchIndex++;
                    if (searchIndex == index) return socket;
                }
            }

            return null;
        }

        /// <summary>Projects the assigned position to a node relative position.</summary>
        /// <param name="canvasPosition">The position in canvas coordinates.</param>
        /// <returns>The position in node coordinates</returns>
        public Vector2 ProjectToNode(Vector2 canvasPosition)
        {
            canvasPosition.Set(canvasPosition.x - this.WindowRect.x, canvasPosition.y - this.WindowRect.y);
            return canvasPosition;
        }

        /// <summary> Searches for a socket that intesects the assigned position.</summary>
        /// <param name="canvasPosition">The position for intersection in canvas coordinates.</param>
        /// <returns>The at the position or null.</returns>
        public AbstractSocket SearchSocketAt(Vector2 canvasPosition)
        {
            //Vector2 nodePosition = ProjectToNode(canvasPosition);
            foreach (AbstractSocket socket in this.Sockets)
            {
                if (socket.Intersects(canvasPosition)) return socket;
            }

            return null;
        }

        /// <summary>
        ///     Triggers the OnChangedNode event from within a Node.
        ///     Call this method if your nodes content has changed.
        /// </summary>
        public void TriggerChangeEvent()
        {
            if (this._parent.TriggerEvents)
            {
                EventManager.TriggerOnChangedNode(this._parent, this);
            }
        }

        /// <summary> Returns true if all input Sockets are connected.</summary>
        /// <returns> True if all input Sockets are connected.</returns>
        public bool AllInputSocketsConnected()
        {
            foreach (AbstractSocket socket in this.Sockets)
            {
                if (!socket.IsConnected() && socket.IsInput()) return false;
            }

            return true;
        }

        /// <summary> Returns the total number of input edges connected into this node.</summary>
        public int GetConnectedInputCount()
        {
            int count = 0;
            foreach (AbstractSocket socket in this.Sockets)
            {
                if (socket.IsInput() && socket.IsConnected())
                {
                    count++;
                }
            }

            return count;
        }


        public void GUIDrawSockets()
        {
            if (!this.Collapsed) foreach (AbstractSocket socket in this.Sockets) socket.Draw();
        }

        public void GUIDrawEdges()
        {
            foreach (AbstractSocket socket in this.Sockets)
            {
                if (socket.IsInput()) // draw only input sockets to avoid double drawing of edges
                {
                    InputSocket inputSocket = (InputSocket) socket;
                    if (inputSocket.IsConnected()) inputSocket.Connection.Draw();
                }
            }
        }

        /// <summary> Aligns the position of the sockets of this node </summary>
        public void GUIAlignSockets()
        {
            int leftCount = 0;
            int rightCount = 0;
            foreach (AbstractSocket socket in this.Sockets)
            {
                if (socket.IsInput())
                {
                    socket.X = -NodeEditorConfig.SocketSize + this.WindowRect.x;
                    socket.Y = this.GUICalcSocketTopOffset(leftCount) + this.WindowRect.y + this.SocketTopOffsetInput;
                    leftCount++;
                }
                else
                {
                    socket.X = this.WindowRect.width + this.WindowRect.x;
                    socket.Y = this.GUICalcSocketTopOffset(rightCount) + this.WindowRect.y + this.SocketTopOffsetOutput;
                    rightCount++;
                }
            }
        }

        /// <summary> Calculates the offset of a socket from the top of this node </summary>
        private int GUICalcSocketTopOffset(int socketTopIndex)
        {
            return NodeEditorConfig.SocketOffsetTop + socketTopIndex * NodeEditorConfig.SocketSize
                   + socketTopIndex * NodeEditorConfig.SocketMargin;
        }

        /// <summary> Gets the menu entry name of this node </summary>
        public static string GetNodeName(Type nodeType)
        {
            object[] attrs = nodeType.GetCustomAttributes(typeof(GraphContextMenuItem), true);
            GraphContextMenuItem attr = (GraphContextMenuItem) attrs[0];
            return string.IsNullOrEmpty(attr.Name) ? nodeType.Name : attr.Name;
        }

        /// <summary> Gets the menu entry path of this node </summary>
        public static string GetNodePath(Type nodeType)
        {
            object[] attrs = nodeType.GetCustomAttributes(typeof(GraphContextMenuItem), true);
            GraphContextMenuItem attr = (GraphContextMenuItem) attrs[0];
            return string.IsNullOrEmpty(attr.Path) ? null : attr.Path;
        }

        public static Color GetEdgeColor(string nodeType)
        {
            switch (nodeType)
            {
                case "Choice":
                    return Color.red;
                case "Encounter":
                    return Color.green;
                default:
                    return Color.black;
            }
        }
    }

    /// <summary> Annotation to register menu entries of Nodes to the editor.</summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class GraphContextMenuItem : Attribute
    {
        #region Fields

        #endregion

        #region Properties

        public string Path { get; private set; }
        public string Name { get; private set; }

        #endregion

        #region Constructors

        public GraphContextMenuItem(string menuPath) : this(menuPath, null)
        {
        }

        public GraphContextMenuItem(string menuPath, string itemName)
        {
            this.Path = menuPath;
            this.Name = itemName;
        }

        #endregion
    }
}