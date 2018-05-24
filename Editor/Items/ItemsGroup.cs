namespace GD.EditorExtentions
{
    using System.Collections.Generic;
    using Game.ItemsSystem;
    using UnityEngine;

    public interface INamedGroup
    {
        #region Properties

        string Name { get; set; }

        #endregion
    }

    public class ItemsGroup : ScriptableObject, INamedGroup
    {
        #region Fields

        [HideInInspector]
        public bool IsObjectsShown;

        [HideInInspector]
        public List<ItemInfo> Items = new List<ItemInfo>();

        [HideInInspector]
        [SerializeField]
        private string _name;

        #endregion

        #region Properties

        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }

        #endregion
    }
}