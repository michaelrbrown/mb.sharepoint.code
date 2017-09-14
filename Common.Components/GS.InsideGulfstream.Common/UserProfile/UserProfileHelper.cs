using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using GS.InsideGulfstream.Common.Caching;
using GS.InsideGulfstream.Common.Logging;
using GS.InsideGulfstream.Common.Utilities;
using Microsoft.Office.Server.UserProfiles;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration.Claims;

namespace GS.InsideGulfstream.Common.UserProfile
{
    public class UserProfileHelper : CommonBase
    {
        #region Locals

        private static object _lock = new object();

        #endregion
        
        /// <summary>
        /// Get the user login domain/user based on current login
        /// </summary>
        /// <returns>STRING domain/user</returns>
        public string GetCurrentUserLogin()
        {
            // Username
            string userName = null;
            // Create a new instance of SPClaimProviderManager
            SPClaimProviderManager mgr = SPClaimProviderManager.Local;
            // Get user login
            userName = mgr != null ? mgr.DecodeClaim(SPContext.Current.Web.CurrentUser.LoginName).Value : System.Web.HttpContext.Current.User.Identity.Name;
            // Return login
            return userName;
        }

        /// <summary>
        /// Gets the current city/state from SP User Profile Service.
        /// Currently mapped to the Office Location field (AD "l" field synced aka City).
        /// </summary>
        /// <returns>STRING city, state</returns>
        public string GetCurrentUserCityState()
        {
            string userLocation = "Savannah, GA";

            try
            {
                // Get current web and user
                SPWeb currentWeb = SPContext.Current.Web;
                SPUser currentUser = SPContext.Current.Web.CurrentUser;
                SPServiceContext serverContext = SPServiceContext.GetContext(currentWeb.Site);

                if (serverContext != null)
                {
                    // New instance of user profile manager
                    var profileManager = new UserProfileManager(serverContext);
                    if (!String.IsNullOrEmpty(currentUser.LoginName))
                    {
                        // Get current user login name (Domain/User)
                        var profile = profileManager.GetUserProfile(currentUser.LoginName);
                        if (profile != null)
                        {
                            // Get user location from "Office Location"
                            if (profile["SPS-Location"] != null)
                            {
                                userLocation = profile["SPS-Location"].ToString();
                                if (!String.IsNullOrEmpty(userLocation))
                                {
                                    // Get Cache Properties
                                    CacheProperties cacheProperties = GetCacheProperties();
                                    // Get all location list items
                                    var userProfileLocations = LoadListItems(cacheProperties);
                                    // Get the matching location for the "Office Location" abbreviation match (do contains search)
                                    var userProfileItem = userProfileLocations.FirstOrDefault(w => w.Abbreviation.Equals(userLocation));
                                    if (userProfileItem != null)
                                    {
                                        if (!String.IsNullOrEmpty(userProfileItem.City))
                                        {
                                            userLocation = userProfileItem.City; // Return final location
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                // Send error to logging provider
                AppLog.AddException(String.Format("Source: {0}; Error: {1}; Stack Trace: {2}", ex.Source, ex.Message, ex.StackTrace));
            }

            // Return current user location
            return userLocation;

        }

        /// <summary>
        /// Load list items for SPAdmin list
        /// </summary>
        /// <returns></returns>
        private IEnumerable<UserProfileProperties> LoadListItems(CacheProperties cacheProperties)
        {
            // Get unique cache name per parent UserProfileItem
            const string cacheName = "UserProfileItem";
            // Create new instance of MenuItem list
            var menuItems = (List<UserProfileProperties>)HttpRuntime.Cache[cacheName];

            try
            {
                // Do we have a cached item OR is caching disabled?
                if (menuItems == null)
                {
                    lock (_lock)
                    {
                        // Run with elevated permissions to pull list
                        SPSecurity.RunWithElevatedPrivileges(() =>
                        {
                            using (var siteRequest = new SPSite(UrlHelper.GetBrandingUrl()))
                            {
                                using (SPWeb web = siteRequest.OpenWeb())
                                {
                                    // Get User Profile Locations list on SPAdmin
                                    var list = web.Lists["User Profile Locations"];
                                    if (list != null)
                                    {
                                        // Order By = Abbreviation
                                        const string caml = @"<OrderBy><FieldRef Name='Abbreviation' Ascending='True' /></OrderBy>";
                                        var qry = new SPQuery { Query = caml };
                                        // Get items from list
                                        SPListItemCollection items = list.GetItems(qry);
                                        if (items != null)
                                        {
                                            menuItems = new List<UserProfileProperties>();
                                            menuItems.AddRange(
                                                from SPListItem item in items
                                                select new UserProfileProperties
                                                {
                                                    Abbreviation = StringHelper.ToSafeString(item["Abbreviation"]),
                                                    City = StringHelper.ToSafeString(item["City"])
                                                }
                                            );
                                        }
                                    }
                                }
                            }
                        });

                        // Cache Strongly Typed Main Menu Item Class
                        if (menuItems != null)
                            HttpRuntime.Cache.Add(cacheName, menuItems, null, DateTime.Now.AddHours(24), Cache.NoSlidingExpiration, CacheItemPriority.AboveNormal, OnRemove);
                    }
                } // End menuItems == null

            }
            catch (Exception ex)
            {
                // Send error to logging provider
                AppLog.AddException(String.Format("Source: {0}; Error: {1}; Stack Trace: {2}", ex.Source, ex.Message, ex.StackTrace));
            }

            // Return strongly typed list items
            return menuItems;
        }
        
    }
}