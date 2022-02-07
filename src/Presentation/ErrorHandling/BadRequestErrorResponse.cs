using CPS360.Sync.CSD.Application.Common.DomainExceptions;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CPS360.Sync.CSD.Presentation.ErrorHandling
{
    public class BadRequestErrorResponse : ErrorResponse
    {
        public IEnumerable<BadRequestError> Errors { get; set; }

        public BadRequestErrorResponse(DomainBadRequestException exception, HttpContext context)
        {
            Init();

            string errorMessage = exception.ErrorMessage.HasValue ?
                exception.ErrorMessage.ToString() :
                exception.ErrorMessageStr;

            TraceId = Activity.Current?.Id ?? context?.TraceIdentifier;
            Errors = new[] { new BadRequestError(exception.Key, errorMessage) };
        }

        public BadRequestErrorResponse(BadRequestException exception)
        {
            Init();

            TraceId = Activity.Current?.Id ?? exception.ActionExecutingContext.HttpContext?.TraceIdentifier;
            Errors = exception.ActionExecutingContext.ModelState.SelectMany(m => m.Value.Errors.Select(o => new BadRequestError(m.Key, o.ErrorMessage)));
        }

        public BadRequestErrorResponse(string errorKey, string errorMessage, HttpContext context)
        {
            Init();

            TraceId = Activity.Current?.Id ?? context?.TraceIdentifier;
            Errors = new[] { new BadRequestError(errorKey, errorMessage) };
        }

        private void Init()
        {
            Status = 400;
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
            Title = "validationFailed";
        }
    }
}