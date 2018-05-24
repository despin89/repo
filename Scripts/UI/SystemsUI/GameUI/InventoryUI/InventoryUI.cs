namespace GD.UI
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.MessengerSystem;
    using Game;
    using Game.ItemsSystem;
    using Game.PlayerSystem;
    using PopupWindows;
    using UnityEngine;

    public class InventoryUI : MonoBehaviour, IWorld
    {
        private List<InventorySlot> Slots { get; set; }

        [SerializeField]
        private GameObject _slotInventoryPrefab;

        public void Init()
        {
            Messenger.AddListener(M.REFRESH_INV, this.InstantiateInventoryItems);
            Messenger<Item>.AddListener(M.ADD_ITEM_TO_INV, this.AddItemToInv);

            this.Slots = new List<InventorySlot>();

            for (int i = 0; i < PartyInventory.SLOT_AMOUNT; i++)
            {
                GameObject slotObj = Instantiate(this._slotInventoryPrefab);
                slotObj.transform.SetParent(this.gameObject.transform);

                InventorySlot slotInv = slotObj.GetComponent<InventorySlot>();
                this.Slots.Add(slotInv);
            }

            this.InstantiateInventoryItems();
        }

        //TODO: Сделать сравнение для пердметов, мб вернуть ID. Нужно проверять, такой же предмет в слоте или нет
        private void InstantiateInventoryItems()
        {
            foreach (InventorySlot slot in this.Slots)
                slot.Clear();

            foreach (Item item in this.CurrentWorld.Inventory.PartyItemsBag)
            {
                InventorySlot slot = this.Slots.FirstOrDefault(e => e.ItemObjectInSlot == null);
                if (slot == null)
                {
                    ModalDialogWindow.Instance.InvokeModalMessage("Here is no empty slot!");
                    return;
                }

                slot.InstantiateItemObject(ItemObject.Instantiate(item));
            }
        }

        //TODO: Сделать сравнение для пердметов, мб вернуть ID. Нужно проверять, такой же предмет в слоте или нет
        private void AddItemToInv(Item item)
        {
            //Если есть место для предмета
            foreach (InventorySlot slot in this.Slots.Where(slot => slot.ItemObjectInSlot == null))
            {
                slot.DropItemObject(ItemObject.Instantiate(item));
                return;
            }

            ModalDialogWindow.Instance.InvokeModalMessage("Here is no empty slot!");
        }

        private void OnDestroy()
        {
            Messenger.RemoveListener(M.REFRESH_INV, this.InstantiateInventoryItems);
            Messenger<Item>.RemoveListener(M.ADD_ITEM_TO_INV, this.AddItemToInv);
        }

        public World CurrentWorld
        {
            get { return GameManager.CurrentWorld; }
        }
    }
}