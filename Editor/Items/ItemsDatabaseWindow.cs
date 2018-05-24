namespace GD.EditorExtentions
{
    using System.Collections.Generic;
    using Extentions;
    using Game.ActionsSystem;
    using Game.ItemsSystem;
    using UnityEditor;
    using UnityEngine;

    public class ItemsDatabaseWindow : EditorWindowBase
    {
        #region Fields

        private List<ItemsGroup> _itemsGroups = new List<ItemsGroup>();

        private Vector2 _scrollPos;

        #endregion

        #region Properties

        public static ItemsDatabaseWindow Instance { get; private set; }

        #endregion

        [MenuItem("Window/Items Database")]
        public static void ShowWindow()
        {
            Instance = GetWindow(typeof(ItemsDatabaseWindow), false, "Items Database") as ItemsDatabaseWindow;
        }

        private void OnEnable()
        {
            ItemsGroup[] itemsGroups = Resources.LoadAll<ItemsGroup>("Items/~ItemsGroups");

            if (itemsGroups != null)
            {
                this._itemsGroups.AddRange(itemsGroups);
            }
        }

        private void OnGUI()
        {
            this._scrollPos = EditorGUILayout.BeginScrollView(this._scrollPos);

            if (GUILayout.Button("Add Items Group"))
            {
                ItemsGroup itemsGroup = CreateInstance<ItemsGroup>();
                if (itemsGroup != null)
                {
                    this._itemsGroups.Add(itemsGroup);
                    CustomEditorUtils.SaveAsset(itemsGroup, "Assets/Resources/Items/~ItemsGroups/", "NewItemsGroup");
                }
            }

            GUILayout.Space(10);

            if (this._itemsGroups.Count > 0)
            {
                int indexToDeleteGroup = -1;
                for (int i = 0; i < this._itemsGroups.Count; i++)
                {
                    ItemsGroup itemGroup = this._itemsGroups[i];
                    if (itemGroup != null)
                    {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                        itemGroup.Name = EditorGUILayout.TextField(itemGroup.Name,
                                                                   new GUIStyle
                                                                   {
                                                                       fontSize = 14,
                                                                       fontStyle = FontStyle.Bold
                                                                   });
                        EditorUtility.SetDirty(itemGroup);

                        if (GUILayout.Button("Add Item"))
                        {
                            ItemInfo item = CreateInstance<ItemInfo>();
                            if (item != null)
                            {
                                itemGroup.Items.Add(item);
                                CustomEditorUtils.SaveAsset(item, "Assets/Resources/Items/ItemsData/", "NewItem", itemGroup);
                            }
                        }

                        Color prevDeleteGroupButtonColor = GUI.color;
                        GUI.color = Color.red;
                        if (GUILayout.Button("Delete Group")) indexToDeleteGroup = i;
                        GUI.color = prevDeleteGroupButtonColor;

                        GUILayout.Space(10);

                        EditorGUI.BeginDisabledGroup(itemGroup.Items.Count == 0);
                        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                        itemGroup.IsObjectsShown = EditorGUILayout.Foldout(itemGroup.IsObjectsShown,
                                                                           "{0}".F(itemGroup.Name), true,
                                                                           new GUIStyle
                                                                           {
                                                                               fontStyle = FontStyle.Bold,
                                                                               fontSize = 12
                                                                           });
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.EndDisabledGroup();

                        GUI.color = Color.grey;
                        if (itemGroup.IsObjectsShown)
                        {
                            int indexToDeleteItem = -1;
                            for (int j = 0; j < itemGroup.Items.Count; j++)
                            {
                                if (itemGroup.Items[j] != null)
                                {
                                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                                    EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

                                    EditorGUILayout.BeginVertical(GUILayout.MinWidth(200.0f));
                                    EditorGUIUtility.labelWidth = 40F;
                                    EditorGUILayout.LabelField("Name: ", itemGroup.Items[j].Name.Text,
                                                               new GUIStyle { fontStyle = FontStyle.Bold });
                                    EditorGUILayout.LabelField("Tag: ", itemGroup.Items[j].Tag,
                                                               new GUIStyle { fontStyle = FontStyle.Bold });
                                    EditorGUILayout.LabelField("Type: ", itemGroup.Items[j].Type.ToString(),
                                                               new GUIStyle { fontStyle = FontStyle.Bold });
                                    EditorGUILayout.EndVertical();

                                    Rect rect = GUILayoutUtility.GetLastRect();

                                    if (itemGroup.Items[j].ItemSprite)
                                    {
                                        rect.x += 200F;
                                        EditorGUILayout.BeginVertical(GUILayout.MinWidth(50.0f));
                                        //GUI.DrawTexture(rect, itemGroup.Items[j].ItemSprite.texture,
                                        //    ScaleMode.ScaleToFit);

                                        rect.height = 50.0f;
                                        rect.width = 50.0f;

                                        Rect texRect = new Rect(0, 0, itemGroup.Items[j].ItemSprite.texture.width,
                                                                itemGroup.Items[j].ItemSprite.texture.height);

                                        Vector2 min = Rect.PointToNormalized(texRect,
                                                                             itemGroup.Items[j].ItemSprite.rect.min);

                                        float height = itemGroup.Items[j].ItemSprite.rect.height /
                                                       itemGroup.Items[j].ItemSprite.texture.height;
                                        float width = itemGroup.Items[j].ItemSprite.rect.width /
                                                      itemGroup.Items[j].ItemSprite.texture.width;

                                        GUI.DrawTextureWithTexCoords(rect, itemGroup.Items[j].ItemSprite.texture,
                                                                     new Rect(min.x, min.y, width, height));

                                        EditorGUILayout.EndVertical();
                                    }
                                    EditorGUILayout.EndHorizontal();

                                    if (GUILayout.Button("Edit Item"))
                                    {
                                        EditItemWindow.ShowWindow(itemGroup.Items[j]);
                                    }

                                    Color prevDeleteItemButtonColor = GUI.color;
                                    GUI.color = Color.red;
                                    if (GUILayout.Button("Delete Item")) indexToDeleteItem = j;
                                    GUI.color = prevDeleteItemButtonColor;

                                    EditorGUILayout.EndVertical();
                                }

                                GUILayout.Space(15);

                                if (indexToDeleteItem > -1)
                                {
                                    bool result = EditorUtility.DisplayDialog("Delete item",
                                                                              "Delete item: {0}".F(
                                                                                                   itemGroup.Items[
                                                                                                                   indexToDeleteItem
                                                                                                                  ].Name),
                                                                              "OK", "Cancel");

                                    if (result)
                                    {
                                        foreach (
                                            ComponentInfoBase component in itemGroup.Items[indexToDeleteItem].ComponentInfos)
                                        {
                                            DestroyImmediate(component, true);
                                        }

                                        string path = AssetDatabase.GetAssetPath(itemGroup.Items[indexToDeleteItem]);
                                        AssetDatabase.DeleteAsset(path);

                                        DestroyImmediate(itemGroup.Items[indexToDeleteItem], true);
                                        itemGroup.Items.RemoveAt(indexToDeleteItem);

                                        AssetDatabase.SaveAssets();
                                        AssetDatabase.Refresh();
                                    }

                                    indexToDeleteItem = -1;
                                    this.Repaint();
                                }
                            }
                        }
                        GUI.color = Color.white;

                        EditorGUILayout.EndVertical();

                        if (indexToDeleteGroup > -1)
                        {
                            bool result = EditorUtility.DisplayDialog("Delete item group",
                                                                      "Delete item group: {0}".F(
                                                                                                 this._itemsGroups[
                                                                                                                   indexToDeleteGroup
                                                                                                                  ].Name),
                                                                      "OK", "Cancel");
                            if (result)
                            {
                                foreach (ItemInfo item in this._itemsGroups[indexToDeleteGroup].Items)
                                {
                                    string path = AssetDatabase.GetAssetPath(item);
                                    AssetDatabase.DeleteAsset(path);

                                    DestroyImmediate(item, true);
                                }

                                string assetPath = AssetDatabase.GetAssetPath(this._itemsGroups[indexToDeleteGroup]);
                                AssetDatabase.DeleteAsset(assetPath);

                                DestroyImmediate(this._itemsGroups[indexToDeleteGroup], true);
                                this._itemsGroups.RemoveAt(indexToDeleteGroup);

                                AssetDatabase.SaveAssets();
                                AssetDatabase.Refresh();
                            }

                            indexToDeleteGroup = -1;
                            this.Repaint();
                        }
                    }
                }
            }

            EditorGUILayout.EndScrollView();
        }
    }
}