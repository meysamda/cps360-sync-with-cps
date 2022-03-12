using System;

namespace Cps360.SyncWithCps.Application.Exceptions
{
    public class ClientException : Exception
    {
        public ClientException(ErrorStatusCode statusCode, ErrorMessage errorMessage, string moreDetails = null)
        : base(null) { ErrorStatusCode = statusCode; ErrorMessage = errorMessage; }

        public ErrorStatusCode ErrorStatusCode { get; set; }
        public ErrorMessage ErrorMessage { get; set; }
        public string MoreDetails { get; set; }
    }
}