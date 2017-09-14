using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.InsideGulfstream.Common.Utilities
{
    public static class NumberHelper
    {
        /// <summary>
        /// Method which converts returns a valid number from a string value
        /// </summary>
        /// <param name="num">String Number to convert to Int</param>
        /// <returns>Zero if null or a valid number</returns>
        public static int ToSafeInt(this string num)
        {
            int number;
            if (!Int32.TryParse(num, out number))
            {
                if (num == null) number = 0;
            }
            return number;
        }
    }
}
