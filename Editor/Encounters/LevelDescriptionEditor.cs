namespace GD.EditorExtentions
{
    using Extentions;
    using Game.EncountersSystem;
    using Game.MapSystem;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(LevelDescription))]
    public class LevelDescriptionEditor : Editor
    {
        #region Fields

        private Vector2 _scrollPos;

        #endregion

        public override void OnInspectorGUI()
        {
            LevelDescription description = this.target as LevelDescription;

            description.LevelEnvironment =
                EditorGUILayout.ObjectField("Level Type", description.LevelEnvironment, typeof(LevelEnvironmentEnum), false) as
                    LevelEnvironmentEnum;

            GUILayout.Space(4);
            if (CustomEditorUtils.GetButton("Add Description", Color.green))
            {
                description.EncounterModels.Add(new EncounterModel());
            }
            GUILayout.Space(4);

            this._scrollPos = EditorGUILayout.BeginScrollView(this._scrollPos);

            int indexToDelete = -1;
            for (int i = 0; i < description.EncounterModels.Count; i++)
            {
                if (description.EncounterModels[i] != null)
                {
                    GUILayout.BeginVertical(GUI.skin.box);
                    description.EncounterModels[i].Encounter =
                        EditorGUILayout.ObjectField("Encounter", description.EncounterModels[i].Encounter, typeof(EncounterAsset),
                                                    false) as EncounterAsset;
                    description.EncounterModels[i].MinCount = EditorGUILayout.IntField("MinCount",
                                                                                       description.EncounterModels[i]
                                                                                           .MinCount);
                    description.EncounterModels[i].MaxCount = EditorGUILayout.IntField("MaxCount",
                                                                                       description.EncounterModels[i]
                                                                                           .MaxCount);

                    if (CustomEditorUtils.GetButton("Delete Description", Color.red))
                    {
                        indexToDelete = i;
                    }

                    EditorGUILayout.EndVertical();
                }

                GUILayout.Space(4);
            }

            if (indexToDelete > -1)
            {
                bool result = EditorUtility.DisplayDialog("Delete component",
                                                          "Delete component: {0}".F(
                                                                                    description.EncounterModels[
                                                                                                                indexToDelete
                                                                                                               ].GetType
                                                                                        ()),
                                                          "OK", "Cancel");

                if (result)
                {
                    description.EncounterModels.Remove(description.EncounterModels[indexToDelete]);
                }
            }

            EditorGUILayout.EndScrollView();

            EditorUtility.SetDirty(this.target);
        }
    }
}