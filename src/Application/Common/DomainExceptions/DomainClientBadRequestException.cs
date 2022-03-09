using System;

namespace Cps360.SyncWithCps.Application.Common.DomainExceptions
{
    public class DomainClientBadRequestException : Exception
    {
        public DomainClientBadRequestException(string key, params ErrorMessage[] errorMessages)
        : base() { Key = key; ErrorMessages = errorMessages; }

        public DomainClientBadRequestException(string key, string errorMessage)
        : base() { Key = key; ErrorMessage = errorMessage; }
        
        public string Key { get; set; }
        public ErrorMessage[] ErrorMessages { get; set; }
        public string ErrorMessage { get; set; }
    }
}