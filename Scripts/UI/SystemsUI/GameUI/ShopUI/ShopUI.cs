namespace GD.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Extentions;
    using Game.ItemsSystem;
    using UnityEngine;

    public class ShopUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject _slotPrfb;

        [SerializeField]
        private GameObject _slotsParent;

        private List<ShopSlot> _lootSlots = new List<ShopSlot>();

        public void InvokeShopUI(List<Item> items)
        {
            this.gameObject.ActivateGO();

            foreach (Item item in items)
            {
                ShopSlot slot = this._lootSlots.FirstOrDefault(_ => _.ItemObjectInSlot == null);
                if (slot == null)
                {
                    throw new Exception("Loot overflow! Maximum size reached!");
                }

                slot.DropItemObject(ItemObject.Instantiate(item));
            }

            foreach (ShopSlot slot in this._lootSlots.Where(_ => _.ItemObjectInSlot == null))
                slot.DeactivateValueText();
        }

        private void Start()
        {
            for (int i = 0; i < 12; i++)
            {
                ShopSlot slot = Instantiate(this._slotPrfb, this._slotsParent.transform).GetComponent<ShopSlot>();
                if (slot == null)
                {
                    throw new ArgumentNullException(nameof(slot));
                }

                this._lootSlots.Add(slot);
            }

            this.gameObject.DeactivateGO();
        }
    }
}
