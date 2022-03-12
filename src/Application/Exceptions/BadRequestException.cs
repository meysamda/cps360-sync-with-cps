using System;

namespace Cps360.SyncWithCps.Application.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string key, params ErrorMessage[] errorMessages)
        : base() { Key = key; ErrorMessages = errorMessages; }

        public string Key { get; set; }
        public ErrorMessage[] ErrorMessages { get; set; }
    }
}