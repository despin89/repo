
//#if UNITY_EDITOR

//namespace GD.EditorExtentions
//{
//    using Game.AnimationSystem;
//    using UnityEditor;
//    using UnityEngine;

//    public class AnimEventEditorWindow : EditorWindow
//    {
//        #region Fields

//        private AnimEventBase _currentEvent;

//        #endregion

//        public static void ShowWindow()
//        {
//            GetWindow(typeof(AnimEventEditorWindow));
//        }

//        public void SetCurrentEvent(AnimEventBase animEvent)
//        {
//            this._currentEvent = animEvent;
//        }

//        private void OnGUI()
//        {
//            if (this._currentEvent != null)
//            {
//                if ((Event.current.type == EventType.keyDown) && (Event.current.keyCode == KeyCode.Return))
//                {
//                    this.Close();
//                }

//                Editor.CreateEditor(this._currentEvent).OnInspectorGUI();

//                if (GUILayout.Button("Save"))
//                {
//                    EditorUtility.SetDirty(this._currentEvent);
//                }
//            }
//            else
//            {
//                GUILayout.Label("Select event");
//            }
//        }
//    }
//}

//#endif