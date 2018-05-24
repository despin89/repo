using System;

namespace GD.Game.ActionsSystem
{
    [Serializable]
    public class DamageReturnComponentInfo : ComponentInfoBase
    {
        public bool WorkFromRangeFlag;

        public int ReflectDamagePercent;

        public int ReflectDamageValue;

        public override ComponentBase GetInstance()
        {
            return new DamageReturnComponent(this);
        }
    }
}
