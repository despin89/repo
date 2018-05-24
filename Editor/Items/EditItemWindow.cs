namespace GD.EditorExtentions
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Extentions;
    using Game.ActionsSystem;
    using Game.ItemsSystem;
    using UnityEditor;
    using UnityEngine;

    public class EditItemWindow : EditorWindowBase
    {
        #region Fields

        private int _componentTypeIndex = -1;

        private ItemInfo _item;

        private string[] _componentTypeNames = new string[0];
        private Vector2 _scrollPos;

        #endregion

        #region Properties

        public static EditItemWindow Instance { get; private set; }

        #endregion

        public static void ShowWindow(ItemInfo item)
        {
            if (Instance != null)
            {
                ItemsDatabaseWindow.Instance.OnClose -= Instance.Close;
                Instance.Close();
            }
            Instance = GetWindow(typeof(EditItemWindow), false, "EditItem") as EditItemWindow;
            ItemsDatabaseWindow.Instance.OnClose += Instance.Close;

            Instance._item = item;
        }

        protected override void OnDestroy()
        {
            this._item.SetDirty();
            base.OnDestroy();
        }

        private void OnRename()
        {
            if (this._item != null)
            {
                string path = AssetDatabase.GetAssetPath(this._item);
                string message = AssetDatabase.RenameAsset(path, this._item.Tag);
                if (!string.IsNullOrEmpty(message))
                {
                    EditorUtility.DisplayDialog("Rename Error", message, "OK", null);
                }
            }
        }

        private void OnEnable()
        {
            Type[] componentTypes = Assembly.GetAssembly(typeof(ComponentInfoBase)).GetTypes();
            this._componentTypeNames =
                componentTypes.Where(e => e.IsSubclassOf(typeof(ComponentInfoBase))).Select(e => e.Name).ToArray();
        }

        private void OnGUI()
        {
            if (this._item != null)
            {
                this._scrollPos = EditorGUILayout.BeginScrollView(this._scrollPos);
                Editor.CreateEditor(this._item).DrawDefaultInspector();

                GUILayout.Space(10);

                if (GUILayout.Button("Rename Asset File"))
                {
                    this.OnRename();
                }

                EditorGUILayout.BeginHorizontal();

                this._componentTypeIndex = EditorGUILayout.Popup(this._componentTypeIndex, this._componentTypeNames);

                Color prevAddComponenButtonColor = GUI.color;
                GUI.color = Color.green;
                if (GUILayout.Button("Add Component"))
                {
                    if (this._componentTypeIndex < 0) return;

                    ComponentInfoBase componentBase = CreateInstance(this._componentTypeNames[this._componentTypeIndex]) as ComponentInfoBase;
                    if (componentBase != null)
                    {
                        this._item.AddComponent(componentBase);
                        CustomEditorUtils.AddAssetToAsset(componentBase, this._item);
                    }
                    else
                    {
                        throw new ArgumentException("component is NULL.");
                    }
                }
                GUI.color = prevAddComponenButtonColor;

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginVertical();
                GUILayout.Label("Item Components", EditorStyles.boldLabel);
                int indexToDelete = -1;
                for (int i = 0; i < this._item.ComponentInfos.Count; i++)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    if (this._item.ComponentInfos[i] != null)
                    {
                        Editor.CreateEditor(this._item.ComponentInfos[i]).OnInspectorGUI();
                    }

                    Color prevDeleteComponentButtonColor = GUI.color;
                    GUI.color = Color.red;
                    if (GUILayout.Button("Delete Component")) indexToDelete = i;
                    GUI.color = prevDeleteComponentButtonColor;
                    EditorGUILayout.EndVertical();
                }

                if (indexToDelete > -1)
                {
                    bool result = EditorUtility.DisplayDialog("Delete component",
                                                              "Delete component: {0}".F(
                                                                                        this._item.ComponentInfos[
                                                                                                              indexToDelete
                                                                                                             ].GetType()),
                                                              "OK", "Cancel");

                    if (result)
                    {
                        this._item.RemoveComponent(this._item.ComponentInfos[indexToDelete]);
                        DestroyImmediate(this._item.ComponentInfos[indexToDelete], true);
                        
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.EndScrollView();
            }
        }
    }
}