namespace GD.Game.ItemsSystem
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Items/ItemSubtype", fileName = "ItemSubtype")]
    public class ItemSubtype : AssetEnumBase
    {
        public override int GetHashCode()
        {
            if (!string.IsNullOrEmpty(this.Name))
            {
                return this.Name.GetHashCode();
            }
            else
            {
                return this.name.GetHashCode();
            }
            
        }
    }
}