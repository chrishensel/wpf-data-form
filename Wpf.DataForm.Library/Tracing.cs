using System.Diagnostics;
using System.Reflection;

namespace Wpf.DataForm.Library
{
    /// <summary>
    /// Facilitates the .Net tracing mechanism to write traces.
    /// </summary>
    public static class Tracing
    {
        #region Fields

        private static readonly TraceSource _source;

        #endregion

        #region Properties

        /// <summary>
        /// Gets/sets whether or not traces will be written.
        /// See documentation for further information.
        /// </summary>
        /// <remarks>The value of this property is automatically set whether or not a debugger is attached at the moment of the call of the static constructor.
        /// You can manually enable or disable tracing at any time.</remarks>
        public static bool IsEnabled { get; set; }

        #endregion

        #region Constructors

        static Tracing()
        {
            IsEnabled = Debugger.IsAttached;
            _source = new TraceSource(Assembly.GetExecutingAssembly().GetName().Name, SourceLevels.All);
        }

        #endregion

        #region Methods

        internal static void WriteVerbose(string format, params object[] args)
        {
            Write(TraceEventType.Verbose, format, args);
        }

        internal static void WriteInfo(string format, params object[] args)
        {
            Write(TraceEventType.Information, format, args);
        }

        internal static void WriteError(string format, params object[] args)
        {
            Write(TraceEventType.Error, format, args);
        }

        private static void Write(TraceEventType type, string format, params object[] args)
        {
            if (IsEnabled)
            {
                _source.TraceEvent(type, 0, format, args);
            }
        }

        #endregion
    }
}
