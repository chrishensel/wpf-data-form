using System;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Wpf.DataForm.Library.Localization
{
    /// <summary>
    /// Provides a <see cref="ILocalizationProvider"/> that uses the internal, multi-lingual table to determine the token.
    /// If the current thread's language is not supported by the table, it falls back to english (EN).
    /// </summary>
    sealed class DefaultLocalizationProvider : DynamicObject, ILocalizationProvider
    {
        #region Constants

        private const string TranslatableTokenFormat = "{0}_{1}";
        private const string TokenSeparator = "_";
        private const string FallbackLanguageCode = "EN";

        #endregion

        #region Fields

        private string _currentCultureString;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultLocalizationProvider"/> class.
        /// </summary>
        public DefaultLocalizationProvider()
        {
            DetermineLanguageToUse();
        }

        #endregion

        #region Methods

        private void DetermineLanguageToUse()
        {
            _currentCultureString = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToUpperInvariant();

            if (!StringTableContainsDesiredLanguage(_currentCultureString))
            {
                _currentCultureString = FallbackLanguageCode;
            }
        }

        private bool StringTableContainsDesiredLanguage(string languageCode)
        {
            PropertyInfo[] properties = typeof(Properties.Strings).GetProperties(BindingFlags.Static | BindingFlags.NonPublic);
            return properties.Any(p => p.Name.StartsWith(_currentCultureString + TokenSeparator, StringComparison.Ordinal));
        }

        /// <summary>
        /// Overridden to treat member names as resource names.
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = ((ILocalizationProvider)this).Localize(binder.Name);
            return true;
        }

        #endregion

        #region ILocalizationProvider Members

        string ILocalizationProvider.Localize(string token)
        {
            string str = string.Format(TranslatableTokenFormat, _currentCultureString, token);
            string value = Properties.Strings.ResourceManager.GetString(str);
            if (value != null)
            {
                return value;
            }

            return token;
        }

        #endregion
    }
}
