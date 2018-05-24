namespace GD.EditorExtentions
{
    using System;
    using UnityEditor;
    using UnityEngine;

    public class EditorWindowBase : EditorWindow
    {
        public event Action OnClose;

        protected virtual void OnDestroy()
        {
            if (this.OnClose != null) this.OnClose();
        }

        public static bool GetButton(string title, float width, Color color)
        {
            EditorGUILayout.BeginHorizontal();
            Color prevAddComponenButtonColor = GUI.color;
            GUI.color = color;
            bool result = GUILayout.Button(title, GUILayout.Width(width));
            GUI.color = prevAddComponenButtonColor;
            EditorGUILayout.EndHorizontal();

            return result;
        }
    }
}