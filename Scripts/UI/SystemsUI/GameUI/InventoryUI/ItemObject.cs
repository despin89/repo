namespace GD.UI
{
    using System;
    using System.Collections.Generic;
    using Extentions;
    using Game.ItemsSystem;
    using PopupWindows;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class ItemObject
        : UIBase,
            IBeginDragHandler,
            IDragHandler,
            IEndDragHandler,
            IPointerEnterHandler,
            IPointerExitHandler,
            IPointerClickHandler
    {
        #region Поля

        private static Transform _parentForDrag;

        private static bool _isDragging;

        private static GameObject _itemObjectPrfb;

        #endregion

        #region Свойства

        public static ItemObject DraggedItemObject { get; private set; }

        public static SlotBase StartDragSlot { get; private set; }

        public bool IsDraggable { get; set; } = true;

        public Sprite ImageSprite
        {
            private set { this.GetComponent<Image>().sprite = value; }
            get { return this.GetComponent<Image>().sprite; }
        }

        public Item Item { get; private set; }

        private static GameObject ItemObjectPrfb
        {
            get
            {
                return _itemObjectPrfb ??
                       (_itemObjectPrfb = Resources.Load("UI/InventoryUI/ItemObjectPrfb") as GameObject);
            }
        }

        #endregion

        #region Публичные методы

        public static void DragComplete()
        {
            _isDragging = false;
            Reset();
        }

        public static ItemObject Instantiate(Item item)
        {
            ItemObject itemObject = Instantiate(ItemObjectPrfb).GetComponent<ItemObject>();

            itemObject.Item = item;
            itemObject.ImageSprite = item.ItemSprite;

            return itemObject;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            GenericTooltip.ShowTooltip(this.Item, null, this.transform.position);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            GenericTooltip.Deactivate();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!this.IsDraggable)
                return;

            StartDragSlot = this.transform.parent.gameObject.GetComponent<SlotBase>();
            DraggedItemObject = this;

            if ((StartDragSlot == null) || (this.Item == null))
            {
                throw new Exception("StartDragSlot or Item is NULL.");
            }

            _isDragging = true;

            this.transform.SetParent(_parentForDrag);
            this.transform.position = eventData.position;
            this.transform.localScale *= 1.2F;

            this.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!this.IsDraggable)
                return;

            if ((StartDragSlot == null) || !_isDragging)
            {
                throw new Exception("OnDrag invoke without OnBeginDrag.");
            }

            this.transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!this.IsDraggable)
                return;

            if (_isDragging)
            {
                this.transform.SetParent(StartDragSlot.transform);
                this.transform.position = StartDragSlot.transform.position;
                Reset();
            }

            this.transform.localScale = Vector3.one;
            this.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                pointerId = -1,
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (RaycastResult result in results)
            {
                LootSlot lootSlot = result.gameObject.GetComponent<LootSlot>();
                if (lootSlot != null)
                {
                    lootSlot.OnPointerClick(eventData);
                    return;
                }
            }
        }

        #endregion

        #region Закрытые методы

        private static void Reset()
        {
            StartDragSlot = null;
            DraggedItemObject = null;
        }

        private void Start()
        {
            if (_parentForDrag == null)
            {
                _parentForDrag = GameObject.Find("InfoPanel").GetComponent<Transform>();
            }
        }

        public void DestroyItemObject()
        {
            this.Item = null;
            this.Destroy();
        }

        #endregion
    }
}