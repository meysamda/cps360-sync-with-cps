using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Cps360.SyncWithCps.Presentation.HttpAdapters.ErrorHandling
{
    public class BadRequestError : Error
    {
        public IEnumerable<BadRequestErrorItem> Errors { get; set; }

        public BadRequestError(Application.Exceptions.BadRequestException exception, HttpContext context)
        {
            Init();

            string errorMessage = string.Join(", ", exception.ErrorMessages.Select(o => o.ToString()));
            TraceId = Activity.Current?.Id ?? context?.TraceIdentifier;
            Errors = new[] { new BadRequestErrorItem(exception.Key, errorMessage) };
        }

        public BadRequestError(BadRequestException exception)
        {
            Init();

            TraceId = Activity.Current?.Id ?? exception.ActionExecutingContext.HttpContext?.TraceIdentifier;
            Errors = exception.ActionExecutingContext.ModelState.SelectMany(m => m.Value.Errors.Select(o => new BadRequestErrorItem(m.Key, o.ErrorMessage)));
        }

        public BadRequestError(string errorKey, string errorMessage, HttpContext context)
        {
            Init();

            TraceId = Activity.Current?.Id ?? context?.TraceIdentifier;
            Errors = new[] { new BadRequestErrorItem(errorKey, errorMessage) };
        }

        public class BadRequestErrorItem
        {
            public string Key { get; private set; }
            public IEnumerable<string> Values { get; private set; }

            public BadRequestErrorItem(string key, string errorMessage)
            {
                Key = Char.ToLowerInvariant(key[0]) + key.Substring(1);

                Values = errorMessage?
                    .Split(",")
                    .Select(o => Char.ToLowerInvariant(o[0]) + o.Substring(1)) ??
                    new string[0];

                if (Values.Any(o => o.Contains("is required")))
                {
                    Values = Values.Select(o => {
                        if (o.Contains("is required"))
                            o = "isRequired";

                        return o;
                    });
                }
            }
        }

        private void Init()
        {
            Status = 400;
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
            Title = "validationFailed";
        }
    }
}