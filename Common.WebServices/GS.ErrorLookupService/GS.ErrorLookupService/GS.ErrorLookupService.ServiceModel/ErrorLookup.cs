using System;
using GS.ErrorLookupService.ServiceModel.Models;
using ServiceStack;
using ServiceStack.DataAnnotations;

namespace GS.ErrorLookupService.ServiceModel
{
    [Description("Lookup SharePoint errors by Correlation Id, Start DateTime, and End DateTime")]
    public class ErrorLookup: IReturn<ErrorLookupResponse>
    {
        public string CorrelationId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }

    public class ErrorLookupResponse
    {
        // Result payload
        public QueryResult Result { get; set; }
        // Error result
        public string ErrorResult { get; set; }
        //Automatic exception handling
        public ResponseStatus ResponseStatus { get; set; }
    }
}