namespace GD.Game.ActionsSystem
{
    using System;
    using Combat;
    using SkillsSystem;
    using StatusesSystem;

    public sealed class CombatEventArgs : EventArgs
    {
        #region Properties

        public ICombatant Attacker { get; set; }

        public ICombatant Victim { get; set; }

        public CommonSkill Skill { get; set; }

        public StatusInfo Status { get; set; }

        public Damage InflictedDamage { get; set; }

        public int? HealValue { get; set; }

        #endregion
    }
}