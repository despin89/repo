namespace GD.UI
{
    using Extentions;
    using Game.ItemsSystem;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class LootSlot : SlotBase, IPointerClickHandler
    {
        #region Fields

        [SerializeField]
        private GameObject _dropMark;

        #endregion

        #region Properties

        public LootUI Loot { get; set; }

        public bool IsReadyToDrop { get; private set; }

        #endregion

        #region Implementations

        /// <summary>
        ///     <para></para>
        /// </summary>
        /// <param name="eventData">Current event data.</param>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!this.IsReadyToDrop)
            {
                this._dropMark.SetLastAndActivate();
                this.IsReadyToDrop = true;
            }
            else
            {
                this._dropMark.DeactivateGO();
                this.IsReadyToDrop = false;
            }

            this.Loot.CheckTake();
        }

        #endregion

        public override bool TryInsertItem(Item item)
        {
            return false;
        }

        public override void DropItemObject(ItemObject droppedItem)
        {
        }

        public override void GrabItemObject()
        {
            this.ItemObjectInSlot = null;

            this.Loot.CheckTake();
        }

        public void Start()
        {
            this._dropMark.DeactivateGO();
        }
    }
}