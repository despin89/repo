namespace GD.Game.ActionsSystem.ProcessSystems
{
    using System.Collections.Generic;
    using System.Linq;
    using Combat;
    using Extentions;

    public class DamageReturnSystem : ProcessSystemBase
    {
        public override void Process(CombatEventArgs args)
        {
            if ((args.Attacker != null) && (args.Victim != null))
            {
                List<DamageReturnComponent> damageReturnList = args.Victim.GetComponents<DamageReturnComponent>();

                if (damageReturnList != null)
                {
                    if (damageReturnList.Any(e => e.WorkFromRangeFlag))
                    {
                        int damageValue = (int) (damageReturnList.Sum(e => e["ReflectDamageValue"]) +
                                                 args.InflictedDamage.Value *
                                                 damageReturnList.Max(e => e["ReflectDamagePercent"]).ToPercent());

                        args.Attacker.Hurt(Damage.New(damageValue), null);
                    }
                }
            }
        }
    }
}