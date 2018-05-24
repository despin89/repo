namespace GD.EditorExtentions
{
    using Core.Localization;
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;
    using UnityEngine.UI;

    [CustomEditor(typeof(LocalizableTextWrapper))]
    public class LocalizableTextWrapperEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            LocalizableTextWrapper localizableTextWrapper = this.target as LocalizableTextWrapper;

            if (GUILayout.Button("Apply Localized Text"))
            {
                if (localizableTextWrapper != null)
                {
                    localizableTextWrapper.Text.text = localizableTextWrapper.LocalizableString.Text;

                    EditorUtility.SetDirty(localizableTextWrapper.Text);
                    localizableTextWrapper.Text.Rebuild(CanvasUpdate.Layout);
                    SceneView.RepaintAll();

                    EditorSceneManager.MarkAllScenesDirty();
                    EditorSceneManager.SaveOpenScenes();
                }
            }

            base.OnInspectorGUI();
        }
    }
}