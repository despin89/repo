namespace GD.Game.ActionsSystem
{
    using System;
    using System.Xml;
    using System.Xml.Linq;
    using Extentions;
    using MoonSharp.Interpreter;

    [MoonSharpUserData]
    [Serializable]
    public class DamageComponent : ComponentBase
    {
        #region Fields

        private int _maxDamageValue;

        private int _minDamageValue;

        private int _totalDamagePercent;

        private int _armorPenetrationPercent;

        private int _criticalChanceValue;

        private int _hitChanceValue;

        #endregion

        #region Properties

        public int MaxDamageValue
        {
            get { return this._maxDamageValue; }
            set
            {
                this._maxDamageValue = value;
            }
        }

        public int MinDamageValue
        {
            get { return this._minDamageValue; }
            set
            {
                this._minDamageValue = value;
            }
        }

        public int TotalDamagePercent
        {
            get { return this._totalDamagePercent; }
            set
            {
                this._totalDamagePercent = value;
            }
        }

        public int ArmorPenetrationPercent
        {
            get { return this._armorPenetrationPercent; }
            set
            {
                this._armorPenetrationPercent = value;
            }
        }

        public int CriticalChanceValue
        {
            get { return this._criticalChanceValue; }
            set
            {
                this._criticalChanceValue = value;
            }
        }

        public int HitChanceValue
        {
            get { return this._hitChanceValue; }
            set
            {
                this._hitChanceValue = value;
            }
        }

        #endregion

        #region Constructors

        public DamageComponent(DamageComponentInfo info)
        {
            this._maxDamageValue = info.MaxDamageValue;
            this._minDamageValue = info.MinDamageValue;
            this._totalDamagePercent = info.TotalDamagePercent;
            this._armorPenetrationPercent = info.ArmorPenetrationPercent;
            this._criticalChanceValue = info.CriticalChanceValue;
            this._hitChanceValue = info.HitChanceValue;
        }

        public DamageComponent(XElement info)
        {
            this._maxDamageValue = info.Element("MaxDamageValue").Value.ToInt();
            this._minDamageValue = info.Element("MinDamageValue").Value.ToInt();
            this._armorPenetrationPercent = info.Element("ArmorPenetrationPercent").Value.ToInt();
            this._criticalChanceValue = info.Element("CriticalChanceValue").Value.ToInt();
            this._hitChanceValue = info.Element("HitChanceValue").Value.ToInt();
            this._totalDamagePercent = info.Element("TotalDamagePercent").Value.ToInt();
        }

        #endregion

        public override XElement GetXDoc()
        {
            var result =  base.GetXDoc();

            result.Add(new XElement("ArmorPenetrationPercent", this._armorPenetrationPercent));
            result.Add(new XElement("CriticalChanceValue", this._criticalChanceValue));
            result.Add(new XElement("HitChanceValue", this._hitChanceValue));
            result.Add(new XElement("MaxDamageValue", this._maxDamageValue));
            result.Add(new XElement("MinDamageValue", this._minDamageValue));
            result.Add(new XElement("TotalDamagePercent", this._totalDamagePercent));

            return result;
        }
    }
}