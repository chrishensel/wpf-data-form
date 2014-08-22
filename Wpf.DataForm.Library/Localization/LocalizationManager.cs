
namespace Wpf.DataForm.Library.Localization
{
    /// <summary>
    /// Provides localization services for the DataForm assembly.
    /// </summary>
    public static class LocalizationManager
    {
        #region Fields

        private static ILocalizationProvider _localizationProvider;

        #endregion

        #region Properties

        /// <summary>
        /// Gets/sets the localization provider that is used for providing localization.
        /// See documentation for further information.
        /// </summary>
        /// <remarks>Setting a new instance is only possible if a value is actually passed. Passing 'null' to the setter will keep the current implementation.</remarks>
        public static ILocalizationProvider LocalizationProvider
        {
            get { return _localizationProvider; }
            set
            {
                if (value != null)
                {
                    _localizationProvider = value;
                }
            }
        }

        #endregion

        #region Constructors

        static LocalizationManager()
        {
            LocalizationProvider = new DefaultLocalizationProvider();
        }

        #endregion
    }
}
