using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cps360.SyncWithCps.Presentation.ErrorHandling
{
    public class BadRequestException : Exception
    {
        public BadRequestException(ActionExecutingContext context)
        : base() { ActionExecutingContext = context; }
        
        public ActionExecutingContext ActionExecutingContext { get; set; }
    }
}