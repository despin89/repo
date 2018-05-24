namespace GD.EditorExtentions
{
    using System;
    using Game.EncountersSystem;
    using UnityEditor;
    using UnityEngine;

    public class Connection
    {
        #region Fields

        public InputSocket Input;
        public OutputSocket Output;

        // cached vectors for drawing
        private Vector2 _tmpStartPos;
        private Vector2 _tmpEndPos;
        private Vector2 _tmpTangent01;
        private Vector2 _tmpTangent02;

        #endregion

        #region Constructors

        public Connection(OutputSocket outputSocket, InputSocket inputSocket)
        {
            this.Input = inputSocket;
            this.Output = outputSocket;
        }

        #endregion

        public AbstractSocket GetOtherSocket(AbstractSocket socket)
        {
            if (socket == this.Input) return this.Output;

            return this.Input;
        }

        public void Draw()
        {
            if ((this.Input != null) && (this.Output != null))
            {
                this._tmpStartPos = GetEdgePosition(this.Output, this._tmpStartPos);
                this._tmpEndPos = GetEdgePosition(this.Input, this._tmpEndPos);
                this._tmpTangent01 = GetTangentPosition(this.Output, this._tmpStartPos);
                this._tmpTangent02 = GetTangentPosition(this.Input, this._tmpEndPos);
                DrawEdge(this._tmpStartPos, this._tmpTangent01, this._tmpEndPos, this._tmpTangent02, this.Output.Type);

                Handles.color = Color.black;
                this._tmpStartPos.Set(this._tmpEndPos.x - 5, this._tmpEndPos.y - 5);
                Handles.DrawLine(this._tmpEndPos, this._tmpStartPos);
                this._tmpStartPos.Set(this._tmpEndPos.x - 5, this._tmpEndPos.y + 5);
                Handles.DrawLine(this._tmpEndPos, this._tmpStartPos);
            }
        }

        public static void DrawEdge(Vector2 position01, Vector2 tangent01, Vector2 position02, Vector2 tangent02,
            string type)
        {
            Handles.DrawBezier(
                               position01, position02,
                               tangent01, tangent02, Color.black, null, 6);

            Handles.DrawBezier(
                               position01, position02,
                               tangent01, tangent02, Node.GetEdgeColor(type), null, 3);
        }

        public static Vector2 GetEdgePosition(AbstractSocket socket, Vector2 position)
        {
            if (socket.Parent.Collapsed)
            {
                float width = NodeEditorConfig.SocketSize;
                if (socket.IsOutput()) width = 0;
                position.Set(socket.X + width, socket.Parent.WindowRect.y + 8);
            }
            else
            {
                float width = 0;
                if (socket.IsOutput()) width = NodeEditorConfig.SocketSize;
                position.Set(socket.X + width, socket.Y + NodeEditorConfig.SocketSize / 2f);
            }
            return position;
        }

        public static Vector2 GetTangentPosition(AbstractSocket socket, Vector2 position)
        {
            if (socket.IsInput()) return position + Vector2.left * NodeEditorConfig.EdgeTangent;

            return position + Vector2.right * NodeEditorConfig.EdgeTangent;
        }
    }
}