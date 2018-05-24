namespace GD.Game.ActionsSystem
{
    using System;
    using System.Collections.Generic;
    using Combat;
    using Extentions;
    using MoonSharp.Interpreter;

    [MoonSharpUserData]
    [Serializable]
    public class AttackActionComponent : ComponentBase
    {
        #region Fields

        private DamageType _damageType;

        private int _areaDamageMult;

        private int _skillDamageMult;

        private int _critMult;

        private List<StatMultiplier> _statMultipliers;

        public AttackActionComponent(AttackActionComponentInfo info)
        {
            this._damageType = info.DamageType;
            this._areaDamageMult = info.AreaDamageMult;
            this._skillDamageMult = info.SkillDamageMult;
            this._critMult = info.CritMult;
            this._statMultipliers = info.StatMultipliers.DeepCopy();
        }

        #endregion

        #region Properties

        public DamageType DamageType
        {
            get { return this._damageType; }
        }

        public int CritMult
        {
            get { return this._critMult; }
            set
            {
                this._critMult = value;
            }
        }

        public int SkillDamageMult
        {
            get { return this._skillDamageMult; }
            set
            {
                this._skillDamageMult = value;
            }
        }

        public int AreaDamageMult
        {
            get { return this._areaDamageMult; }
            set
            {
                this._areaDamageMult = value;
            }
        }

        public List<StatMultiplier> StatMultipliers
        {
            get { return this._statMultipliers; }
        }

        #endregion
    }
}