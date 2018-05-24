namespace GD.UI
{
    using Game.ItemsSystem;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public abstract class SlotBase
        : UIBase, IDropHandler
    {
        #region Properties

        public ItemObject ItemObjectInSlot { get; set; }

        #endregion

        #region Закрытые методы

        private bool TryChangeItems(ItemObject itemToChange)
        {
            return this.TryInsertItem(itemToChange.Item) &&
                   ItemObject.StartDragSlot.TryInsertItem(this.ItemObjectInSlot.Item);
        }

        #endregion

        #region Публичные методы

        public abstract bool TryInsertItem(Item item);

        public abstract void DropItemObject(ItemObject droppedItem);

        public virtual void InstantiateItemObject(ItemObject droppedItem)
        {
            droppedItem.transform.SetParent(this.transform);
            droppedItem.transform.position = this.transform.transform.position;

            droppedItem.GetComponent<RectTransform>().sizeDelta = new Vector2(70, 70);

            this.ItemObjectInSlot = droppedItem;
        }

        public abstract void GrabItemObject();

        public void OnDrop(PointerEventData eventData)
        {
            if(ItemObject.StartDragSlot == null)
                return;

            if (!ItemObject.StartDragSlot.Equals(this))
            {
                if (this.ItemObjectInSlot == null)
                {
                    if (this.TryInsertItem(ItemObject.DraggedItemObject.Item))
                    {
                        this.DropItemObject(ItemObject.DraggedItemObject);
                        ItemObject.StartDragSlot.GrabItemObject();

                        ItemObject.DragComplete();
                    }
                }
                else
                {
                    if (this.TryChangeItems(ItemObject.DraggedItemObject))
                    {
                        ItemObject tempItem = this.ItemObjectInSlot;

                        this.GrabItemObject();
                        this.DropItemObject(ItemObject.DraggedItemObject);
                        ItemObject.StartDragSlot.DropItemObject(tempItem);

                        ItemObject.DragComplete();
                    }
                }
            }
        }

        public virtual void Clear()
        {
            this.ItemObjectInSlot?.DestroyItemObject();
            this.ItemObjectInSlot = null;
        }

        #endregion
    }
}