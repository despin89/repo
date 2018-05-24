namespace GD.Game.ActionsSystem
{
    using System;
    using System.Reflection;
    using System.Xml.Linq;
    using MoonSharp.Interpreter;

    [MoonSharpUserData]
    [Serializable]
    public abstract class ComponentBase
    {

        #region Properties

        public int this[string key]
        {
            get
            {
                PropertyInfo p = this.GetType().GetProperty(key);
                if(p == null)
                    throw new ArgumentOutOfRangeException(nameof(key));

                return (int) p.GetValue(this, null);
            }
        }

        public string Tag
        {
            get { return this.GetType().Name; }
        }

        #endregion

        public virtual XElement GetXDoc()
        {
            var result = new XElement("Component");

            result.Add(new XAttribute("Tag", this.Tag));

            return result;
        }

        public static ComponentBase GetFromXEl(XElement element)
        {
            string type = element.Attribute("Tag").Value;
            switch (type)
            {
                case "DamageComponent":
                    return new DamageComponent(element);
                case "DefenceComponent":
                    return new DefenceComponent(element);
                default:
                    throw new ArgumentOutOfRangeException(type);
            }
        }
    }
}