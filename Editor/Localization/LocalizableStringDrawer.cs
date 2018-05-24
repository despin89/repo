namespace GD.EditorExtentions
{
    using Core.Localization;
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(LocalizableString))]
    public class LocalizableStringDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 80f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            Rect textRect = new Rect(position.x, position.y, position.width, 80f);
            EditorGUI.PropertyField(textRect, property.FindPropertyRelative("_text"), GUIContent.none);

            EditorGUI.EndProperty();
        }
    }
}