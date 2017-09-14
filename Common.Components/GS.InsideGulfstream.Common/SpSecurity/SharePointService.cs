using System;
using GS.InsideGulfstream.Common.Logging;
using Microsoft.SharePoint.Client;

namespace GS.InsideGulfstream.Common.SpSecurity
{
    public static class SharePointService
    {
        /// <summary>
        /// Get the user name from SharePoint user context
        /// </summary>
        /// <param name="spContext"></param>
        /// <returns>String user name</returns>
        public static string GetUserName(SharePointContext spContext)
        {
            string userName = null;
            User spUser = null;

            // Get new instance client context for SharePoint host
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {
                    spUser = clientContext.Web.CurrentUser;
                    clientContext.Load(spUser, user => user.Title);
                    clientContext.ExecuteQuery();
                    userName = spUser.Title;
                }
            }

            return userName;
        }

        /// <summary>
        /// Get the user login (gac\id) from SharePoint user context
        /// </summary>
        /// <param name="spContext"></param>
        /// <returns>String user login (gac\id)</returns>
        public static string GetUserLogin(SharePointContext spContext)
        {
            string userLogin = null;
            User spUser = null;

            try
            {
                // Get new instance client context for SharePoint host
                using (var clientContext = spContext.CreateUserClientContextForSPHost())
                {
                    if (clientContext != null)
                    {
                        spUser = clientContext.Web.CurrentUser;
                        clientContext.Load(spUser, user => user.LoginName);
                        clientContext.ExecuteQuery();
                        userLogin = spUser.LoginName;
                    }
                }

            }
            catch (Exception ex)
            {
                // Send error to logging provider
                AppLog.AddException(String.Format("Source: {0}; Error: {1}; Stack Trace: {2}", ex.Source, ex.Message, ex.StackTrace));
            }
           
            return userLogin;
        }

        /// <summary>
        /// Get the user id info object from SharePoint user context
        /// </summary>
        /// <param name="spContext"></param>
        /// <returns>UserIdInfo context</returns>
        public static UserIdInfo GetUserId(SharePointContext spContext)
        {
            UserIdInfo userId = null;
            User spUser = null;

            // Get new instance client context for SharePoint host
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {
                    spUser = clientContext.Web.CurrentUser;
                    clientContext.Load(spUser, user => user.UserId);
                    clientContext.ExecuteQuery();
                    userId = spUser.UserId;
                }
            }

            return userId;
        }

        /// <summary>
        /// Determines if user is a site admin based on SharePoint user context
        /// </summary>
        /// <param name="spContext"></param>
        /// <returns>Bool is site admin</returns>
        public static bool IsSiteAdmin(SharePointContext spContext)
        {
            bool isSiteAdmin = false;
            User spUser = null;

            // Get new instance client context for SharePoint host
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {
                    spUser = clientContext.Web.CurrentUser;
                    clientContext.Load(spUser, user => user.IsSiteAdmin);
                    clientContext.ExecuteQuery();
                    isSiteAdmin = spUser.IsSiteAdmin;
                }
            }

            return isSiteAdmin;
        }

    }

}
