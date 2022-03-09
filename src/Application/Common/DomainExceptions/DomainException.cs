using System;

namespace Cps360.SyncWithCps.Application.Common.DomainExceptions
{
    public class DomainException : Exception
    {
        public DomainException(ErrorMessage errorMessage)
        : base(null) { ErrorMessage = errorMessage; }

        public ErrorMessage ErrorMessage { get; set; }
    }
}