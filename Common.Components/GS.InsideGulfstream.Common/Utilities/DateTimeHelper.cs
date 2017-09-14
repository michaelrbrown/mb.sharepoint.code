using System;
using System.Globalization;

namespace GS.InsideGulfstream.Common.Utilities
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// Method to return a valid date from an object
        /// </summary>
        /// <example>
        /// DateTime startTime = DateTryParse(item["Start Time"]);
        /// </example>
        /// <param name="dateObj"></param>
        /// <returns></returns>
        public static DateTime? DateTryParse(this object dateString, string format = null)
        {
            DateTime dt;
            bool success = format == null ? DateTime.TryParse(dateString.ToSafeString(), out dt) : DateTime.TryParseExact(dateString.ToSafeString(), format, null, DateTimeStyles.None, out dt);
            return success ? dt : (DateTime?)null;
        }

        /// <summary>
        /// Helper method to determine if date is between start and end date time
        /// </summary>
        /// <param name="dt">DateTime Today (or date to check between)</param>
        /// <param name="start">DateTime StartDateTime</param>
        /// <param name="end">DateTime EndDateTime</param>
        /// <returns>True if between, False if not</returns>
        public static bool IsBetween(DateTime dt, DateTime start, DateTime end)
        {
            return dt >= start && dt <= end;
        }

    }
}
