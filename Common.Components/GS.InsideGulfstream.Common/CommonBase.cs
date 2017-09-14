using System;
using System.Configuration;
using System.Globalization;
using System.Web.Caching;
using GS.InsideGulfstream.Common.Caching;
using GS.InsideGulfstream.Common.ExceptionHandling;
using GS.InsideGulfstream.Common.Utilities;
using Microsoft.SharePoint;

namespace GS.InsideGulfstream.Common
{
    public class CommonBase
    {
        #region EventHandlers

        /// <summary>
        /// OnRemove event handler for cache
        /// </summary>
        /// <param name="key">Cache Key</param>
        /// <param name="value">Object Value</param>
        /// <param name="reason">Reason for removal of cache</param>
        protected void OnRemove(string key, object value, CacheItemRemovedReason reason)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Methods
        
        /// Get caching properties from web.config
        /// </summary>
        /// <returns>CacheProperties Class</returns>
        protected static CacheProperties GetCacheProperties()
        {
            if (ConfigurationManager.AppSettings["IG.Cache.CacheTimeout"] == null) throw new InsideGulfstreamException("IG.Cache.CacheTimeout cannot be null in web.config!");
            if (ConfigurationManager.AppSettings["IG.Cache.IsCacheEnabled"] == null) throw new InsideGulfstreamException("IG.Cache.IsCacheEnabled cannot be null in web.config!");

            // Get web.config cache properties
            string cacheTimeout = ConfigurationManager.AppSettings["IG.Cache.CacheTimeout"].ToString(CultureInfo.InvariantCulture);
            string cacheEnabled = ConfigurationManager.AppSettings["IG.Cache.IsCacheEnabled"].ToString(CultureInfo.InvariantCulture);

            // Set CacheProperties to web.config values
            var cacheProperties = new CacheProperties
            {
                CacheTimeout = NumberHelper.ToSafeInt(StringHelper.ToSafeString(cacheTimeout)),
                CacheEnabled = (cacheEnabled.Trim().ToLowerInvariant() == "true")
            };

            // Return strongly typed Cache Class
            return cacheProperties;
        }

        /// <summary>
        /// Parse out link Description from SP Link field
        /// </summary>
        /// <example>
        /// string linkDescription = GetLinkDescription(item[SPBuiltInFieldId.URL]);
        /// </example>
        /// <param name="spLink">SharePoint Object Link</param>
        /// <returns>String Link Description</returns>
        protected string GetLinkDescription(Object spLink)
        {
            var typedValue = new SPFieldUrlValue(spLink.ToString());
            return typedValue.Description;
        }

        /// <summary>
        /// Parse out link Url from SP Link field
        /// </summary>
        /// <example>
        /// string linkUrl = GetLinkUrl(item[SPBuiltInFieldId.URL]);
        /// </example>
        /// <param name="spLink">SharePoint Object Link</param>
        /// <returns>String Link Url</returns>
        protected string GetLinkUrl(Object spLink)
        {
            var typedValue = new SPFieldUrlValue(spLink.ToString());
            return typedValue.Url;
        }
        
        /// <summary>
        /// Parse lookup items from a SharePoint List
        /// </summary>
        /// <example>
        /// string parentItem = ParseLookUpItem(item["ParentMenuItem"]);
        /// </example>
        /// <param name="obj">SharePoint List Item Object</param>
        /// <returns>String Value</returns>
        protected string ParseLookUpItem(object obj)
        {
            if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
            {
                return obj.ToString().Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries)[1];
            }
            return String.Empty;
        }
        
        /// <summary>
        /// Get a specified field from a SharePoint list where the list title matches the list title parameter.
        /// </summary>
        /// <param name="listName">Name of SharePoint List</param>
        /// <param name="listTitle">Name of List Title to match</param>
        /// <param name="listField">Name of SharePoint List Column To Retrieve</param>
        /// <returns>String List Field Value based on List Title Match</returns>
        protected string GetListFieldByParentTitle(string listName, string listTitle, string listField)
        {
            // Custom Content
            string retVal = String.Empty;

            // Run with elevated permissions to pull list
            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                // Get reference to list
                using (var siteRequest = new SPSite(UrlHelper.GetBrandingUrl()))
                {
                    using (SPWeb web = siteRequest.OpenWeb())
                    {
                        // Get IG Main Menu list on SPAdmin
                        var list = web.Lists[listName];
                        if (list != null)
                        {
                            SPListItemCollection items = list.GetItems("Title", listField);
                            foreach (SPListItem it in items)
                            {
                                if (it.Title == listTitle)
                                {
                                    retVal = StringHelper.ToSafeString(it[listField]);
                                    break; // Exit as we now have our custom content
                                }
                            }
                        }
                    }
                }
            });

            // Return value
            return retVal;
        }

        #endregion

    }
}