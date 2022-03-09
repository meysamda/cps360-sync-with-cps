using System;

namespace Cps360.SyncWithCps.Application.Common.DomainExceptions
{
    public class DomainClientException : Exception
    {
        public DomainClientException(ErrorStatusCode statusCode, ErrorMessage errorMessage)
        : base(null) { ErrorStatusCode = statusCode; ErrorMessage = errorMessage; }

        public ErrorStatusCode ErrorStatusCode { get; set; }
        public ErrorMessage ErrorMessage { get; set; }
    }
}