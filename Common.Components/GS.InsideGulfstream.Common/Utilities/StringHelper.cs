using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.InsideGulfstream.Common.Utilities
{
    public static class StringHelper
    {
        /// <summary>
        /// Method to return a valid string from an object
        /// </summary>
        /// <param name="obj">Date Object</param>
        /// <returns>Either an empty string if null or safe string value</returns>
        public static string ToSafeString(this object obj)
        {
            return (obj ?? string.Empty).ToString();
        }

        /// <summary>
        /// Remove all non alphanumeric characters from string
        /// </summary>
        /// <example>
        /// string text = StripNonAlpha("235asdfsdr23!~@5r23");
        /// </example>
        /// <param name="text"></param>
        /// <returns>String with Non Alphanumeric Characters stripped</returns>
        public static string StripNonAlpha(this string text)
        {
            char[] arr = text.ToCharArray();
            arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c)
                                              || char.IsWhiteSpace(c)
                                              || c == '-')));
            return new string(arr);
        }

        /// <summary>
        /// Remove all non alphanumeric characters and spaces from string
        /// </summary>
        /// <example>
        /// string text = StripNonAlpha("235as dfsd r23!~@5r23");
        /// </example>
        /// <param name="text"></param>
        /// <returns>String with Non Alphanumeric Characters and Whitespace stripped</returns>
        public static string StripNonAlphaAndWhitespace(this string text)
        {
            char[] arr = text.ToCharArray();
            arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c)
                                              || char.IsWhiteSpace(c)
                                              || c == '-')));
            return new string(arr).Replace(" ", "");
        }

        /// <summary>
        /// Get the first 10 words from text input.
        /// </summary>
        /// <param name="text">Text input such as a paragraph</param>
        /// <returns>First 10 words from the paragraph input</returns>
        public static string GetShortDescription(this string text)
        {
            return GetShortDescription(text, 10);
        }

        /// <summary>
        /// Get the first [x] words from text input.
        /// </summary>
        /// <param name="text">Text input such as a paragraph</param>
        /// <param name="maxWordCount">Max number of words to get from paragraph</param>
        /// <returns>First [x] words in small font from the paragraph input with line break before and followed by "..."</returns>
        public static string GetShortDescription(this string text, int maxWordCount)
        {
            if (text == null) return string.Empty;
            int wordCounter = 0;
            int stringIndex = 0;
            char[] delimiters = { '\n', ' ', ',', '.' };

            while (wordCounter < maxWordCount)
            {
                stringIndex = text.IndexOfAny(delimiters, stringIndex + 1);
                if (stringIndex == -1)
                    return String.Format("<br /><small>{0}</small>", text);

                ++wordCounter;
            }

            return String.Format("<br /><small>{0}...</small>", text.Substring(0, stringIndex));
        }

        /// <summary>
        /// Get the first [x] words from text input.
        /// </summary>
        /// <param name="text">Text input such as a title</param>
        /// <param name="maxWordCount">Max number of words to get from paragraph</param>
        /// <returns>First [x] words from the paragraph input</returns>
        public static string GetShortTitle(this string text, int maxWordCount)
        {
            if (text == null) return string.Empty;
            int wordCounter = 0;
            int stringIndex = 0;
            char[] delimiters = { '\n', ' ', ',', '.' };

            while (wordCounter < maxWordCount)
            {
                stringIndex = text.IndexOfAny(delimiters, stringIndex + 1);
                if (stringIndex == -1)
                    return text;

                ++wordCounter;
            }

            return text.Substring(0, stringIndex);
        }

    }
}
