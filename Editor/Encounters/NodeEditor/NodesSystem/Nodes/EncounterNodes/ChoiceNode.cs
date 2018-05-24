using System.Linq;
namespace GD.EditorExtentions
{
    using Game.EncountersSystem;
    using UnityEditor;
    using UnityEngine;

    [GraphContextMenuItem("Encounters", "Choice")]
    public class ChoiceNode : EncounterNodeBase
    {
        public ChoiceNode(int id, Graph parent) : base(id, parent)
        {
            this.TitleColor = GetMonoTex(new Color(1F, 0.3F, 0.3F, 1));

            this.Sockets.Add(new OutputSocket(this, "Choice"));

            this.Encounter.Texts.First().Text = "CHOICE_TEXT";

            this.Sockets.Add(new InputSocket(this, "Encounter"));
            this.Sockets.Add(new InputSocket(this, "Encounter"));
            this.Sockets.Add(new InputSocket(this, "Encounter"));
            this.Sockets.Add(new InputSocket(this, "Encounter"));
            this.Sockets.Add(new InputSocket(this, "Encounter"));
            this.Sockets.Add(new InputSocket(this, "Encounter"));
            this.Sockets.Add(new InputSocket(this, "Encounter"));

            this.Encounter.NodeType = EncounterNodeType.Choice;
            this.Height = 240;
            this.Width = 240;
        }

        public override void OnGUI()
        {
            EditorStyles.textField.wordWrap = true;
            GUILayout.BeginVertical(new GUIStyle(GUI.skin.box)
            {
                normal = { background = Node.GetMonoTex(Color.gray) }
            });
            
            this.Encounter.Texts.First().Text = EditorGUILayout.TextArea(this.Encounter.Texts.First().Text, GUILayout.Width(220), GUILayout.Height(140));

            GUILayout.Space(5);

            if (GUILayout.Button("Edit Texts"))
                TextsWindow.ShowWindow(this.Encounter);

            if (GUILayout.Button("Edit Events"))
                EventsWindow.ShowWindow(this.Encounter);

            if (GUILayout.Button("Edit Allowers"))
                AllowersWindow.ShowWindow(this.Encounter);

            GUILayout.EndVertical();
        }
    }
}
