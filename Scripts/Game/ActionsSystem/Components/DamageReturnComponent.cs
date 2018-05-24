namespace GD.Game.ActionsSystem
{
    using System;
    using MoonSharp.Interpreter;

    [MoonSharpUserData]
    [Serializable]
    public class DamageReturnComponent : ComponentBase
    {
        private bool _workFromRangeFlag;

        private int _reflectDamagePercent;

        private int _reflectDamageValue;

        public DamageReturnComponent(DamageReturnComponentInfo info)
        {
            this._workFromRangeFlag = info.WorkFromRangeFlag;
            this._reflectDamageValue = info.ReflectDamageValue;
            this._reflectDamagePercent = info.ReflectDamagePercent;
        }

        public bool WorkFromRangeFlag
        {
            get { return this._workFromRangeFlag; }
            set { this._workFromRangeFlag = value; }
        }

        public int ReflectDamagePercent
        {
            get { return this._reflectDamagePercent; }
            set
            {
                this._reflectDamagePercent = value;
            }
        }

        public int ReflectDamageValue
        {
            get { return this._reflectDamageValue; }
            set
            {
                this._reflectDamageValue = value;
            }
        }
    }
}
