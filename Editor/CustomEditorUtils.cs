namespace GD.EditorExtentions
{
    using UnityEditor;
    using UnityEngine;

    public static class CustomEditorUtils
    {
        public static GUIStyle ItalicStyle()
        {
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Italic};

            return labelStyle;
        }

        public static void SaveAsset(ScriptableObject so, string path, string fileName, params ScriptableObject[] otherSO)
        {
            INamedGroup namedGroup = so as INamedGroup;
            if (namedGroup != null) namedGroup.Name = fileName;
            so.name = fileName;

            int index = 0;
            while (AssetDatabase.FindAssets(so.name).Length > 0)
            {
                index++;
                so.name = fileName + "_" + index;
                if (namedGroup != null) namedGroup.Name = fileName + "_" + index;
            }

            AssetDatabase.CreateAsset(so, path + so.name + ".asset");

            EditorUtility.SetDirty(so);
            foreach (ScriptableObject o in otherSO)
            {
                EditorUtility.SetDirty(o);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void SaveAssetWithStrongName(ScriptableObject so, string path, string fileName)
        {
            so.name = fileName;
            AssetDatabase.CreateAsset(so, path + so.name + ".asset");
            EditorUtility.SetDirty(so);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void AddAssetToAsset(Object asset, Object parentAsset, bool hide = true)
        {
            if(hide) asset.hideFlags = HideFlags.HideInHierarchy;

            AssetDatabase.AddObjectToAsset(asset, parentAsset);
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(asset));

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static bool GetButton(string label, Color color)
        {
            Color prevAddComponenButtonColor = GUI.color;
            GUI.color = color;
            bool result = GUILayout.Button(label);
            GUI.color = prevAddComponenButtonColor;

            return result;
        }
    }
}