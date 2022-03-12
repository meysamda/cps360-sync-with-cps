using System;
using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace Cps360.SyncWithCps.Presentation.HttpAdapters.ErrorHandling
{
    public class ServerError : Error
    {
        public string Exception { get; private set; }

        public ServerError(Exception exception, HttpContext context)
        {
            Init(exception);

            if (exception is NotImplementedException)
                Status = (int)HttpStatusCode.NotImplemented;
                
            else
                Status = (int)HttpStatusCode.InternalServerError;

            Exception = exception.Message;
            TraceId = Activity.Current?.Id ?? context?.TraceIdentifier;
        }

        private void Init(Exception exception)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6";
            Title = "serverErrorOccurred";
        }
    }
}
