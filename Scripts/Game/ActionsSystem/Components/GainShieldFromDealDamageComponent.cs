namespace GD.Game.ActionsSystem
{
    public class GainShieldFromDealDamageComponent : ComponentBase
    {
        private int _damageToShieldPercent;

        public GainShieldFromDealDamageComponent(GainShieldFromDealDamageComponentInfo info)
        {
            this._damageToShieldPercent = info.DamageToShieldPercent;
        }

        public int DamageToShieldPercent
        {
            get { return this._damageToShieldPercent; }
            set { this._damageToShieldPercent = value; }
        }
    }
}
