using CPS360.Sync.CSD.Application.Common.DomainExceptions;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Diagnostics;

namespace CPS360.Sync.CSD.Presentation.ErrorHandling
{
    public class ClientErrorResponse : ErrorResponse
    {
        public string Error { get; set; }
        public string MoreDetails { get; set; }

        public ClientErrorResponse(DomainException exception, HttpContext context)
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