namespace GD.EditorExtentions
{
    using Game.MapSystem;
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(EncounterModel))]
    public class EncounterModelDrawer : PropertyDrawer
    {
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            // Draw fields - passs GUIContent.none to each so they are drawn without labels

            GUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.PropertyField(property.FindPropertyRelative("Encounter"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("MinCount"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("MaxCount"));
            GUILayout.EndVertical();

            GUILayout.Space(5);

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}