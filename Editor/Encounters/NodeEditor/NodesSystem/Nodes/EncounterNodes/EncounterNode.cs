namespace GD.EditorExtentions
{
    using System.Linq;
    using Game.EncountersSystem;
    using UnityEditor;
    using UnityEngine;

    [GraphContextMenuItem("Encounters", "Encounter")]
    public class EncounterNode : EncounterNodeBase
    {
        public EncounterNode(int id, Graph parent) : base(id, parent)
        {
            this.TitleColor = GetMonoTex(new Color(0.4F, 0.9F, 0.1F, 1));

            this.Sockets.Add(new OutputSocket(this, "Encounter"));

            this.Sockets.Add(new InputSocket(this, "Choice"));
            this.Sockets.Add(new InputSocket(this, "Choice"));
            this.Sockets.Add(new InputSocket(this, "Choice"));
            this.Sockets.Add(new InputSocket(this, "Choice"));
            this.Sockets.Add(new InputSocket(this, "Choice"));
            this.Sockets.Add(new InputSocket(this, "Choice"));
            this.Sockets.Add(new InputSocket(this, "Choice"));

            this.Encounter.Texts.First().Text = "ENCOUNTER_TEXT";

            this.Encounter.NodeType = EncounterNodeType.Encounter;
            this.Height = 300;
            this.Width = 240;
        }

        public override void OnGUI()
        {
            EditorStyles.textField.wordWrap = true;
            GUILayout.BeginVertical(new GUIStyle(GUI.skin.box)
            {
                normal = {background = GetMonoTex(Color.gray)}
            });
            
            this.Encounter.Texts.First().Text = EditorGUILayout.TextArea(this.Encounter.Texts.First().Text, GUILayout.Width(220), GUILayout.Height(180));

            GUILayout.Space(5);

            if (GUILayout.Button("Edit Texts"))
                TextsWindow.ShowWindow(this.Encounter);
           
            if (GUILayout.Button("Edit Events"))
                EventsWindow.ShowWindow(this.Encounter);

            GUILayout.EndVertical();
        }
    }
}
