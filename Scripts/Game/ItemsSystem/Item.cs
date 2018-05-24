namespace GD.Game.ItemsSystem
{
    using System.Collections.Generic;
    using System.Linq;
    using ActionsSystem;
    using UI.PopupWindows;
    using UnityEngine;

    public sealed class Item : ITooltip
    {
        #region Fields

        private int _value;

        private List<ComponentBase> _components;

        private Sprite _itemSprite;

        private string _subtype;

        private string _tag;

        #endregion

        #region Properties

        public string OnConsumeScript { get; private set; }

        public bool IsEquipable
        {
            get
            {
                return (this.Type != ItemType.Consumable) && (this.Type != ItemType.None) && (this.Type != ItemType.Rune);
            }
        }

        public int Value
        {
            get { return this._value; }
        }

        public string Subtype
        {
            get { return this._subtype; }
        }

        public List<ComponentBase> Components
        {
            get { return this._components; }
        }

        public string Name { get; }

        public string Tooltip { get; }

        public Sprite ItemSprite
        {
            get { return this._itemSprite; }
        }

        public string Tag
        {
            get { return this._tag; }
        }

        public ItemType Type { get; }

        #endregion

        #region Constructors

        public Item(ItemInfo info)
        {
            this._value = info.Value;
            this._subtype = info.Subtype?.Name ?? "None";
            this.Type = info.Type;
            this.Name = info.Name.Text;
            this.Tooltip = info.Tooltip.Text;
            this._itemSprite = info.ItemSprite;
            this._tag = info.Tag;
            this.OnConsumeScript = info.OnConsumeScript;

            this._components = info.ComponentInfos.Select(_ => _.GetInstance()).ToList();
        }

        #endregion

        #region Implementations

        public string GenerateTooltip()
        {
            return this.Tooltip;
        }

        #endregion

        public override string ToString()
        {
            return this.Type + " " + this.Name;
        }
    }
}