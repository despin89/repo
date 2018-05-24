//namespace GD.EditorExtentions
//{
//    using Game.AnimationSystem;
//    using UnityEditor;
//    using UnityEngine;

//    [CustomEditor(typeof(AnimEventsSequence))]
//    public class AnimSequenceEditor : Editor
//    {
//        public override void OnInspectorGUI()
//        {
//            if (GUILayout.Button("Open sequence editor"))
//            {
//                AnimSequenceEditorWindow.ShowWindow().SetSequence(this.target as AnimEventsSequence);
//            }
//        }
//    }
//}