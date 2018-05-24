namespace GD.Core.Localization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public partial class Localization
    {
        #region Constants

        private static Dictionary<string, string> _localizationStrings;

        private static Dictionary<string, string> _localizationStringsByKey;

        private static bool _isInstantiated;

        #endregion

        public static void Init()
        {
            if (!_isInstantiated)
            {
                LocalizationDatabase database = Resources.Load<LocalizationDatabase>("Localization/LocalizationDatabase");
                if (database != null)
                {
                    _localizationStrings = database.CachedStrings.ToDictionary(k => k.Key, v => v.Value.GetThisText());
                    _localizationStringsByKey =
                        database.DefaultLocalizationStrings.Where(_ => !string.IsNullOrEmpty(_.Key))
                            .ToDictionary(_ => _.Key, _ => _.GetThisText());
                }
                else
                {
                    Debug.LogError("No LocalizationDatabase file at \"Localization\"");
                    throw new Exception("No LocalizationDatabase file at \"Localization\"");
                }

                _isInstantiated = true;
            }
        }

        public static string GetStringById(string id)
        {
            if (_localizationStrings != null)
            {
                return _localizationStrings.ContainsKey(id) ? _localizationStrings[id] : null;
            }

            return null;
        }

        public static string GetStringByKey(string key)
        {
            if (_localizationStringsByKey != null)
            {
                return _localizationStringsByKey.ContainsKey(key) ? _localizationStringsByKey[key] : null;
            }

            return null;
        }
    }
}