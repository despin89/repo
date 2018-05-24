namespace GD.Game.ActionsSystem
{
    using System;
    using System.Xml.Linq;
    using Extentions;
    using MoonSharp.Interpreter;

    [MoonSharpUserData]
    [Serializable]
    public class DefenceComponent : ComponentBase
    {
        #region Fields

        private int _armorReductionPercent;

        private int _magicAbsorptionValue;

        private int _armorAbsorptionValue;

        private int _takenDamagePercent;

        private int _criticalResistValue;

        private int _evasionChanceValue;

        private int _hpPercent;

        private int _shieldValue;

        #endregion

        #region Properties

        public int MagicAbsorptionValue
        {
            get { return this._magicAbsorptionValue; }
            set
            {
                this._magicAbsorptionValue = value;
            }
        }

        public int ArmorReductionPercent
        {
            get { return this._armorReductionPercent; }
            set
            {
                this._armorReductionPercent = value;
            }
        }

        public int ArmorAbsorptionValue
        {
            get { return this._armorAbsorptionValue; }
            set
            {
                this._armorAbsorptionValue = value;
            }
        }

        public int TakenDamagePercent
        {
            get { return this._takenDamagePercent; }
            set
            {
                this._takenDamagePercent = value;
            }
        }

        public int CriticalResistValue
        {
            get { return this._criticalResistValue; }
            set
            {
                this._criticalResistValue = value;
            }
        }

        public int EvasionChanceValue
        {
            get { return this._evasionChanceValue; }
            set
            {
                this._evasionChanceValue = value;
            }
        }

        public int HpPercent
        {
            get { return this._hpPercent; }
            set
            {
                this._hpPercent = value;
            }
        }

        public int ShieldValue
        {
            get { return this._shieldValue; }
            set { this._shieldValue = value; }
        }

        #endregion

        #region Constructors

        public DefenceComponent(DefenceComponentInfo info)
        {
            this._hpPercent = info.HPPercent;
            this._magicAbsorptionValue = info.MagicAbsorptionValue;
            this._armorReductionPercent = info.ArmorReductionPercent;
            this._armorAbsorptionValue = info.ArmorAbsorptionValue;
            this._takenDamagePercent = info.TakenDamagePercent;
            this._criticalResistValue = info.CriticalResistValue;
            this._evasionChanceValue = info.EvasionChanceValue;
            this._shieldValue = info.ShieldValue;
        }

        public DefenceComponent(XElement element)
        {
            this._hpPercent = element.Element("HpPercent").Value.ToInt();
            this._magicAbsorptionValue = element.Element("MagicAbsorptionValue").Value.ToInt();
            this._armorReductionPercent = element.Element("ArmorReductionPercent").Value.ToInt();
            this._armorAbsorptionValue = element.Element("ArmorAbsorptionValue").Value.ToInt();
            this._takenDamagePercent = element.Element("TakenDamagePercent").Value.ToInt();
            this._criticalResistValue = element.Element("CriticalResistValue").Value.ToInt();
            this._evasionChanceValue = element.Element("EvasionChanceValue").Value.ToInt();
            this._shieldValue = element.Element("ShieldValue").Value.ToInt();
        }

        #endregion

        public override XElement GetXDoc()
        {
            var result = base.GetXDoc();

            result.Add(new XElement("MagicAbsorptionValue", this._magicAbsorptionValue));
            result.Add(new XElement("ArmorReductionPercent", this._armorReductionPercent));
            result.Add(new XElement("ArmorAbsorptionValue", this._armorAbsorptionValue));
            result.Add(new XElement("TakenDamagePercent", this._takenDamagePercent));
            result.Add(new XElement("CriticalResistValue", this._criticalResistValue));
            result.Add(new XElement("EvasionChanceValue", this._evasionChanceValue));
            result.Add(new XElement("HpPercent", this._hpPercent));
            result.Add(new XElement("ShieldValue", this._shieldValue));

            return result;
        }
    }
}