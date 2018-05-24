namespace GD.Game.ItemsSystem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public static class ItemsDatabase
    {
        #region Constants

        private static readonly List<ItemInfo> _items = new List<ItemInfo>();

        private static bool isInited;

        #endregion

        public static void Init()
        {
            if (!isInited)
            {
                ItemInfo[] items = Resources.LoadAll<ItemInfo>("Items/ItemsData");
                if (items != null)
                {
                    _items.AddRange(items);
                }
                else
                {
                    Debug.LogError("No items at \"Items/ItemsData\"");
                }

                isInited = true;
            }
        }

        public static Item GetItemByTag(string tag)
        {
            ItemInfo itemInfo = _items.FirstOrDefault(e => e.Tag == tag);

            if (itemInfo == null)
            {
                Debug.Log("Database in not contain item with tag: " + tag);
                throw new NullReferenceException("Database in not contain item with tag: " + tag);
            }

            return new Item(itemInfo);
        }
    }
}