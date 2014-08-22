using System;
using System.Windows.Markup;

namespace Wpf.DataForm.Library.Localization
{
    /// <summary>
    /// Represents a markup extension to provide localization services to XAML.
    /// </summary>
    public class LocExtension : MarkupExtension
    {
        #region Fields

        private string _token;

        #endregion

        #region Constructors

        private LocExtension()
            : base()
        {

        }


        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultLocalizationProvider"/> class.
        /// </summary>
        /// <param name="token">The token to translate.</param>
        public LocExtension(string token)
            : this()
        {
            if (token == null)
            {
                throw new ArgumentNullException("token");
            }

            _token = token;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Provides the translation from the token with which this instance was constructed.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return LocalizationManager.LocalizationProvider.Localize(_token);
        }

        #endregion
    }
}
