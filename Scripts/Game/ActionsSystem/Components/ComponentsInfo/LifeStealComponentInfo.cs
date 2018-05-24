using System;

namespace GD.Game.ActionsSystem
{
    [Serializable]
    public class LifeStealComponentInfo : ComponentInfoBase
    {
        public int LifeStealPercent;

        public int LifeStealValue;

        public override ComponentBase GetInstance()
        {
            return new LifeStealComponent(this);
        }
    }
}