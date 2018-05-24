namespace GD.EditorExtentions
{
    using System;
    using System.Collections.Generic;
    using Game.EncountersSystem;
    using UnityEngine;

    public class OutputSocket : AbstractSocket
    {
        #region Fields

        public List<Connection> Connections;

        #endregion

        #region Constructors

        public OutputSocket(Node parent, string type) : base(parent, type)
        {
            this.Connections = new List<Connection>();
        }

        #endregion

        public override bool IsConnected()
        {
            return this.Connections.Count > 0;
        }

        public override bool Intersects(Vector2 nodePosition)
        {
            if (this.Parent.Collapsed) return false;

            return this.BoxRect.Contains(nodePosition);
        }

        protected override void OnDraw()
        {
            GUI.Box(this.BoxRect, ">");
        }
    }
}