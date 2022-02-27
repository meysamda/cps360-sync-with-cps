using System;

namespace Cps360.SyncWithCps.Application.Common.DomainExceptions
{
    public class DomainBadRequestException : Exception
    {
        public DomainBadRequestException(string key, ErrorMessage errorMessage)
        : base() { Key = key; ErrorMessage = errorMessage; }

        public DomainBadRequestException(string key, string errorMessage)
        : base() { Key = key; ErrorMessageStr = errorMessage; }
        
        public string Key { get; set; }
        public ErrorMessage? ErrorMessage { get; set; }
        public string ErrorMessageStr { get; set; }
    }
}