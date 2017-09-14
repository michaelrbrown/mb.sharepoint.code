using System;
using GS.ErrorLookupService.ServiceModel.Models;
using ServiceStack;
using GS.ErrorLookupService.ServiceModel;
using Microsoft.SharePoint;

namespace GS.ErrorLookupService.ServiceInterface
{
    public class MyServices : Service
    {
        /// <summary>
        /// GET method to lookup SharePoint error messages
        /// </summary>
        /// <param name="errorLookupRequest">ErrorLookup Request DTO</param>
        /// <returns>ErrorLookup Response DTO</returns>
        public object Get(ErrorLookup errorLookupRequest)
        {
            // Return an error message if no correlation id is null
            if (String.IsNullOrEmpty(errorLookupRequest.CorrelationId))
            {
                return new ErrorLookupResponse {ErrorResult = "Correlation Id is required!"};
            };
            // New instance of ErrorLookupResponse DTO
            var dto = new ErrorLookupResponse { Result = GetLogInfo(errorLookupRequest.StartDateTime, errorLookupRequest.EndDateTime, errorLookupRequest.CorrelationId) };
            // Use SericeStacks built in JSON Serializer to convert to JSON
            string json = dto.ToJson();
            //var fromJson = json.FromJson<ErrorLookupResponse>();
            // Return error lookup result as JSON
            return json;
        }

        /// <summary>
        /// Method to get SharePoint error info from combining ULS logs across all servers (works for load balanced server farm)
        /// </summary>
        /// <param name="startDateTime">Start DateTime for logs</param>
        /// <param name="endDateTime">End DateTime for logs</param>
        /// <param name="correlationId">Correlation Id to search for</param>
        /// <returns>QueryResult DTO</returns>
        protected QueryResult GetLogInfo(DateTime startDateTime, DateTime endDateTime, string correlationId)
        {
            // Create new instance of query result
            QueryResult result = null;
            try
            {
                // Elevate permissions
                SPSecurity.RunWithElevatedPrivileges(
                    delegate()
                    {
                        // Get query results from correlation id
                        var query = new CorrelationQuery(DateTime.Now.AddHours(-1), DateTime.Now, new Guid().ToString());
                        // Query ALL SP servers
                        query.QueryServers(DateTime.Now.AddHours(-1), DateTime.Now, new Guid().ToString());
                        // Get result
                        result = query.Result;
                    });
            }
            catch (Exception ex)
            {
                result = new QueryResult(ex);
            }

            // Return query result
            return result;
        }

    }
}