namespace GD.EditorExtentions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Game.EncountersSystem;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    ///     This class contains the logic of the editor window. It contains canvases that
    ///     are containing graphs. It uses the Launcher to load, save and close Graphs.
    /// </summary>
    public class NodeEditorWindow : EditorWindow
    {
        #region Constants

        private const string Name = "Encounter Editor";
        public const int TopOffset = 32;
        public const int BottomOffset = 20;
        public const int TopMenuHeight = 20;


        private const int TabButtonWidth = 200;
        private const int TabButtonMargin = 4;
        private const int TabCloseButtonSize = TopMenuHeight;

        private const int WindowTitleHeight = 21;
        private const float CanvasZoomMin = 0.1f;
        private const float CanvasZoomMax = 1.0f;

        #endregion

        #region Fields

        private AbstractSocket _dragSourceSocket;
        private NodesCanvas _currentCanvas;

        private Color _tabColorUnselected = new Color(0.8f, 0.8f, 0.8f, 0.5f);
        private Color _tabColorSelected = Color.white;
        private Dictionary<string, Type> _menuEntryToNodeType;

        private GenericMenu _menu;


        private List<NodesCanvas> _canvasList = new List<NodesCanvas>();

        private Rect _openButtonRect = new Rect(0, 0, 80, TopMenuHeight);
        private Rect _saveButtonRect = new Rect(80, 0, 80, TopMenuHeight);
        private Rect _newButtonRect = new Rect(160, 0, 80, TopMenuHeight);
        private Rect _canvasRegion = new Rect();

        private Rect _tmpRect = new Rect();

        private Vector2 _nextTranlationPosition;
        private Vector2 _lastMousePosition;

        #endregion

        [MenuItem("Window/" + Name)]
        private static void OnCreateWindow()
        {
            NodeEditorWindow window = GetWindow<NodeEditorWindow>();
            // NodeEditorWindow window = CreateInstance<NodeEditorWindow>(); // to create a new window
            window.Show();
        }

        public void OnEnable()
        {
            this.Init();
        }

        public void Init()
        {
            EditorApplication.playmodeStateChanged = this.OnPlaymodeStateChanged;
            // create GameObject and the Component if it is not added to the scene

            this.titleContent = new GUIContent(Name);
            this.wantsMouseMove = true;
            EventManager.TriggerOnWindowOpen();
            this._menuEntryToNodeType = this.CreateMenuEntries();
            this._menu = this.CreateGenericMenu();

            this._canvasList.Clear();
            this._currentCanvas = null;

            if (Launcher.Graphs.Count > 0)
            {
                this.LoadCanvas(Launcher.Graphs);
            }
            else
            {
                this.LoadCanvas(Launcher.LoadGraph(NodeEditorConfig.DefaultGraphName));
            }
            this.Repaint();
        }

        private void OnPlaymodeStateChanged()
        {
            this.Repaint();
        }

        private void LoadCanvas(List<Graph> graphs)
        {
            foreach (Graph graph in graphs) this.LoadCanvas(graph);
        }

        private void LoadCanvas(Graph graph)
        {
            this._currentCanvas = new NodesCanvas(graph);
            this._canvasList.Add(this._currentCanvas);
        }

        /// <summary>
        ///     Creates a dictonary that maps a menu entry string to a node type using reflection.
        /// </summary>
        /// <returns>
        ///     Dictonary that maps a menu entry string to a node type
        /// </returns>
        public Dictionary<string, Type> CreateMenuEntries()
        {
            Dictionary<string, Type> menuEntries = new Dictionary<string, Type>();

            IEnumerable<Type> classesExtendingNode = Assembly.GetAssembly(typeof(Node)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Node)));

            foreach (Type type in classesExtendingNode) menuEntries.Add(this.GetItemMenuName(type), type);

            menuEntries.OrderBy(x => x.Key);
            return menuEntries;
        }

        private string GetItemMenuName(Type type)
        {
            string path = Node.GetNodePath(type);
            if (path != null) return path + "/" + Node.GetNodeName(type);

            return Node.GetNodeName(type);
        }

        /// <summary>Draws the UI</summary>
        private void OnGUI()
        {
            this.HandleCanvasTranslation();
            this.HandleDragAndDrop();

            if (Event.current.type == EventType.ContextClick)
            {
                this._menu.ShowAsContext();
                Event.current.Use();
            }
            this.HandleMenuButtons();

            this.HandleTabButtons();

            if (this._currentCanvas != null)
            {
                float infoPanelY = Screen.height - TopOffset - 6;
                this._tmpRect.Set(5, infoPanelY, 70, 20);
                GUI.Label(this._tmpRect, "zoom: " + Math.Round(this._currentCanvas.Zoom, 1));
                this._tmpRect.Set(60, infoPanelY, 70, 20);
                GUI.Label(this._tmpRect, "x: " + Math.Round(this._currentCanvas.Position.x));
                this._tmpRect.Set(130, infoPanelY, 70, 20);
                GUI.Label(this._tmpRect, "y: " + Math.Round(this._currentCanvas.Position.y));
                this._tmpRect.Set(200, infoPanelY, 70, 20);
                GUI.Label(this._tmpRect, "nodes: " + this._currentCanvas.Graph.GetNodeCount());
            }

            if (this._currentCanvas != null)
            {
                this._canvasRegion.Set(0, TopOffset, Screen.width, Screen.height - 2 * TopOffset - BottomOffset);
                this._currentCanvas.Draw(this, this._canvasRegion, this._dragSourceSocket);
            }
            this._lastMousePosition = Event.current.mousePosition;

            this.Repaint();
        }

        private void HandleTabButtons()
        {
            Color standardBackgroundColor = GUI.backgroundColor;
            int tabIndex = 0;
            NodesCanvas canvasToClose = null;
            foreach (NodesCanvas tmpCanvas in this._canvasList)
            {
                int width = TabButtonWidth + TabButtonMargin + TabCloseButtonSize;
                int xOffset = width * tabIndex;

                tmpCanvas.TabButton.Set(xOffset, TopMenuHeight + TabButtonMargin, TabButtonWidth, TopMenuHeight);
                tmpCanvas.CloseTabButton.Set(xOffset + width - TabCloseButtonSize - TabButtonMargin - 4,
                                             TopMenuHeight + TabButtonMargin, TabCloseButtonSize, TabCloseButtonSize);

                bool isSelected = this._currentCanvas == tmpCanvas;
                string tabName = tmpCanvas.Graph.Name;
                //tabName = Path.GetFileName(tmpCanvas.FilePath);

                if (isSelected)
                {
                    GUI.backgroundColor = this._tabColorSelected;
                }
                else
                {
                    GUI.backgroundColor = this._tabColorUnselected;
                }

                if (GUI.Button(tmpCanvas.TabButton, tabName))
                {
                    this.SetCurrentCanvas(tmpCanvas);
                }
                if (isSelected)
                {
                    if (GUI.Button(tmpCanvas.CloseTabButton, "X"))
                    {
                        canvasToClose = tmpCanvas;
                    }
                }
                tabIndex++;
            }

            GUI.backgroundColor = standardBackgroundColor;
            if (canvasToClose != null) this.CloseCanvas(canvasToClose);
        }

        private void SetCurrentCanvas(NodesCanvas canvas)
        {
            this.Repaint();
            if (canvas != null) EventManager.TriggerOnFocusGraph(canvas.Graph);
            this._currentCanvas = canvas;
        }

        private void CloseCanvas(NodesCanvas canvas)
        {
            bool doSave = EditorUtility.DisplayDialog("Do you want to save.",
                                                      "Do you want to save the graph " + canvas.FilePath + " ?",
                                                      "Yes", "No");
            if (doSave)
            {
                Launcher.SaveGraph(canvas.Graph, canvas.FilePath);
            }
            EventManager.TriggerOnCloseGraph(canvas.Graph);
            Launcher.RemoveGraph(canvas.Graph);
            this._canvasList.Remove(canvas);
            if (this._canvasList.Count > 0)
            {
                this.SetCurrentCanvas(this._canvasList[0]);
            }
            else
            {
                this.SetCurrentCanvas(null);
            }
        }

        private GenericMenu CreateGenericMenu()
        {
            GenericMenu m = new GenericMenu();
            foreach (KeyValuePair<string, Type> entry in this._menuEntryToNodeType)
            {
                m.AddItem(new GUIContent(entry.Key), false, this.OnGenericMenuClick, entry.Value);
            }

            return m;
        }

        private void OnGenericMenuClick(object item)
        {
            if (this._currentCanvas != null)
            {
                this._currentCanvas.CreateNode((Type) item, this._lastMousePosition);
            }
        }

        private void CreateCanvas(string path)
        {
            NodesCanvas canvas;
            if (path != null)
            {
                canvas = new NodesCanvas(Launcher.LoadGraph(path));
            }
            else
            {
                canvas = new NodesCanvas(Launcher.LoadGraph(NodeEditorConfig.DefaultGraphName));
            }
            canvas.FilePath = path;
            this._canvasList.Add(canvas);
            this.SetCurrentCanvas(canvas);
        }

        private void HandleMenuButtons()
        {
            if (GUI.Button(this._openButtonRect, "Open"))
            {
                string path = EditorUtility.OpenFilePanel("load graph data", "", "json");
                if (!path.Equals("")) this.CreateCanvas(path);
            }

            // Save Button
            if (GUI.Button(this._saveButtonRect, "Save")) Launcher.SaveGraph(this._currentCanvas.Graph, this._currentCanvas.FilePath);
            // New Button
            if (GUI.Button(this._newButtonRect, "New")) this.CreateCanvas(null);
        }


        private void HandleCanvasTranslation()
        {
            if (this._currentCanvas == null) return;

            // Zoom
            if (Event.current.type == EventType.ScrollWheel)
            {
                Vector2 zoomCoordsMousePos = this.ConvertScreenCoordsToZoomCoords(Event.current.mousePosition);
                float zoomDelta = -Event.current.delta.y / 150.0f;
                float oldZoom = this._currentCanvas.Zoom;
                this._currentCanvas.Zoom = Mathf.Clamp(this._currentCanvas.Zoom + zoomDelta, CanvasZoomMin,
                                                       CanvasZoomMax);

                this._nextTranlationPosition = this._currentCanvas.Position +
                                               (zoomCoordsMousePos - this._currentCanvas.Position) -
                                               oldZoom / this._currentCanvas.Zoom *
                                               (zoomCoordsMousePos - this._currentCanvas.Position);

                if (this._nextTranlationPosition.x >= 0) this._nextTranlationPosition.x = 0;
                if (this._nextTranlationPosition.y >= 0) this._nextTranlationPosition.y = 0;
                this._currentCanvas.Position = this._nextTranlationPosition;
                Event.current.Use();
                return;
            }

            // Translate
            if (((Event.current.type == EventType.MouseDrag) && (Event.current.button == 0) &&
                 (Event.current.modifiers == EventModifiers.Alt)) ||
                (Event.current.button == 2))
            {
                Vector2 delta = Event.current.delta;
                delta /= this._currentCanvas.Zoom;

                this._nextTranlationPosition = this._currentCanvas.Position + delta;
                if (this._nextTranlationPosition.x >= 0) this._nextTranlationPosition.x = 0;
                if (this._nextTranlationPosition.y >= 0) this._nextTranlationPosition.y = 0;

                this._currentCanvas.Position = this._nextTranlationPosition;
                Event.current.Use();
            }
        }

        private void HandleSocketDrag(AbstractSocket dragSource)
        {
            if (dragSource != null)
            {
                if (dragSource.IsInput() && dragSource.IsConnected())
                {
                    this._dragSourceSocket = ((InputSocket) dragSource).Connection.GetOtherSocket(dragSource);
                    this._currentCanvas.Graph.UnLink((InputSocket) dragSource, (OutputSocket) this._dragSourceSocket);
                }
                if (dragSource.IsOutput()) this._dragSourceSocket = dragSource;
                Event.current.Use();
            }
            this.Repaint();
        }

        private void HandleSocketDrop(AbstractSocket dropTarget)
        {
            if ((dropTarget != null) && (dropTarget.GetType() != this._dragSourceSocket.GetType()))
            {
                if (dropTarget.IsInput())
                {
                    this._currentCanvas.Graph.Link((InputSocket) dropTarget, (OutputSocket) this._dragSourceSocket);
                }
                Event.current.Use();
            }
            this._dragSourceSocket = null;
            this.Repaint();
        }

        private void HandleDragAndDrop()
        {
            if (this._currentCanvas == null) return;

            if (Event.current.type == EventType.MouseDown)
            {
                this.HandleSocketDrag(this._currentCanvas.GetSocketAt(Event.current.mousePosition));
            }

            if ((Event.current.type == EventType.MouseUp) && (this._dragSourceSocket != null))
            {
                this.HandleSocketDrop(this._currentCanvas.GetSocketAt(Event.current.mousePosition));
            }

            if (Event.current.type == EventType.MouseDrag)
            {
                if (this._dragSourceSocket != null) Event.current.Use();
            }
        }

        private Vector2 ConvertScreenCoordsToZoomCoords(Vector2 screenCoords)
        {
            return (screenCoords - this._canvasRegion.TopLeft()) / this._currentCanvas.Zoom +
                   this._currentCanvas.Position;
        }
    }
}