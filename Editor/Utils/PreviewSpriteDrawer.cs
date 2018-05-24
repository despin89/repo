namespace GD.EditorExtentions
{
    using System;
    using Attributes;
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(PreviewSpriteAttribute))]
    public class PreviewSpriteDrawer : PropertyDrawer
    {
        #region Constants

        private const float imageHeight = 100;

        #endregion

        public override float GetPropertyHeight(SerializedProperty property,
            GUIContent label)
        {
            if ((property.propertyType == SerializedPropertyType.ObjectReference) &&
                property.objectReferenceValue is Sprite)
            {
                return EditorGUI.GetPropertyHeight(property, label, true) + imageHeight + 10;
            }

            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        private static string GetPath(SerializedProperty property)
        {
            string path = property.propertyPath;
            int index = path.LastIndexOf(".", StringComparison.Ordinal);
            return path.Substring(0, index + 1);
        }

        public override void OnGUI(Rect position,
            SerializedProperty property,
            GUIContent label)
        {
            //Draw the normal property field
            EditorGUI.PropertyField(position, property, label, true);

            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                Sprite sprite = property.objectReferenceValue as Sprite;
                if (sprite != null)
                {
                    position.y += EditorGUI.GetPropertyHeight(property, label, true) + 5;
                    //position.height = imageHeight;
                    if (sprite.textureRect.height < 50)
                    {
                        position.height = sprite.textureRect.height * 2;
                        position.width = sprite.textureRect.width * 2;
                    }
                    else
                    {
                        position.height = sprite.textureRect.height;
                        position.width = sprite.textureRect.width;
                    }

                    Rect texRect = new Rect(0, 0, sprite.texture.width, sprite.texture.height);

                    Vector2 min = Rect.PointToNormalized(texRect, sprite.textureRect.min);

                    float height = sprite.textureRect.height / sprite.texture.height;
                    float width = sprite.textureRect.width / sprite.texture.width;

                    GUI.DrawTextureWithTexCoords(position, sprite.texture, new Rect(min.x, min.y, width, height));
                    //EditorGUI.DrawPreviewTexture(position, sprite.texture, null, ScaleMode.ScaleToFit, 0);
                    //GUI.DrawTexture(position, sprite.texture, ScaleMode.ScaleToFit);
                }
            }
        }
    }
}