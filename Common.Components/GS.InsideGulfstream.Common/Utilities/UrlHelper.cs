using System;
using System.Configuration;
using System.Globalization;
using System.Web;
using GS.InsideGulfstream.Common.ExceptionHandling;

namespace GS.InsideGulfstream.Common.Utilities
{
    public static class UrlHelper
    {
        /// <summary>
        /// Get branding url properties from web.config
        /// </summary>
        /// <returns>Branding Url String</returns>
        public static string GetBrandingUrl()
        {
            if (ConfigurationManager.AppSettings["IG.BrandingUrl"] == null) throw new InsideGulfstreamException("IG.BrandingUrl cannot be null in web.config!");

            // Get web.config branding url
            string brandingUrl = ConfigurationManager.AppSettings["IG.BrandingUrl"].ToString(CultureInfo.InvariantCulture);

            // Return Branding Url
            return brandingUrl;
        }
        
        /// <summary>
        /// Gets the main IG fully qualified absolute Uri
        /// </summary>
        /// <returns>IG absolute Uri</returns>
        public static Uri GetInsideGulfstreamAbsoluteUri()
        {
            // Get BrandingUrl from web.config
            Uri originalUrl = new Uri(GetBrandingUrl());
            // Get host part from BrandingUrl
            string domain = originalUrl.Host; // www.mydomain.com
            // Return the entire fully qualified Url
            return new Uri(String.Concat(originalUrl.Scheme, Uri.SchemeDelimiter, originalUrl.Host));
        }

        /// <summary>
        /// Determines if the Url is absolute
        /// </summary>
        /// <param name="url">String Url(valid)</param>
        /// <returns>bool true if Url is absolute</returns>
        public static bool IsAbsoluteUrl(this string url)
        {
            Uri result;
            return Uri.TryCreate(url, UriKind.Absolute, out result);
        }

        /// <summary>
        /// Returns the absolute url from a full url path domain/relative/querystring
        /// </summary>
        /// <param name="url">String Url</param>
        /// <returns>Absolute Url from a full Url Address</returns>
        public static string ToAbsoluteUrl(this string url)
        {
            var uri = new Uri(url);
            return uri.GetLeftPart(UriPartial.Authority);
        }

        /// <summary>
        /// Converts the provided app-relative path into an absolute Url containing the 
        /// full host name
        /// </summary>
        /// <param name="relativeUrl">App-Relative path</param>
        /// <returns>Provided relativeUrl parameter as fully qualified Url</returns>
        /// <example>~/path/to/foo to http://www.web.com/path/to/foo</example>
        public static string ToAbsoluteUrlFromHttpContext(this string relativeUrl)
        {
            if (String.IsNullOrEmpty(relativeUrl)) return relativeUrl;
            // If we can't get current context Url just return relative
            if (HttpContext.Current == null) return relativeUrl;

            if (relativeUrl.StartsWith("/"))
                relativeUrl = relativeUrl.Insert(0, "~");
            if (!relativeUrl.StartsWith("~/"))
                relativeUrl = relativeUrl.Insert(0, "~/");

            // Get current context Url
            var url = HttpContext.Current.Request.Url;
            // Get the port number
            var port = url.Port != 80 ? (":" + url.Port) : String.Empty;

            // Return the full domain and relative path combined
            return String.Format("{0}://{1}{2}{3}", url.Scheme, url.Host, port, VirtualPathUtility.ToAbsolute(relativeUrl));
        }

        /// <summary>
        /// Converts the provided app-relative path into an absolute Url containing the 
        /// full host name from the web.config BrandingUrl
        /// </summary>
        /// <param name="relativeUrl">App-Relative path</param>
        /// <returns>Provided relativeUrl parameter as fully qualified Url</returns>
        /// <example>~/path/to/foo to http://insidegulfstream.com/path/to/foo</example>
        public static string CombineRelativeToInsideGulfstreamDomain(this string relativeUrl)
        {
            // Get fully qualified IG absolute Uri
            Uri domainUrl = GetInsideGulfstreamAbsoluteUri();

            // If the relativeUrl is null or empty just return the domain
            if (String.IsNullOrEmpty(relativeUrl)) return domainUrl.ToString();

            if (relativeUrl.StartsWith("/"))
                relativeUrl = relativeUrl.Insert(0, "~");
            if (!relativeUrl.StartsWith("~/"))
                relativeUrl = relativeUrl.Insert(0, "~/");

            // Get the port number
            string port = domainUrl.Port != 80 ? (":" + domainUrl.Port) : String.Empty;

            // Return the full domain and relative path combined
            return String.Format("{0}{1}{2}", domainUrl, port, VirtualPathUtility.ToAbsolute(relativeUrl));
        }

        /// <summary>
        /// Combine a base and relative address using <seealso cref="Uri">Uri</seealso> namespace. 
        /// The <paramref name="baseAddress">baseAddress</paramref> is stripped to the absolute path (removes all relative path info) 
        /// before combining the new <paramref name="relativeAddress">relativeAddress</paramref>.
        /// </summary>
        /// <param name="baseAddress">Base Uri</param>
        /// <param name="relativeAddress">Relative Uri</param>
        /// <returns>Combined Uri from base and relative address</returns>
        public static string CombineUrls(string baseAddress, string relativeAddress)
        {
            // Validation
            if (baseAddress.Length == 0)
                return relativeAddress;
            if (relativeAddress.Length == 0)
                return new Uri(String.Format("{0}/", baseAddress.TrimEnd(new char[] { '/', '\\' }))).ToString();

            // Format base and relative addresses into a full Uri
            var baseUri = new Uri(String.Format("{0}/", baseAddress.TrimEnd(new char[] { '/', '\\' })));

            // Return new Uri
            return new Uri(baseUri, relativeAddress).ToString();
        }

        /// <summary>
        ///  Combine a Url from a base and relative Url
        /// </summary>
        /// <param name="baseUrl">Base Url path</param>
        /// <param name="relativeUrl">Relative Url path</param>
        /// <returns>Combined Url path from base and relative Url</returns>
        public static string UrlCombine(this string baseUrl, string relativeUrl)
        {
            if (baseUrl.Length == 0)
                return relativeUrl;
            if (relativeUrl.Length == 0)
                return baseUrl;
            return string.Format("{0}/{1}", baseUrl.TrimEnd(new char[] { '/', '\\' }), relativeUrl.TrimStart(new char[] { '/', '\\' }));
        }

        /// <summary>
        /// Decode the Publishing Image Link to extract the right Url
        /// </summary>
        /// <param name="publishingImage">Publishing Image</param>
        /// <returns>The Url to the Image</returns>
        public static string ToPublishingImageUrl(this string publishingImage)
        {
            string imageUrl = "";
            string[] attrArray = publishingImage.Split('\"');
            int index = Array.IndexOf(attrArray, " src=");

            imageUrl = (index >= 0) ? attrArray[index + 1] : "";

            return imageUrl;
        }

    }
}
