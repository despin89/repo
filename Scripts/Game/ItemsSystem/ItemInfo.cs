#if UNITY_EDITOR
    using UnityEditor;
#endif

namespace GD.Game.ItemsSystem
{
    using System.Collections.Generic;
    using ActionsSystem;
    using Attributes;
    using Core.Localization;
    using UnityEngine;

    public sealed class ItemInfo : ScriptableObject, ILocalizable
    {
        #region Fields

        public int Value = -1;

        public ItemSubtype Subtype;

        public ItemType Type = ItemType.None;

        [HideInInspector]
        public List<ComponentInfoBase> ComponentInfos = new List<ComponentInfoBase>();

        public LocalizableString Name;

        public LocalizableString Tooltip;

        [PreviewSprite]
        public Sprite ItemSprite;

        public string Tag = "None";

        [TextArea]
        public string OnConsumeScript;

        #endregion

        #region Properties

        public string LocalizationGroup
        {
            get { return "Items"; }
        }

        #endregion

        #region Implementations

        public IEnumerable<LocalizableString> GetLocalizableStrings()
        {
            return new[] {this.Name, this.Tooltip};
        }

        #endregion

        public void AddComponent(ComponentInfoBase componentBase)
        {
            this.ComponentInfos.Add(componentBase);
        }

        public void RemoveComponent(ComponentInfoBase componentBase)
        {
            this.ComponentInfos.Remove(componentBase);
        }

#if UNITY_EDITOR
        public new void SetDirty()
        {
            EditorUtility.SetDirty(this);
            foreach (ComponentInfoBase component in this.ComponentInfos)
            {
                EditorUtility.SetDirty(component);
            }
        }
#endif
    }

    public enum ItemType
    {
        None,
        Weapon,
        Armor,
        Trinket,
        Consumable,
        Rune
    }
}