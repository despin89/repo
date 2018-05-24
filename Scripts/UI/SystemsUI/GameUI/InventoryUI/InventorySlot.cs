namespace GD.UI
{
    using Core.MessengerSystem;
    using Game.ItemsSystem;

    public class InventorySlot : SlotBase
    {
        #region Публичные методы

        public override bool TryInsertItem(Item item)
        {
            return true;
        }

        public override void DropItemObject(ItemObject droppedItem)
        {
            this.InstantiateItemObject(droppedItem);
            Messenger<Item>.Broadcast(M.DROP_ITEM_TO_INV, this.ItemObjectInSlot.Item);
            Messenger.Broadcast(M.INV_CHANGED);
        }

        public override void GrabItemObject()
        {
            Messenger<Item>.Broadcast(M.GRAB_ITEM_FROM_INV, this.ItemObjectInSlot.Item);
            Messenger.Broadcast(M.INV_CHANGED);
            this.ItemObjectInSlot = null;
        }

        #endregion
    }
}