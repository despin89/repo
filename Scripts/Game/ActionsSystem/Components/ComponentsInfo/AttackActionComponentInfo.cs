namespace GD.Game.ActionsSystem
{
    using System;
    using System.Collections.Generic;
    using Combat;

    [Serializable]
    public class AttackActionComponentInfo : ComponentInfoBase
    {
        #region Fields

        public DamageType DamageType = DamageType.Physical;

        public int AreaDamageMult = 100;

        public int SkillDamageMult = 100;

        public int CritMult = 150;

        public List<StatMultiplier> StatMultipliers;

        public override ComponentBase GetInstance()
        {
            return new AttackActionComponent(this);
        }

        #endregion
    }

    [Serializable]
    public class StatMultiplier
    {
        public Stat Stat;

        public int Multiplier;
    }
}