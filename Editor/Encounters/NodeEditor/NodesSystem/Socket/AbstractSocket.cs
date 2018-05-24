namespace GD.EditorExtentions
{
    using System;
    using Game.EncountersSystem;
    using UnityEngine;

    public abstract class AbstractSocket
    {
        #region Fields

        public Node Parent;

        protected Rect BoxRect;
        protected RectOffset Padding;
        public string Type;

        #endregion

        #region Properties

        /// The x position of the node
        public float X
        {
            get { return this.BoxRect.x; }
            set { this.BoxRect.x = value; }
        }

        /// The y position of the node
        public float Y
        {
            get { return this.BoxRect.y; }
            set { this.BoxRect.y = value; }
        }

        #endregion

        #region Constructors

        protected AbstractSocket(Node parent, string type)
        {
            this.Type = type;
            this.Padding = new RectOffset(0, 0, -2, 0);
            this.Parent = parent;
            this.BoxRect.width = NodeEditorConfig.SocketSize;
            this.BoxRect.height = NodeEditorConfig.SocketSize;
        }

        #endregion

        public abstract bool IsConnected();
        protected abstract void OnDraw();
        public abstract bool Intersects(Vector2 nodePosition);


        public void Draw()
        {
            GUI.skin.box.normal.textColor = Node.GetEdgeColor(this.Type);
            GUI.skin.box.padding = this.Padding;
            GUI.skin.box.fontSize = 14;
            GUI.skin.box.fontStyle = FontStyle.Bold;
            this.OnDraw();
        }

        public bool IsInput()
        {
            return this.GetType() == typeof(InputSocket);
        }

        public bool IsOutput()
        {
            return this.GetType() == typeof(OutputSocket);
        }
    }
}