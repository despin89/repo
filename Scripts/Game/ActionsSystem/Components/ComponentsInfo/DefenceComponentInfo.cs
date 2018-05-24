using System;

namespace GD.Game.ActionsSystem
{
    [Serializable]
    public class DefenceComponentInfo : ComponentInfoBase
    {
        public int MagicAbsorptionValue;

        public int ArmorReductionPercent;

        public int ArmorAbsorptionValue;

        public int TakenDamagePercent;

        public int CriticalResistValue;

        public int EvasionChanceValue;

        public int HPPercent;

        public int ShieldValue;

        public override ComponentBase GetInstance()
        {
            return new DefenceComponent(this);
        }
    }
}
