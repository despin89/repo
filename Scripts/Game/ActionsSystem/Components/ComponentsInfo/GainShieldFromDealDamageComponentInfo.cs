namespace GD.Game.ActionsSystem
{
    public class GainShieldFromDealDamageComponentInfo : ComponentInfoBase
    {

        public int DamageToShieldPercent;

        public override ComponentBase GetInstance()
        {
            return new GainShieldFromDealDamageComponent(this);
        }
    }
}
