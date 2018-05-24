namespace GD.UI
{
    using System;
    using Core.MessengerSystem;
    using Extentions;
    using Game;
    using Game.ItemsSystem;
    using PopupWindows;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class ShopSlot : SlotBase, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField]
        private Text _valueText;

        [SerializeField]
        private GameObject _glassPanel;

        private Color _prevBackColor;

        private Image _backImage;

        public void DeactivateValueText()
        {
            this._valueText.text = String.Empty;
        }

        public override bool TryInsertItem(Item item)
        {
            return false;
        }

        public override void DropItemObject(ItemObject droppedItem)
        {
            droppedItem.IsDraggable = false;

            this.InstantiateItemObject(droppedItem);

            this._valueText.text = droppedItem.Item.Value.ToString();

            this._glassPanel.SetLastAndActivate();
        }

        public override void GrabItemObject()
        {
            this.ItemObjectInSlot = null;
        }

        /// <summary>
        ///   <para></para>
        /// </summary>
        /// <param name="eventData">Current event data.</param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (this.ItemObjectInSlot == null)
                return;

            GenericTooltip.ShowTooltip(this.ItemObjectInSlot.Item, null, this.transform.position);
            this._prevBackColor = this._backImage.color;
            this._backImage.color = Color.white;
        }

        /// <summary>
        ///   <para></para>
        /// </summary>
        /// <param name="eventData">Current event data.</param>
        public void OnPointerExit(PointerEventData eventData)
        {
            if (this.ItemObjectInSlot == null)
                return;

            GenericTooltip.Deactivate();
            this._backImage.color = this._prevBackColor;
        }

        /// <summary>
        ///   <para></para>
        /// </summary>
        /// <param name="eventData">Current event data.</param>
        public void OnPointerClick(PointerEventData eventData)
        {
            if(this.ItemObjectInSlot == null)
                return;

            if (GameManager.CurrentWorld.Inventory.IsEnoughGold(this.ItemObjectInSlot.Item.Value))
            {
                GameManager.CurrentWorld.Inventory.Gold -= this.ItemObjectInSlot.Item.Value;
                GameManager.CurrentWorld.Inventory.AddItemToBag(this.ItemObjectInSlot.Item);

                this.Clear();
                this.GrabItemObject();

                this._backImage.color = this._prevBackColor;

                Messenger.Broadcast(M.REFRESH_INV);
            }
        }

        public override void Clear()
        {
            this.DeactivateValueText();
            base.Clear();
        }

        private void Start()
        {
            this._backImage = this.GetComponent<Image>();
        }
    }
}
