using System;
using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace CPS360.Sync.CSD.Presentation.ErrorHandling
{
    public class ServerErrorResponse : ErrorResponse
    {
        public string Exception { get; private set; }

        public ServerErrorResponse(Exception exception, HttpContext context)
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
