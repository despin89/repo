namespace GD.Extentions
{
    using System;
    using System.Xml.Linq;

    [Serializable]
    public class Pair<TKey, TValue>
    {
        #region Fields

        public TKey Key;

        public TValue Value;

        #endregion
    }

    [Serializable]
    public class StringIntPair : Pair<string, int>
    {
        public XElement GetXDoc()
        {
            var result = new XElement("Pair");
            result.Add(new XAttribute("Key", this.Key));
            result.Add(new XAttribute("Value", this.Value));

            return result;
        }
    }
}