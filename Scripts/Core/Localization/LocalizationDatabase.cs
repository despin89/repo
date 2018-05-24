namespace GD.Core.Localization
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class LocalizationDatabase : ScriptableObject
    {
        #region Fields

        private Dictionary<string, LocalizableString> _cachedStrings;

        [SerializeField]
        [HideInInspector]
        private List<LocalizableString> _defaultLocalizationStrings = new List<LocalizableString>();

        #endregion

        #region Properties

        public Dictionary<string, LocalizableString> CachedStrings
        {
            get { return this._cachedStrings ?? (this._cachedStrings = this.GetLocalizationStringsDictionary()); }
        }

        public List<LocalizableString> DefaultLocalizationStrings
        {
            get { return this._defaultLocalizationStrings; }
        }

        #endregion

        private Dictionary<string, LocalizableString> GetLocalizationStringsDictionary()
        {
            return this._defaultLocalizationStrings.ToDictionary(k => k.Id, v => v);
        }

        public string TryAddString(string s, string alias)
        {
            string error = null;

            if (this.CachedStrings.Values.Any(e => e.Alias == alias))
            {
                error = "Alias: \"" + alias + "\" already set.";
                return error;
            }

            if (this.CachedStrings.Values.All(e => e.Text != s))
            {
                this.AddLocalizableString(s, alias);
            }
            else
            {
                error = "String: \"" + s + "\" already stored.";
            }
            return error;
        }

        public string TryAddKeyString(string s, string key)
        {
            string error = null;

            if (this.CachedStrings.Values.Any(e => e.Key == key))
            {
                error = "Key: \"" + key + "\" already set.";
                return error;
            }

            if (this.CachedStrings.Values.All(e => e.Text != s))
            {
                this.AddLocalizableString(s, null, key);
            }
            else
            {
                error = "String: \"" + s + "\" already stored.";
            }
            return error;
        }

        public void TryAddLocalizableString(LocalizableString s)
        {
            if (this.CachedStrings.ContainsKey(s.Id)) return;

            this.AddLocalizableString(s);
        }

        public string TryRemoveStringById(string id)
        {
            string error = null;
            if (this.CachedStrings.ContainsKey(id))
            {
                this.RemoveStringById(id);
            }
            else
            {
                error = "This string is not in database.";
            }
            return error;
        }

        public void RemoveStringById(string id)
        {
            this._defaultLocalizationStrings.RemoveAll(e => e.Id == id);
            if (this._cachedStrings != null)
            {
                this._cachedStrings.Remove(id);
            }
        }

        private void AddLocalizableString(string s, string alias = null, string key = null)
        {
            LocalizableString stringToAdd = new LocalizableString(s, alias, key);

            this._defaultLocalizationStrings.Add(stringToAdd);
            if (this._cachedStrings != null)
            {
                this._cachedStrings.Add(stringToAdd.Id, stringToAdd);
            }
        }

        private void AddLocalizableString(LocalizableString s)
        {
            this._defaultLocalizationStrings.Add(s);
            if (this._cachedStrings != null)
            {
                this._cachedStrings.Add(s.Id, s);
            }
        }
    }
}