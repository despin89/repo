namespace GD.EditorExtentions
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Game.EncountersSystem;
    using UnityEditor;
    using UnityEngine;

    public class EventsWindow : EditorWindow
    {
        #region Fields

        private EncounterInfo _encounterInfo;

        private int _eventTypeIndex = -1;

        private string[] _componentTypeNames = new string[0];

        private Vector2 _scrollPos;

        #endregion

        #region Properties

        public static EventsWindow Instance { get; private set; }

        #endregion

        public static void ShowWindow(EncounterInfo item)
        {
            Instance = GetWindow(typeof(EventsWindow), false, "EventsWindow") as EventsWindow;
            Instance._encounterInfo = item;
        }

        protected void OnDisable()
        {
            this._encounterInfo.SetDirty();
            this._encounterInfo.Events.ForEach(_ => _.SetDirty());
        }

        private void OnEnable()
        {
            Type[] componentTypes = Assembly.GetAssembly(typeof(EventInfoBase)).GetTypes();
            this._componentTypeNames =
                componentTypes.Where(e => e.IsSubclassOf(typeof(EventInfoBase))).Select(e => e.Name).ToArray();
        }

        private void OnGUI()
        {
            if (this._encounterInfo != null)
            {
                this._eventTypeIndex = EditorGUILayout.Popup(this._eventTypeIndex, this._componentTypeNames);

                if (EditorWindowBase.GetButton("Add Event", 80, Color.green))
                {
                    if (this._eventTypeIndex < 0) return;

                    EventInfoBase eventInfoBase = CreateInstance(this._componentTypeNames[this._eventTypeIndex]) as EventInfoBase;
                    if (eventInfoBase != null)
                    {
                        this._encounterInfo.Events.Add(eventInfoBase);
                    }
                    else
                    {
                        throw new ArgumentException("component is NULL.");
                    }
                }

                this._scrollPos = EditorGUILayout.BeginScrollView(this._scrollPos);

                GUILayout.Space(10);

                if (this._encounterInfo.Events.Count > 0)
                {
                    int indexToDelete = -1;
                    for (int i = 0; i < this._encounterInfo.Events.Count; i++)
                    {
                        var @event = this._encounterInfo.Events[i];
                        if (@event != null)
                        {
                            EditorGUILayout.BeginVertical(GUI.skin.box);
                            Editor.CreateEditor(this._encounterInfo.Events[i]).OnInspectorGUI();

                            if (EditorWindowBase.GetButton("-", 20, Color.red)) indexToDelete = i;

                            EditorGUILayout.EndVertical();

                            if (indexToDelete > -1)
                            {
                                bool result = EditorUtility.DisplayDialog("Delete", "Delete Event",
                                                                          "OK", "Cancel");
                                if (result)
                                {
                                    this._encounterInfo.Events.Remove(this._encounterInfo.Events[indexToDelete]);
                                    DestroyImmediate(this._encounterInfo.Events[indexToDelete], true);

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