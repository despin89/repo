namespace GD.Game.ActionsSystem.ProcessSystems
{
    using System.Collections.Generic;
    using System.Linq;

    public class ApplyStatusOnCritSystem : ProcessSystemBase
    {
        public override void Process(CombatEventArgs args)
        {
            List<ApplyStatusActionComponent> applyStatusActions =
                args.Attacker.GetComponents<ApplyStatusActionComponent>();
            if (applyStatusActions != null)
            {
                List<ApplyStatusActionComponent> onCritActions =
                    applyStatusActions.Where(e => e.ApplyType == StatusType.ApplyOnCrit).ToList();
                foreach (ApplyStatusActionComponent onCritAction in onCritActions)
                {
                    if (onCritAction.ApplyTarget == StatusTarget.Self)
                    {
                        ApplyStatus(onCritAction, args.Attacker, args.Attacker);
                    }
                    else
                    {
                        ApplyStatus(onCritAction, args.Victim, args.Attacker);
                    }
                }
            }
        }
    }
}