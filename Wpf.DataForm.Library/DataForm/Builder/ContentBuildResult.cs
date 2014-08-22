using System.Windows;

namespace Wpf.DataForm.Library.DataForm.Builder
{
    /// <summary>
    /// Represents the result of building a content for a given element.
    /// </summary>
    sealed class ContentBuildResult
    {
        #region Properties

        /// <summary>
        /// Gets/sets the display name to use for the built content.
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// Gets/sets whether or not the label for this element shall be shown.
        /// The default value is true.
        /// </summary>
        public bool ShowLabel { get; set; }
        /// <summary>
        /// Gets/sets the content element that was built.
        /// </summary>
        public UIElement Element { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentBuildResult"/> class.
        /// </summary>
        public ContentBuildResult()
        {
            ShowLabel = true;
        }

        #endregion
    }
}
