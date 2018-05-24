namespace GD.Game.ActionsSystem
{
    using Combat;
    using StatusesSystem;

    public abstract class ProcessSystemBase
    {
        public virtual void Process(CombatEventArgs args){}

        public virtual void Process(ICombatant sender){}

        protected static void ApplyStatus(ApplyStatusActionComponent applyStatusAction, ICombatant victim, ICombatant imposer)
        {
            if (applyStatusAction != null)
            {
                foreach (Status status in applyStatusAction.Statuses)
                {
                    victim.TryApplyStatus(status.Clone(), imposer);
                }
            }
        }
    }
}