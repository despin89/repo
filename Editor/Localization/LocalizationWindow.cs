namespace GD.EditorExtentions
{
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using Core.Localization;
    using Extentions;
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;
    using LocalizationDatabase = Core.Localization.LocalizationDatabase;

    public class LocalizationWindow : EditorWindowBase
    {
        #region Fields

        private bool _showStaticStrings;

        private bool _showAllStrings;

        private bool _showKeyStrings;

        private LocalizationDatabase _localizationDatabase;

        private string _staticStringText;

        private string _staticStringAlias;

        private string _keyStringText;

        private string _keyStringKey;

        private string[] _localizableAssetTypes;

        #endregion

        #region Properties

        public static LocalizationWindow Instance { get; private set; }

        #endregion

        [MenuItem("Window/Localization Database")]
        public static void ShowWindow()
        {
            Instance = GetWindow(typeof(LocalizationWindow), false, "Localization Database") as LocalizationWindow;
        }

        private void OnEnable()
        {
            this._localizationDatabase = Resources.Load<LocalizationDatabase>("Localization/LocalizationDatabase");

            if (this._localizationDatabase == null)
            {
                LocalizationDatabase localizationDatabase =
                    CreateInstance("LocalizationDatabase") as LocalizationDatabase;

                if (localizationDatabase != null)
                {
                    this._localizationDatabase = localizationDatabase;
                    CustomEditorUtils.SaveAssetWithStrongName(localizationDatabase, "Assets/Resources/Localization/",
                                                              "LocalizationDatabase");
                }
            }
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Generate Localization File"))
            {
                this.GenerateStringsFile();
            }

            if (GUILayout.Button("Get Scenes Localization"))
            {
                EditorSceneManager.SaveOpenScenes();

                IEnumerable<string> scenesNames =
                    Directory.GetFiles(@"Assets\Scenes\", "*.unity").Select(Path.GetFileName);

                foreach (string scenesName in scenesNames)
                {
                    EditorSceneManager.OpenScene(@"Assets\Scenes\" + scenesName);

                    IEnumerable<ILocalizable> localizables =
                        FindObjectsOfType<MonoBehaviour>().OfType<ILocalizable>().ToArray();
                    this.GetLocalization(localizables);

                    EditorSceneManager.MarkAllScenesDirty();
                    EditorSceneManager.SaveOpenScenes();
                }

                EditorUtility.SetDirty(this._localizationDatabase);
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
            }

            if (GUILayout.Button("Get Assets Localization"))
            {
                List<ILocalizable> localizables = new List<ILocalizable>();
                string[] localizableAssetsGUIDs = AssetDatabase.FindAssets("t:ScriptableObject");

                localizables.AddRange(
                                      localizableAssetsGUIDs.Select(
                                                                    guid =>
                                                                        AssetDatabase.LoadAssetAtPath<ScriptableObject>(
                                                                                                                        AssetDatabase
                                                                                                                            .GUIDToAssetPath
                                                                                                                            (guid)))
                                          .OfType<ILocalizable>());

                this.GetLocalization(localizables);

                EditorUtility.SetDirty(this._localizationDatabase);
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
            }

            GUILayout.Space(20);

            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            this._showStaticStrings = EditorGUILayout.Foldout(this._showStaticStrings, "Static Strings", true,
                                                              new GUIStyle
                                                              {
                                                                  fontStyle = FontStyle.Bold,
                                                                  fontSize = 12
                                                              });
            EditorGUILayout.EndHorizontal();

            if (this._showStaticStrings)
            {
                GUI.SetNextControlName("AddStaticStringButton");
                if (GUILayout.Button("Add Static String"))
                {
                    if (!string.IsNullOrEmpty(this._staticStringText) &&
                        (this._staticStringText != "Static String Text") &&
                        !string.IsNullOrEmpty(this._staticStringAlias) &&
                        (this._staticStringAlias != "Static String Alias"))
                    {
                        if (Regex.IsMatch(this._staticStringAlias, @"^[a-zA-Z_]+$"))
                        {
                            string error = this._localizationDatabase.TryAddString(this._staticStringText,
                                                                                   this._staticStringAlias);

                            if (!string.IsNullOrEmpty(error))
                            {
                                Debug.LogError(error);
                                EditorUtility.DisplayDialog("Error", error, "OK");
                            }
                            else
                            {
                                EditorUtility.SetDirty(this._localizationDatabase);
                                AssetDatabase.SaveAssets();
                                AssetDatabase.Refresh();

                                this._staticStringAlias = string.Empty;
                                this._staticStringText = string.Empty;

                                GUI.FocusControl("AddStaticStringButton");
                                GUI.FocusControl("_staticStringAlias");
                            }
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Alias Error", "Alias must contain only letters and underscore",
                                                        "OK");
                        }
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Error", "Cannot add empty data",
                                                    "OK");
                    }
                }

                GUI.SetNextControlName("_staticStringAlias");
                this._staticStringAlias = EditorGUILayout.TextField("Static String Alias", this._staticStringAlias);
                this._staticStringText = EditorGUILayout.TextField("Static String Text", this._staticStringText);

                GUILayout.Space(10);

                if (this._localizationDatabase.CachedStrings.Count(e => !string.IsNullOrEmpty(e.Value.Alias)) > 0)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                    GUILayout.Label("Static Strings", EditorStyles.boldLabel);

                    int indexToDeleteString = -1;
                    LocalizableString[] cachedStrings =
                        this._localizationDatabase.CachedStrings.Values.Where(e => !string.IsNullOrEmpty(e.Alias))
                            .ToArray();

                    for (int i = 0; i < cachedStrings.Length; i++)
                    {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                        GUILayout.Label(cachedStrings[i].Alias);
                        GUI.color = Color.red;
                        if (GUILayout.Button("Delete", GUILayout.Width(100))) indexToDeleteString = i;
                        GUI.color = Color.white;
                        EditorGUILayout.EndHorizontal();
                        GUILayout.Label(cachedStrings[i].Text, CustomEditorUtils.ItalicStyle());
                        EditorGUILayout.EndVertical();
                    }

                    EditorGUILayout.EndVertical();

                    if (indexToDeleteString > -1)
                    {
                        bool result = EditorUtility.DisplayDialog("Delete string",
                                                                  "Delete string: {0}".F(cachedStrings[
                                                                                                       indexToDeleteString
                                                                                                      ].Text),
                                                                  "OK", "Cancel");
                        if (result)
                        {
                            string error = this._localizationDatabase.TryRemoveStringById(cachedStrings[
                                                                                                        indexToDeleteString
                                                                                                       ].Id);

                            if (!string.IsNullOrEmpty(error))
                            {
                                Debug.LogError(error);
                            }
                            else
                            {
                                EditorUtility.SetDirty(this._localizationDatabase);
                                AssetDatabase.SaveAssets();
                                AssetDatabase.Refresh();
                            }
                        }

                        this.Repaint();
                    }
                }
            }

            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            this._showAllStrings = EditorGUILayout.Foldout(this._showAllStrings, "All Strings", true,
                                                           new GUIStyle
                                                           {
                                                               fontStyle = FontStyle.Bold,
                                                               fontSize = 12
                                                           });
            EditorGUILayout.EndHorizontal();

            if (this._showAllStrings)
            {
                GUILayout.Space(10);

                if (this._localizationDatabase.CachedStrings.Count > 0)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                    int indexToDeleteString = -1;
                    LocalizableString[] cachedStrings = this._localizationDatabase.CachedStrings.Values.ToArray();

                    for (int i = 0; i < cachedStrings.Length; i++)
                    {
                        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                        GUILayout.Label(cachedStrings[i].Text, CustomEditorUtils.ItalicStyle());
                        GUI.color = Color.red;
                        if (GUILayout.Button("Delete", GUILayout.Width(100))) indexToDeleteString = i;
                        GUI.color = Color.white;
                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.EndVertical();

                    if (indexToDeleteString > -1)
                    {
                        bool result = EditorUtility.DisplayDialog("Delete string",
                                                                  "Delete string: {0}".F(cachedStrings[
                                                                                                       indexToDeleteString
                                                                                                      ].Text),
                                                                  "OK", "Cancel");
                        if (result)
                        {
                            string error = this._localizationDatabase.TryRemoveStringById(cachedStrings[
                                                                                                        indexToDeleteString
                                                                                                       ].Id);

                            if (!string.IsNullOrEmpty(error))
                            {
                                Debug.LogError(error);
                            }
                            else
                            {
                                EditorUtility.SetDirty(this._localizationDatabase);
                                AssetDatabase.SaveAssets();
                                AssetDatabase.Refresh();
                            }
                        }

                        this.Repaint();
                    }
                }
            }

            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            this._showKeyStrings = EditorGUILayout.Foldout(this._showKeyStrings, "Key Strings", true,
                                                           new GUIStyle
                                                           {
                                                               fontStyle = FontStyle.Bold,
                                                               fontSize = 12
                                                           });
            EditorGUILayout.EndHorizontal();

            if (this._showKeyStrings)
            {
                GUI.SetNextControlName("AddKeyStringButton");
                if (GUILayout.Button("Add Key String"))
                {
                    if (!string.IsNullOrEmpty(this._keyStringText) &&
                        (this._keyStringText != "Key String Text") &&
                        !string.IsNullOrEmpty(this._keyStringKey) &&
                        (this._keyStringKey != "String Key"))
                    {
                        if (Regex.IsMatch(this._keyStringKey, @"^[a-zA-Z_]+$"))
                        {
                            string error = this._localizationDatabase.TryAddKeyString(this._keyStringText,
                                                                                   this._keyStringKey);

                            if (!string.IsNullOrEmpty(error))
                            {
                                Debug.LogError(error);
                                EditorUtility.DisplayDialog("Error", error, "OK");
                            }
                            else
                            {
                                EditorUtility.SetDirty(this._localizationDatabase);
                                AssetDatabase.SaveAssets();
                                AssetDatabase.Refresh();

                                this._keyStringText = string.Empty;
                                this._keyStringKey = string.Empty;

                                GUI.FocusControl("AddKeyStringButton");
                                GUI.FocusControl("_keyStringAlias");
                            }
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Alias Error", "Key must contain only letters and underscore",
                                                        "OK");
                        }
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Error", "Cannot add empty data",
                                                    "OK");
                    }
                }

                GUI.SetNextControlName("_keyStringAlias");
                this._keyStringKey = EditorGUILayout.TextField("String Key", this._keyStringKey);
                this._keyStringText = EditorGUILayout.TextField("Key String Text", this._keyStringText);

                GUILayout.Space(10);

                if (this._localizationDatabase.CachedStrings.Count(e => !string.IsNullOrEmpty(e.Value.Key)) > 0)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                    GUILayout.Label("Key Strings", EditorStyles.boldLabel);

                    int indexToDeleteString = -1;
                    LocalizableString[] cachedStrings =
                        this._localizationDatabase.CachedStrings.Values.Where(e => !string.IsNullOrEmpty(e.Key))
                            .ToArray();

                    for (int i = 0; i < cachedStrings.Length; i++)
                    {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                        GUILayout.Label(cachedStrings[i].Key);
                        GUI.color = Color.red;
                        if (GUILayout.Button("Delete", GUILayout.Width(100))) indexToDeleteString = i;
                        GUI.color = Color.white;
                        EditorGUILayout.EndHorizontal();
                        GUILayout.Label(cachedStrings[i].Text, CustomEditorUtils.ItalicStyle());
                        EditorGUILayout.EndVertical();
                    }

                    EditorGUILayout.EndVertical();

                    if (indexToDeleteString > -1)
                    {
                        bool result = EditorUtility.DisplayDialog("Delete string",
                                                                  "Delete string: {0}".F(cachedStrings[
                                                                                                       indexToDeleteString
                                                                                                      ].Text),
                                                                  "OK", "Cancel");
                        if (result)
                        {
                            string error = this._localizationDatabase.TryRemoveStringById(cachedStrings[
                                                                                                        indexToDeleteString
                                                                                                       ].Id);

                            if (!string.IsNullOrEmpty(error))
                            {
                                Debug.LogError(error);
                            }
                            else
                            {
                                EditorUtility.SetDirty(this._localizationDatabase);
                                AssetDatabase.SaveAssets();
                                AssetDatabase.Refresh();
                            }
                        }

                        this.Repaint();
                    }
                }
            }
        }

        private void GenerateStringsFile()
        {
            if (this._localizationDatabase.CachedStrings.Values.Count(e => !string.IsNullOrEmpty(e.Alias)) > 0)
            {
                CodeCompileUnit compileUnit = new CodeCompileUnit();
                CodeNamespace codeNamespace = new CodeNamespace("GD.Core.Localization");
                CodeTypeDeclaration typeDeclaration = new CodeTypeDeclaration("Localization")
                {
                    IsClass = true,
                    TypeAttributes = TypeAttributes.Public,
                    IsPartial = true
                };
                codeNamespace.Types.Add(typeDeclaration);
                compileUnit.Namespaces.Add(codeNamespace);

                foreach (
                    LocalizableString cachedString in
                    this._localizationDatabase.CachedStrings.Values.Where(e => !string.IsNullOrEmpty(e.Alias)))
                {
                    CodeMemberField field = new CodeMemberField
                    {
                        Name = "_ID_" + cachedString.Id.Replace("-", string.Empty),
                        Type = new CodeTypeReference(typeof(string)),
                        Attributes = (MemberAttributes) 20483,
                        InitExpression = new CodePrimitiveExpression(cachedString.Id)
                    };

                    CodeMemberProperty property = new CodeMemberProperty
                    {
                        Name = cachedString.Alias,
                        Type = new CodeTypeReference(typeof(string)),
                        Attributes = (MemberAttributes) 24579
                    };
                    property.GetStatements.Add(new CodeMethodReturnStatement(
                                                                             new CodeMethodInvokeExpression(
                                                                                                            new CodeMethodReferenceExpression
                                                                                                                (
                                                                                                                 new CodeTypeReferenceExpression
                                                                                                                     (typeof
                                                                                                                     (
                                                                                                                         Localization
                                                                                                                     )),
                                                                                                                 "GetStringById"),
                                                                                                            new CodeFieldReferenceExpression
                                                                                                                (new CodeTypeReferenceExpression
                                                                                                                     (typeof
                                                                                                                     (
                                                                                                                         Localization
                                                                                                                     )),
                                                                                                                 field
                                                                                                                     .Name))));

                    typeDeclaration.Members.Add(field);
                    typeDeclaration.Members.Add(property);
                }

                CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
                CodeGeneratorOptions options = new CodeGeneratorOptions {BracingStyle = "C"};
                using (
                    StreamWriter sourceWriter =
                        new StreamWriter(Application.dataPath + "/Scripts/Core/Localization/Localization.generated.cs"))
                {
                    provider.GenerateCodeFromCompileUnit(
                                                         compileUnit, sourceWriter, options);
                }

                EditorUtility.DisplayDialog("Genaration Complete", "Strings constants file generated!", "OK");
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "No strings to generate!", "OK");
            }
        }

        private void GetLocalization(IEnumerable<ILocalizable> args)
        {
            foreach (ILocalizable item in args)
            {
                EditorUtility.SetDirty((Object) item);
                foreach (LocalizableString localizableString in item.GetLocalizableStrings())
                {
                    if (!string.IsNullOrEmpty(localizableString.Text))
                    {
                        this._localizationDatabase.TryAddLocalizableString(localizableString);

                        EditorUtility.SetDirty(this._localizationDatabase);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                }
            }
        }
    }
}