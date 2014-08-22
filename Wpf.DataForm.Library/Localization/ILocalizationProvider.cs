using System;

namespace Wpf.DataForm.Library.Localization
{
    /// <summary>
    /// Defines a means for a type to localize a given token.
    /// </summary>
    public interface ILocalizationProvider
    {
        /// <summary>
        /// Returns a localized string for the given token.
        /// </summary>
        /// <param name="token">The token to localize.</param>
        /// <returns>A localized string for the given token.</returns>
        string Localize(string token);
    }
}
