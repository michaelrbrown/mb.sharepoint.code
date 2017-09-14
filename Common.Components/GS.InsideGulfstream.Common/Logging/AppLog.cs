using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace GS.InsideGulfstream.Common.Logging
{
    public class AppLog
    {
        #region Properties

        // Get application name from assembly info
        private static string _applicationName = "GS.IG";
        public static string ApplicationName
        {
            get
            {
                string initialAssembly = "Newsletter";
                if (initialAssembly.Contains("Microsoft.VisualStudio"))
                {
                    return AppLog._applicationName + ".VisualStudioDebugger";
                }
                else
                {
                    return String.Format(AppLog._applicationName + ".{0}", initialAssembly);
                }
            }
            set
            {
                AppLog._applicationName = value;
            }
        }
        
        #endregion
        
        #region Methods

        /// <summary>
        /// Add Debug Event
        /// </summary>
        /// <param name="message">string</param>
        public static void AddDebugEvent(bool debug, string message)
        {
            if (debug)
            {
                try
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        // Write to SP ULS Log
                        var spDiagnosticsService = SPDiagnosticsService.Local;
                        spDiagnosticsService.WriteTrace(0, new SPDiagnosticsCategory(
                            ApplicationName,
                            TraceSeverity.Monitorable,
                            EventSeverity.Information),
                            TraceSeverity.Monitorable,
                            "AddEvent Info:  {0}", new object[] { message });
                    });
                }
                catch (Exception ex)
                {
                    // Write to SP ULS Log
                    var spDiagnosticsService = SPDiagnosticsService.Local;
                    spDiagnosticsService.WriteTrace(0, new SPDiagnosticsCategory(
                        ApplicationName,
                        TraceSeverity.Monitorable,
                        EventSeverity.Error),
                        TraceSeverity.Monitorable,
                        "AddEvent Error:  {0} :: {1}", new object[] { ex.Message, ex.StackTrace });
                }
            }
        }

        /// <summary>
        /// Add Event
        /// </summary>
        /// <param name="message">string</param>
        public static void AddEvent(string message)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    // Write to SP ULS Log
                    var spDiagnosticsService = SPDiagnosticsService.Local;
                    spDiagnosticsService.WriteTrace(0, new SPDiagnosticsCategory(
                        ApplicationName,
                        TraceSeverity.Monitorable,
                        EventSeverity.Information),
                        TraceSeverity.Monitorable,
                        "AddEvent Info:  {0}", new object[] { message });
                });
            }
            catch (Exception ex)
            {
                // Write to SP ULS Log
                var spDiagnosticsService = SPDiagnosticsService.Local;
                spDiagnosticsService.WriteTrace(0, new SPDiagnosticsCategory(
                    ApplicationName,
                    TraceSeverity.Monitorable,
                    EventSeverity.Error),
                    TraceSeverity.Monitorable,
                    "AddEvent Error:  {0} :: {1}", new object[] { ex.Message, ex.StackTrace });
            }
        }

        /// <summary>
        /// Add Exception
        /// </summary>
        /// <param name="message">string</param>
        public static void AddException(string message)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    // Write to SP ULS Log
                    var spDiagnosticsService = SPDiagnosticsService.Local;
                    spDiagnosticsService.WriteTrace(0, new SPDiagnosticsCategory(
                        ApplicationName,
                        TraceSeverity.Monitorable,
                        EventSeverity.Error),
                        TraceSeverity.Monitorable,
                        "AddException Info:  {0}", new object[] { message });
                });
            }
            catch (Exception ex)
            {
                // Write to SP ULS Log
                var spDiagnosticsService = SPDiagnosticsService.Local;
                spDiagnosticsService.WriteTrace(0, new SPDiagnosticsCategory(
                    ApplicationName,
                    TraceSeverity.Monitorable,
                    EventSeverity.Error),
                    TraceSeverity.Monitorable,
                    "AddException Error:  {0} :: {1}", new object[] { ex.Message, ex.StackTrace });
            }
        }

        /// <summary>
        /// Add Warning
        /// </summary>
        /// <param name="message">string</param>
        public static void AddWarning(string message)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    // Write to SP ULS Log
                    var spDiagnosticsService = SPDiagnosticsService.Local;
                    spDiagnosticsService.WriteTrace(0, new SPDiagnosticsCategory(
                        ApplicationName,
                        TraceSeverity.Monitorable,
                        EventSeverity.Warning),
                        TraceSeverity.Monitorable,
                        "AddWarning Info:  {0}", new object[] { message });
                });
            }
            catch (Exception ex)
            {
                // Write to SP ULS Log
                var spDiagnosticsService = SPDiagnosticsService.Local;
                spDiagnosticsService.WriteTrace(0, new SPDiagnosticsCategory(
                    ApplicationName,
                    TraceSeverity.Monitorable,
                    EventSeverity.Error),
                    TraceSeverity.Monitorable,
                    "AddWarning Error:  {0} :: {1}", new object[] { ex.Message, ex.StackTrace });
            }
        }

        #endregion

    }
}

