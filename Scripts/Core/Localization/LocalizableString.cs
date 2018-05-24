namespace GD.Core.Localization
{
    using System;
    using MoonSharp.Interpreter;
    using UnityEngine;

    [MoonSharpUserData]
    [Serializable]
    public class LocalizableString
    {
        #region Fields

        [TextArea]
        [SerializeField]
        private string _text;

        [HideInInspector]
        [SerializeField]
        private string _id;

        [HideInInspector]
        [SerializeField]
        private string _alias;

        [HideInInspector]
        [SerializeField]
        private string _key;

        #endregion

        #region Properties

        public string Text
        {
            get
            {
                string storedText = Localization.GetStringById(this._id);
                return !string.IsNullOrEmpty(storedText) ? storedText : this._text;
            }
            set { this._text = value; }
        }

        public string Alias
        {
            get { return this._alias; }
        }

        public string Id
        {
            get { return this._id; }
        }

        public string Key
        {
            get { return this._key; }
        }

        #endregion

        #region Constructors

        public LocalizableString()
        {
            this._id = Guid.NewGuid().ToString();
        }

        public LocalizableString(string s, string alias = null, string key = null)
        {
            this._text = s;
            this._id = Guid.NewGuid().ToString();
            this._alias = alias;
            this._key = key;
        }

        #endregion

        public string GetThisText()
        {
            return this._text;
        }
    }
}