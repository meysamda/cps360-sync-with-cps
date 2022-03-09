using Cps360.SyncWithCps.Application.Common.DomainExceptions;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Diagnostics;

namespace Cps360.SyncWithCps.Presentation.ErrorHandling
{
    public class ClientErrorResponse : ErrorResponse
    {
        public string Error { get; set; }
        public string MoreDetails { get; set; }

        public ClientErrorResponse(DomainClientException exception, HttpContext context)
        {
            Init();

            Status = (int)exception.ErrorStatusCode;
            Error = exception.ErrorMessage.ToString();
            MoreDetails = exception.Message.Contains("DomainException") ? null : exception.Message;
            TraceId = Activity.Current?.Id ?? context?.TraceIdentifier;
        }

        private void Init()
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5";
            Title = "clientErrorOccurred";
        }
    }
}