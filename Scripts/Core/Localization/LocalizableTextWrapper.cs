namespace GD.Core.Localization
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    [Serializable]
    public class LocalizableTextWrapper : MonoBehaviour, ILocalizable
    {
        #region Fields

        public LocalizableString LocalizableString;

        #endregion

        public Text Text
        {
            get
            {
                return this.GetComponent<Text>();
            }
        }

        #region Implementations

        IEnumerable<LocalizableString> ILocalizable.GetLocalizableStrings()
        {
            return new[] {this.LocalizableString};
        }

        #endregion

        public void Start()
        {
            this.Text.text = this.LocalizableString.Text;
        }

        public string LocalizationGroup
        {
            get { return "TextWrapper"; }
        }
    }
}