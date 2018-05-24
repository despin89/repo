namespace GD.EditorExtentions
{
    using Game.EncountersSystem;
    using UnityEngine;

    public class InputSocket : AbstractSocket
    {
        #region Fields

        public Connection Connection;
        private float _directInputNumber = float.NaN;
        private string _directInputString = "0";

        #endregion

        #region Constructors

        public InputSocket(Node parent, string type) : base(parent, type)
        {
        }

        #endregion

        public float GetDirectInputNumber()
        {
            if (float.IsNaN(this._directInputNumber)) this._directInputNumber = float.Parse(this._directInputString);
            return this._directInputNumber;
        }

        public void SetDirectInputNumber(float number, bool triggerChangeEvent)
        {
            if (!float.IsNaN(number))
            {
                this._directInputNumber = number;
                this._directInputString = number + "";
                if (triggerChangeEvent) this.Parent.TriggerChangeEvent();
            }
        }

        public override bool IsConnected()
        {
            return this.Connection != null;
        }

        public override bool Intersects(Vector2 nodePosition)
        {
            return !this.Parent.Collapsed && this.BoxRect.Contains(nodePosition);
        }

        protected override void OnDraw()
        {
            GUI.Box(this.BoxRect, ">");
        }

        public OutputSocket GetConnectedSocket()
        {
            return this.Connection.Output;
        }
    }
}