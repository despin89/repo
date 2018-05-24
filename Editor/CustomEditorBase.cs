namespace GD.EditorExtentions
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEditor;
    using UnityEditor.AnimatedValues;
    using UnityEditorInternal;
    using UnityEngine;
    using Object = UnityEngine.Object;

    [CustomEditor(typeof(Object), true, isFallback = true)]
    [CanEditMultipleObjects]
    public class CustomEditorBase : Editor
    {
        #region Fields

        private Dictionary<string, ReorderableListProperty> _reorderableLists;

        #endregion

        protected virtual void OnEnable()
        {
            this._reorderableLists = new Dictionary<string, ReorderableListProperty>(10);
        }

        ~CustomEditorBase()
        {
            this._reorderableLists.Clear();
            this._reorderableLists = null;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Custom Editor", EditorStyles.centeredGreyMiniLabel);
            Color cachedGuiColor = GUI.color;
            this.serializedObject.Update();
            SerializedProperty property = this.serializedObject.GetIterator();
            bool next = property.NextVisible(true);
            if (next)
            {
                do
                {
                    GUI.color = cachedGuiColor;
                    this.HandleProperty(property);
                } while (property.NextVisible(false));
            }

            this.serializedObject.ApplyModifiedProperties();
        }

        protected void HandleProperty(SerializedProperty property)
        {
            //Debug.LogFormat("name: {0}, displayName: {1}, type: {2}, propertyType: {3}, path: {4}", property.name, property.displayName, property.type, property.propertyType, property.propertyPath);
            bool isdefaultScriptProperty = property.name.Equals("m_Script") && property.type.Equals("PPtr<MonoScript>") &&
                                           (property.propertyType == SerializedPropertyType.ObjectReference) &&
                                           property.propertyPath.Equals("m_Script");
            bool cachedGUIEnabled = GUI.enabled;
            if (isdefaultScriptProperty)
            {
                GUI.enabled = false;
            }
            //var attr = this.GetPropertyAttributes(property);
            if (property.isArray && (property.propertyType != SerializedPropertyType.String))
            {
                this.HandleArray(property);
            }
            else
            {
                EditorGUILayout.PropertyField(property, property.isExpanded);
            }
            if (isdefaultScriptProperty)
            {
                GUI.enabled = cachedGUIEnabled;
            }
        }

        protected void HandleArray(SerializedProperty property)
        {
            ReorderableListProperty listData = this.GetReorderableList(property);
            listData.IsExpanded.target = property.isExpanded;
            if ((!listData.IsExpanded.value && !listData.IsExpanded.isAnimating) ||
                (!listData.IsExpanded.value && listData.IsExpanded.isAnimating))
            {
                EditorGUILayout.BeginHorizontal();
                property.isExpanded = EditorGUILayout.ToggleLeft(string.Format("{0}[]", property.displayName),
                                                                 property.isExpanded, EditorStyles.boldLabel);
                EditorGUILayout.LabelField(string.Format("size: {0}", property.arraySize));
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                if (EditorGUILayout.BeginFadeGroup(listData.IsExpanded.faded))
                {
                    listData.List.DoLayoutList();
                }
                EditorGUILayout.EndFadeGroup();
            }
        }

        protected object[] GetPropertyAttributes(SerializedProperty property)
        {
            return this.GetPropertyAttributes<PropertyAttribute>(property);
        }

        protected object[] GetPropertyAttributes<T>(SerializedProperty property) where T : Attribute
        {
            BindingFlags bindingFlags = BindingFlags.GetField
                                        | BindingFlags.GetProperty
                                        | BindingFlags.IgnoreCase
                                        | BindingFlags.Instance
                                        | BindingFlags.NonPublic
                                        | BindingFlags.Public;
            if (property.serializedObject.targetObject == null)
            {
                return null;
            }

            Type targetType = property.serializedObject.targetObject.GetType();
            FieldInfo field = targetType.GetField(property.name, bindingFlags);
            if (field != null)
            {
                return field.GetCustomAttributes(typeof(T), true);
            }

            return null;
        }

        private ReorderableListProperty GetReorderableList(SerializedProperty property)
        {
            ReorderableListProperty ret = null;
            if (this._reorderableLists.TryGetValue(property.name, out ret))
            {
                ret.Property = property;
                return ret;
            }

            ret = new ReorderableListProperty(property);
            this._reorderableLists.Add(property.name, ret);
            return ret;
        }

        #region ReorderableListProperty

        private class ReorderableListProperty
        {
            #region Fields

            private SerializedProperty _property;

            #endregion

            #region Properties

            public AnimBool IsExpanded { get; private set; }

            public ReorderableList List { get; private set; }

            public SerializedProperty Property
            {
                get { return this._property; }
                set
                {
                    this._property = value;
                    this.List.serializedProperty = this._property;
                }
            }

            #endregion

            #region Constructors

            public ReorderableListProperty(SerializedProperty property)
            {
                this.IsExpanded = new AnimBool(property.isExpanded);
                this.IsExpanded.speed = 1f;
                this._property = property;
                this.CreateList();
            }

            #endregion

            ~ReorderableListProperty()
            {
                this._property = null;
                this.List = null;
            }

            private void CreateList()
            {
                bool dragable = true, header = true, add = true, remove = true;
                this.List = new ReorderableList(this.Property.serializedObject, this.Property, dragable, header, add,
                                                remove);
                this.List.drawHeaderCallback +=
                    rect => this._property.isExpanded =
                        EditorGUI.ToggleLeft(rect, this._property.displayName, this._property.isExpanded,
                                             EditorStyles.boldLabel);
                this.List.onCanRemoveCallback += list => { return this.List.count > 0; };
                this.List.drawElementCallback += this.drawElement;
                this.List.elementHeightCallback +=
                    idx =>
                    {
                        return
                            Mathf.Max(EditorGUIUtility.singleLineHeight,
                                      EditorGUI.GetPropertyHeight(this._property.GetArrayElementAtIndex(idx),
                                                                  GUIContent.none, true)) +
                            4.0f;
                    };
            }

            private void drawElement(Rect rect, int index, bool active, bool focused)
            {
                if (this._property.GetArrayElementAtIndex(index).propertyType == SerializedPropertyType.Generic)
                {
                    EditorGUI.LabelField(rect, this._property.GetArrayElementAtIndex(index).displayName);
                }
                //rect.height = 16;
                rect.height = EditorGUI.GetPropertyHeight(this._property.GetArrayElementAtIndex(index), GUIContent.none,
                                                          true);
                rect.y += 1;
                EditorGUI.PropertyField(rect, this._property.GetArrayElementAtIndex(index), GUIContent.none, true);
                this.List.elementHeight = rect.height + 4.0f;
            }
        }

        #endregion
    }
}