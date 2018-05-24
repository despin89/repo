namespace GD.Game.ActionsSystem
{
    using System;
    using MoonSharp.Interpreter;

    [Serializable]
    [MoonSharpUserData]
    public class LifeStealComponent : ComponentBase
    {
        #region Fields

        private int _lifeStealPercent;

        private int _lifeStealValue;

        #endregion

        #region Properties

        public int LifeStealPercent
        {
            get { return this._lifeStealPercent; }
            set
            {
                this._lifeStealPercent = value;
            }
        }

        public int LifeStealValue
        {
            get { return this._lifeStealValue; }
            set
            {
                this._lifeStealValue = value;
            }
        }

        #endregion

        #region Constructors

        public LifeStealComponent(LifeStealComponentInfo info)
        {
            this._lifeStealValue = info.LifeStealValue;
            this._lifeStealPercent = info.LifeStealPercent;
        }

        #endregion
    }
}