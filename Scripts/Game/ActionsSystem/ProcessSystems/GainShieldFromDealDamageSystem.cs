using System.Collections.Generic;
using System.Linq;

namespace GD.Game.ActionsSystem.ProcessSystems
{
    using Extentions;

    public class GainShieldFromDealDamageSystem : ProcessSystemBase
    {
        public override void Process(CombatEventArgs args)
        {
            if ((args.Attacker != null) && (args.Victim != null))
            {
                List<GainShieldFromDealDamageComponent> shieldComponents = args.Attacker.GetComponents<GainShieldFromDealDamageComponent>();

                if (shieldComponents != null)
                {
                    int shieldValue =
                        (int)(args.InflictedDamage.Value * shieldComponents.Sum(e => e.DamageToShieldPercent).ToPercent());

                    //TODO: Сейчас эта система не работает из за того, что щит решено сделать менее глудоким по механике
                }
            }
        }
    }
}
