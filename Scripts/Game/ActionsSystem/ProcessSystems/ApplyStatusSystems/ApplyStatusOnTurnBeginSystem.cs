namespace GD.Game.ActionsSystem.ProcessSystems
{
    using System.Collections.Generic;
    using System.Linq;
    using Combat;

    public class ApplyStatusOnTurnBeginSystem : ProcessSystemBase
    {
        public override void Process(ICombatant sender)
        {
            List<ApplyStatusActionComponent> applyStatusActions =
                sender.GetComponents<ApplyStatusActionComponent>();

            if (applyStatusActions != null)
            {
                List<ApplyStatusActionComponent> onTurnBeginActions =
                    applyStatusActions.Where(e => e.ApplyType == StatusType.ApplyOnTurnBegin).ToList();

                foreach (ApplyStatusActionComponent applyStatusAction in onTurnBeginActions)
                {
                    if (applyStatusAction.ApplyTarget == StatusTarget.Self)
                    {
                        ApplyStatus(applyStatusAction, sender, sender);
                    }
                }
            }
        }
    }
}