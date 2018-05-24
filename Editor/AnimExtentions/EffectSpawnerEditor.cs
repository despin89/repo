//namespace GD.EditorExtentions
//{
//    using Extentions;
//    using Game.AnimationSystem;
//    using Game.Combat;
//    using UnityEditor;
//    using UnityEngine;
//    using Animator = UnityEngine.Animator;

//    [CustomEditor(typeof(EffectSpawner))]
//    public class EffectSpawnerEditor : CustomEditorBase
//    {
//        public override void OnInspectorGUI()
//        {
//            base.OnInspectorGUI();

//            EffectSpawner effectSpawner = (EffectSpawner) this.target;

//            if (effectSpawner.Clip != null)
//            {
//                EditorGUILayout.LabelField(
//                                           "Current clip lenght: " +
//                                           effectSpawner.Clip.length * effectSpawner.AnimLenght,
//                                           EditorStyles.boldLabel);
//            }
//            else
//            {
//                EditorGUILayout.LabelField("Select anim clip!", EditorStyles.boldLabel);
//            }

//            if (GUILayout.Button("Set spawner"))
//            {
//                effectSpawner.Animator = effectSpawner.gameObject.GetComponent<Animator>();
//                foreach (SpriteRenderer sr in effectSpawner.gameObject.GetComponentsInChildren<SpriteRenderer>())
//                {
//                    sr.sortingLayerName = "CombatLayer";
//                    sr.sortingOrder = 20;
//                }

//                EditorUtility.SetDirty(effectSpawner);
//            }

//            if (GUILayout.Button("Play"))
//            {
//                effectSpawner.PlayEditor();
//            }
//        }
//    }
//}