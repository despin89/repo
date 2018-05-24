namespace GD.EditorExtentions
{
    using System;
    using Game.EncountersSystem;
    using UnityEditor;
    using UnityEngine;

    [Serializable]
    public class NodesCanvas
    {
        #region Constants

        public const float CanvasSize = 100000;

        #endregion

        #region Fields

        private Color _backgroundColor = new Color(0.18f, 0.18f, 0.18f, 1f);
        private Color _backgroundLineColor01 = new Color(0.14f, 0.14f, 0.14f, 1f);
        private Color _backgroundLineColor02 = new Color(0.10f, 0.10f, 0.10f, 1f);

        [SerializeField]
        public float Zoom = 1;

        public Graph Graph;
        public GUIStyle Style = new GUIStyle();

        private GUIStyle centeredLabelStyle;

        public Rect DrawArea = new Rect();

        public Rect TabButton = new Rect();
        public Rect CloseTabButton = new Rect();

        public string FilePath;

        [SerializeField]
        public Vector2 Position = new Vector2();

        private Vector2 _tmpVector01;
        private Vector2 _tmpVector02;

        #endregion

        #region Constructors

        public NodesCanvas(Graph graph)
        {
            this.Graph = graph;
            this.Style.normal.background = this.CreateBackgroundTexture();
            this.Style.normal.background.wrapMode = TextureWrapMode.Repeat;
            this.Style.fixedHeight = CanvasSize;
            this.Style.fixedWidth = CanvasSize;
        }

        #endregion

        private Texture2D CreateBackgroundTexture()
        {
            Texture2D texture = new Texture2D(100, 100, TextureFormat.ARGB32, false);
            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.width; y++)
                {
                    bool isVerticalLine = x % 11 == 0;
                    bool isHorizontalLine = y % 11 == 0;
                    if ((x == 0) || (y == 0))
                    {
                        texture.SetPixel(x, y, this._backgroundLineColor02);
                    }
                    else if (isVerticalLine || isHorizontalLine)
                    {
                        texture.SetPixel(x, y, this._backgroundLineColor01);
                    }
                    else
                    {
                        texture.SetPixel(x, y, this._backgroundColor);
                    }
                }
            }

            texture.filterMode = FilterMode.Trilinear;
            texture.wrapMode = TextureWrapMode.Repeat;
            texture.Apply();
            return texture;
        }

        public void Draw(EditorWindow window, Rect region, AbstractSocket currentDragingSocket)
        {
            if (this.centeredLabelStyle == null) this.centeredLabelStyle = GUI.skin.GetStyle("Label");
            this.centeredLabelStyle.alignment = TextAnchor.MiddleCenter;

            EditorZoomArea.Begin(this.Zoom, region);

            if (this.Style.normal.background == null) this.Style.normal.background = this.CreateBackgroundTexture();
            GUI.DrawTextureWithTexCoords(this.DrawArea, this.Style.normal.background, new Rect(0, 0, 1000, 1000));
            this.DrawArea.Set(this.Position.x, this.Position.y, CanvasSize, CanvasSize);
            GUILayout.BeginArea(this.DrawArea);
            this.DrawEdges();
            window.BeginWindows();
            this.DrawNodes();
            window.EndWindows();
            this.DrawDragEdge(currentDragingSocket);

            for (int i = 0; i < this.Graph.GetNodeCount(); i++)
            {
                this.Graph.GetNodeAt(i).GUIDrawSockets();
            }

            GUILayout.EndArea();
            EditorZoomArea.End();
        }

        private void DrawDragEdge(AbstractSocket currentDragingSocket)
        {
            if (currentDragingSocket != null)
            {
                this._tmpVector01 = Connection.GetEdgePosition(currentDragingSocket, this._tmpVector01);
                this._tmpVector02 = Connection.GetTangentPosition(currentDragingSocket, this._tmpVector01);
                Connection.DrawEdge(this._tmpVector01, this._tmpVector02, Event.current.mousePosition,
                              Event.current.mousePosition,
                              currentDragingSocket.Type);
            }
        }

        public void DrawNodes()
        {
            for (int i = 0; i < this.Graph.GetNodeCount(); i++)
            {
                Node node = this.Graph.GetNodeAt(i);
                if (!node.Collapsed) node.WindowRect.height = node.Height;
                    node.WindowRect = GUI.Window(node.Id, node.WindowRect, this.GUIDrawNodeWindow, new GUIContent{ text = node.Name }, new GUIStyle
                    {
                        normal = {background = node.TitleColor},
                        fontStyle = FontStyle.Bold,
                        alignment = TextAnchor.UpperCenter,
                        fontSize = 16,
                        margin = new RectOffset(10, 10, 10, 10)
                    });
                if (node.Collapsed)
                {
                    // title bar text is not visible if collapsed
                    GUI.Label(node.WindowRect, node.Name + "", this.centeredLabelStyle);
                }

                node.GUIAlignSockets();
            }
        }

        private void GUIDrawNodeWindow(int nodeId)
        {
            Node node = this.Graph.GetNode(nodeId);
            node.ContentRect.Set(0, NodeEditorConfig.SocketOffsetTop,
                                 node.Width, node.Height - NodeEditorConfig.SocketOffsetTop);

            if ((Event.current.type == EventType.MouseDown) && (Event.current.button == 1))
            {
                GenericMenu m = new GenericMenu();
                m.AddDisabledItem(new GUIContent(node.Name + " (" + nodeId + ")"));
                m.AddSeparator("");
                m.AddItem(new GUIContent("Delete"), false, this.DeleteNode, nodeId);

                if (node.Collapsed)
                {
                    m.AddItem(new GUIContent("Expand"), false, this.ExpandNode, nodeId);
                }
                else
                {
                    m.AddItem(new GUIContent("Collapse"), false, this.CollapseNode, nodeId);
                }

                m.ShowAsContext();
                Event.current.Use();
            }

            if (!node.Collapsed)
            {
                GUILayout.BeginArea(node.ContentRect);
                GUI.color = Color.white;
                node.OnGUI();
                GUILayout.EndArea();
            }

            GUI.DragWindow();
            if (Event.current.GetTypeForControl(node.Id) == EventType.Used)
            {
                if (Node.LastFocusedNodeId != node.Id) node.OnFocus();
                Node.LastFocusedNodeId = node.Id;
            }
        }

        private void CollapseNode(object nodeId)
        {
            Node node = this.Graph.GetNode((int) nodeId);
            node.Collapse();
        }

        private void ExpandNode(object nodeId)
        {
            this.Graph.GetNode((int) nodeId).Expand();
        }


        private void DeleteNode(object nodeId)
        {
            this.Graph.RemoveNode((int) nodeId);
        }


        public void DrawEdges()
        {
            for (int i = 0; i < this.Graph.GetNodeCount(); i++)
            {
                this.Graph.GetNodeAt(i).GUIDrawEdges();
            }
        }

        public Node GetFocusedNode()
        {
            for (int i = 0; i < this.Graph.GetNodeCount(); i++)
            {
                Node node = this.Graph.GetNodeAt(i);
                if (node.HasFocus()) return node;
            }

            return null;
        }

        /// <summary> Returns the socket at the window position.</summary>
        /// <param name="windowPosition"> The position to get the Socket from in window coordinates</param>
        /// <returns>The socket at the posiiton or null or null.</returns>
        public AbstractSocket GetSocketAt(Vector2 windowPosition)
        {
            Vector2 projectedPosition = this.ProjectToCanvas(windowPosition);

            for (int i = 0; i < this.Graph.GetNodeCount(); i++)
            {
                Node node = this.Graph.GetNodeAt(i);
                AbstractSocket socket = node.SearchSocketAt(projectedPosition);
                if (socket != null)
                {
                    return socket;
                }
            }

            return null;
        }

        public Node CreateNode(Type nodeType, Vector2 windowPosition)
        {
            Node node = this.Graph.CreateNode(nodeType);
            Vector2 position = this.ProjectToCanvas(windowPosition);
            node.X = position.x;
            node.Y = position.y;
            this.Graph.AddNode(node);
            return node;
        }

        public void RemoveFocusedNode()
        {
            Node node = this.GetFocusedNode();
            if (node != null) this.Graph.RemoveNode(node);
        }

        public Vector2 ProjectToCanvas(Vector2 windowPosition)
        {
            windowPosition.y += 21 - NodeEditorWindow.TopOffset * 2;
            windowPosition = windowPosition / this.Zoom;
            windowPosition.x -= this.DrawArea.x;
            windowPosition.y -= this.DrawArea.y;
            return windowPosition;
        }
    }
}