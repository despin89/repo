namespace GD.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.MessengerSystem;
    using Extentions;
    using Game;
    using Game.ItemsSystem;
    using UnityEngine;
    using UnityEngine.UI;

    public class LootUI : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Button _buttonOK;

        [SerializeField]
        private GameObject _slotPrfb;

        [SerializeField]
        private GameObject _slotsParent;

        [SerializeField]
        private GameObject _notEnoughSpaceMark;

        private List<LootSlot> _lootSlots = new List<LootSlot>();

        #endregion

        #region Properties

        private int LootItemsCount
        {
            get { return this._lootSlots.Where(_ => _.ItemObjectInSlot != null).Where(_ => !_.IsReadyToDrop).ToArray().Length; }
        }

        #endregion

        public void InvokeLootUI(List<Item> items)
        {
            this.gameObject.ActivateGO();

            foreach (Item item in items)
            {
                LootSlot slot = this._lootSlots.FirstOrDefault(_ => _.ItemObjectInSlot == null);
                if (slot == null)
                {
                    throw new Exception("Loot overflow! Maximum size reached!");
                }

                slot.InstantiateItemObject(ItemObject.Instantiate(item));
            }

            this.CheckTake();
        }

        public void CheckTake()
        {
            if (!this.CheckSpaceToTake())
            {
                this._buttonOK.DeactivateSelectable();
                this._notEnoughSpaceMark.ActivateGO();
            }
            else
            {
                this._buttonOK.ActivateSelectable();
                this._notEnoughSpaceMark.DeactivateGO();
            }
        }

        private bool CheckSpaceToTake()
        {
            return this.LootItemsCount <= GameManager.CurrentWorld.Inventory.EmptySlotsCount;
        }

        private void TakeItems()
        {
            foreach (LootSlot slot in this._lootSlots.Where(_ => _.ItemObjectInSlot != null).Where(_ => !_.IsReadyToDrop))
            {
                GameManager.CurrentWorld.Inventory.AddItemToBag(slot.ItemObjectInSlot.Item);
                slot.Clear();
                slot.GrabItemObject();
            }

            Messenger.Broadcast(M.REFRESH_INV);

            this.gameObject.DeactivateGO();
        }

        private void Start()
        {
            Messenger.AddListener(M.INV_CHANGED, this.CheckTake);

            this._buttonOK.AddAction(this.TakeItems);

            for (int i = 0; i < 12; i++)
            {
                LootSlot slot = Instantiate(this._slotPrfb, this._slotsParent.transform).GetComponent<LootSlot>();
                if (slot == null)
                {
                    throw new ArgumentNullException(nameof(slot));
                }

                slot.Loot = this;

                this._lootSlots.Add(slot);
            }

            this.gameObject.DeactivateGO();
            this._notEnoughSpaceMark.DeactivateGO();
        }

        private void OnDestroy()
        {
            Messenger.RemoveListener(M.INV_CHANGED, this.CheckTake);
        }
    }
}