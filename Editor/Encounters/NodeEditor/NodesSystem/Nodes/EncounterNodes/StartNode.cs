namespace GD.EditorExtentions
{
    using System.Linq;
    using Game.EncountersSystem;
    using UnityEditor;
    using UnityEngine;

    [GraphContextMenuItem("Encounters", "Start")]
    public class StartNode : EncounterNodeBase
    {
        public StartNode(int id, Graph parent) : base(id, parent)
        {
            this.TitleColor = GetMonoTex(new Color(0.3F, 0.8F, 1F, 1));

            this.Sockets.Add(new OutputSocket(this, "Encounter"));

            this.Encounter.NodeType = EncounterNodeType.Start;
            this.Encounter.Name = "NEW_ENCOUNTER";
            this.Encounter.Texts.First().Text = "ENCOUNTER_TEXT";


            this.Height = 310;
            this.Width = 240;
        }

        public override void OnGUI()
        {
            EditorStyles.textField.wordWrap = true;
            GUILayout.BeginVertical(new GUIStyle(GUI.skin.box)
            {
                normal = {background = GetMonoTex(Color.gray)}
            });

            GUILayout.Label("Encounter Tag");
            this.Encounter.Name = EditorGUILayout.TextField(this.Encounter.Name, GUILayout.Width(180), GUILayout.Height(20));

            GUILayout.Space(5);

            this.Encounter.Texts.First().Text = EditorGUILayout.TextArea(this.Encounter.Texts.First().Text, GUILayout.Width(220), GUILayout.Height(180));

            GUILayout.Space(5);

            if (GUILayout.Button("Edit Events"))
                EventsWindow.ShowWindow(this.Encounter);

            GUILayout.EndVertical();
        }
    }
}