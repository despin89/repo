namespace GD.UI
{
    using UnityEngine.EventSystems;

    public class Scrapper : UIBase, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            ItemObject.DraggedItemObject.Remove();
            ItemObject.StartDragSlot.GrabItemObject();
            ItemObject.DragComplete();
        }
    }
}