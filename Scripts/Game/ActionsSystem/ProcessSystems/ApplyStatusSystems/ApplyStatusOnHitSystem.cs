namespace GD.Game.ActionsSystem.ProcessSystems
{
    using System.Collections.Generic;
    using System.Linq;

    public class ApplyStatusOnHitSystem : ProcessSystemBase
    {
        public override void Process(CombatEventArgs args)
        {
            List<ApplyStatusActionComponent> applyStatusActions =
                args.Attacker?.GetComponents<ApplyStatusActionComponent>();

            if (applyStatusActions != null)
            {
                List<ApplyStatusActionComponent> onHitActions =
                    applyStatusActions.Where(e => e.ApplyType == StatusType.ApplyOnHit).ToList();

                foreach (ApplyStatusActionComponent applyStatusAction in onHitActions)
                {
                    if (applyStatusAction.ApplyTarget == StatusTarget.Self)
                    {
                        ApplyStatus(applyStatusAction, args.Attacker, args.Attacker);
                    }
                    else
                    {
                        ApplyStatus(applyStatusAction, args.Victim, args.Attacker);
                    }
                }
            }
        }
    }
}