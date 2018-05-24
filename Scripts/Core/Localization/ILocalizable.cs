namespace GD.Core.Localization
{
    using System.Collections.Generic;

    public interface ILocalizable
    {
        IEnumerable<LocalizableString> GetLocalizableStrings();

        string LocalizationGroup { get; }
    }
}