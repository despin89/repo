using System.Collections.Generic;

namespace GD.EditorExtentions
{
    /// <summary>
    /// A class to controll the creation of Graphs. It contains loaded Grpahs.
    /// (A gameobject with this script is created by the editor if it is not in the scene)
    /// </summary>
    public static class Launcher
    {
        private static List<Graph> _graphs = new List<Graph>();

        private static StandardGraphController _controller;

        public static List<Graph> Graphs
        {
            get { return _graphs; }
        }

        /// <summary>
        /// Loads a graph by its path, adds it to the internal list
        /// and returns it.
        /// (Also used by the editor to open Graphs)
        /// </summary>
        public static Graph LoadGraph(string path)
        {
            Graph g;
            if (path.Equals(NodeEditorConfig.DefaultGraphName)) g = CreateDefaultGraph();
            else g = Graph.Load(path);
            g.Name = path;
            Graphs.Add(g);
            CreateGraphController(g);
            return g;
        }

        /// <summary>
        /// Saves a graph by its path.
        /// (Also used by the editor to save Graphs)
        /// </summary>
        public static void SaveGraph(Graph g, string path)
        {
            Graph.Save(path, g);
        }

        /// <summary>
        /// Removes a Graph from the internal list.
        /// (Also used by the editor to close Graphs)
        /// </summary>
        public static void RemoveGraph(Graph g)
        {
            Graphs.Remove(g);
        }

        /// <summary>
        /// Returns the graph at the index
        /// </summary>
        public static Graph GetGraph(int index)
        {
            return Graphs[index];
        }

        /// <summary>
        /// Create a controller for the assigned Graph.
        /// </summary>
        private static void CreateGraphController(Graph graph)
        {
            // in this case we create one controller for all graphs
            // you could also create different controllers for different graphs
            //if (_controller == null) _controller = new StandardGraphController();
        }

        static Launcher()
        {
            if (_controller == null) _controller = new StandardGraphController();
            _controller.Register();

            foreach (var graph in Graphs)
            {
                graph.ResetVisitCount();
            }
        }

        /// <summary>
        /// Creates a default Graph.
        /// (see: NodeEditorConfig.DefaultGraphName)
        /// </summary>
        public static Graph CreateDefaultGraph()
        {
            return new Graph();
        }

    }
}


