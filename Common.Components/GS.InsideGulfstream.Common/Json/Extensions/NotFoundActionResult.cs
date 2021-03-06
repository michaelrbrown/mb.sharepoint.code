﻿using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace GS.InsideGulfstream.Common.Json.Extensions
{
    public class NotFoundActionResult : IHttpActionResult
    {
        public string Message { get; private set; }
        public HttpRequestMessage Request { get; private set; }

        public NotFoundActionResult(HttpRequestMessage request, string message)
        {
            this.Request = request;
            this.Message = message;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(ExecuteResult());
        }

        public HttpResponseMessage ExecuteResult()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.NotFound);

            response.Content = new StringContent(Message);
            response.RequestMessage = Request;
            return response;
        }
    }
    
}