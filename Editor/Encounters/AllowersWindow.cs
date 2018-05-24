using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GD.EditorExtentions
{
    using System.Reflection;
    using Game.EncountersSystem;
    using UnityEditor;
    using UnityEngine;

    public class AllowersWindow : EditorWindow
    {
        #region Fields

        private EncounterInfo _encounterInfo;

        private int _allowerTypeIndex = -1;

        private string[] _allowerTypeNames = new string[0];

        private Vector2 _scrollPos;

        #endregion

        #region Properties

        public static AllowersWindow Instance { get; private set; }

        #endregion

        public static void ShowWindow(EncounterInfo item)
        {
            Instance = GetWindow(typeof(AllowersWindow), false, "AllowersWindow") as AllowersWindow;
            Instance._encounterInfo = item;
        }

        protected void OnDisable()
        {
            this._encounterInfo.SetDirty();
            this._encounterInfo.Allowers.ForEach(_ => _.SetDirty());
        }

        private void OnEnable()
        {
            Type[] componentTypes = Assembly.GetAssembly(typeof(AllowerInfoBase)).GetTypes();
            this._allowerTypeNames =
                componentTypes.Where(e => e.IsSubclassOf(typeof(AllowerInfoBase))).Select(e => e.Name).ToArray();
        }

        private void OnGUI()
        {
            if (this._encounterInfo != null)
            {
                this._allowerTypeIndex = EditorGUILayout.Popup(this._allowerTypeIndex, this._allowerTypeNames);

                if (EditorWindowBase.GetButton("Add Allower", 80, Color.green))
                {
                    if (this._allowerTypeIndex < 0) return;

                    AllowerInfoBase allowerInfoBase = CreateInstance(this._allowerTypeNames[this._allowerTypeIndex]) as AllowerInfoBase;
                    if (allowerInfoBase != null)
                    {
                        this._encounterInfo.Allowers.Add(allowerInfoBase);
                        CustomEditorUtils.AddAssetToAsset(allowerInfoBase, this._encounterInfo);
                    }
                    else
                    {
                        throw new ArgumentException("component is NULL.");
                    }
                }

                this._scrollPos = EditorGUILayout.BeginScrollView(this._scrollPos);

                GUILayout.Space(10);

                if (this._encounterInfo.Allowers.Count > 0)
                {
                    int indexToDelete = -1;
                    for (int i = 0; i < this._encounterInfo.Allowers.Count; i++)
                    {
                        var allower = this._encounterInfo.Allowers[i];
                        if (allower != null)
                        {
                            EditorGUILayout.BeginVertical(GUI.skin.box);
                            Editor.CreateEditor(this._encounterInfo.Allowers[i]).OnInspectorGUI();

                            if (EditorWindowBase.GetButton("-", 20, Color.red)) indexToDelete = i;

                            EditorGUILayout.EndVertical();

                            if (indexToDelete > -1)
                            {
                                bool result = EditorUtility.DisplayDialog("Delete", "Delete Event",
                                                                          "OK", "Cancel");
                                if (result)
                                {
                                    this._encounterInfo.Allowers.Remove(this._encounterInfo.Allowers[indexToDelete]);
                                    DestroyImmediate(this._encounterInfo.Allowers[indexToDelete], true);

                                    AssetDatabase.SaveAssets();
                                    AssetDatabase.Refresh();
                                }

                                this.Repaint();
                            }
                        }
                    }
                }

                EditorGUILayout.EndScrollView();
            }
        }
    }
}
