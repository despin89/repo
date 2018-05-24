namespace GD.Game.ActionsSystem
{
    using System;

    [Serializable]
    public class DamageComponentInfo : ComponentInfoBase
    {
        public int MaxDamageValue;

        public int MinDamageValue;

        public int TotalDamagePercent;

        public int ArmorPenetrationPercent;

        public int CriticalChanceValue;

        public int HitChanceValue;

        public override ComponentBase GetInstance()
        {
            return new DamageComponent(this);
        }
    }
}
