namespace GD.Game.ActionsSystem.ProcessSystems
{
    using System.Collections.Generic;
    using System.Linq;
    using Extentions;

    public class LifeStealSystem : ProcessSystemBase
    {
        public override void Process(CombatEventArgs args)
        {
            if ((args.Attacker != null) && (args.Victim != null))
            {
                List<LifeStealComponent> lifeStealList = args.Attacker.GetComponents<LifeStealComponent>();

                if (lifeStealList != null)
                {
                    int lifeStealValue =
                        (int)
                        (lifeStealList.Sum(e => e["LifeStealValue"]) +
                         args.InflictedDamage.Value * lifeStealList.Max(e => e["LifeStealPercent"]).ToPercent());

                    args.Attacker.Heal(lifeStealValue, null);
                }
            }
        }
    }
}