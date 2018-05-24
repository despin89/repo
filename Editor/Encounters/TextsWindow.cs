namespace GD.EditorExtentions
{
    using System.Linq;
    using Core.Localization;
    using Game.EncountersSystem;
    using UnityEditor;
    using UnityEngine;

    public class TextsWindow : EditorWindow
    {
        #region Fields

        private Vector2 _scrollPos;

        private EncounterInfo _encounterInfo;

        #endregion

        #region Properties

        public static TextsWindow Instance { get; private set; }

        #endregion

        public static void ShowWindow(EncounterInfo item)
        {
            Instance = GetWindow(typeof(TextsWindow), false, "Texts") as TextsWindow;
            Instance._encounterInfo = item;
        }

        private void OnDisable()
        {
            this._encounterInfo.SetDirty();
        }

        private void OnGUI()
        {
            if (this._encounterInfo != null)
            {
                if (EditorWindowBase.GetButton("Add Text", 80, Color.green))
                {
                    this._encounterInfo.Texts.Add(new LocalizableString());
                    this._encounterInfo.Texts.Last().Text = string.Empty;
                }

                this._scrollPos = EditorGUILayout.BeginScrollView(this._scrollPos);

                GUILayout.Space(10);

                if (this._encounterInfo.Texts.Count > 0)
                {
                    int indexToDeleteText = -1;
                    for (int i = 0; i < this._encounterInfo.Texts.Count; i++)
                    {
                        var text = this._encounterInfo.Texts[i];
                        if (text != null)
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorStyles.textField.wordWrap = true;
                            text.Text = EditorGUILayout.TextArea(text.Text, GUILayout.Width(400), GUILayout.Height(100));

                            if (EditorWindowBase.GetButton("-", 20, Color.red)) indexToDeleteText = i;

                            EditorGUILayout.EndHorizontal();

                            if (indexToDeleteText > -1)
                            {
                                bool result = EditorUtility.DisplayDialog("Delete", "Delete Text",
                                                                          "OK", "Cancel");
                                if (result)
                                {
                                    this._encounterInfo.Texts.RemoveAt(indexToDeleteText);
                                }

                                indexToDeleteText = -1;
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
