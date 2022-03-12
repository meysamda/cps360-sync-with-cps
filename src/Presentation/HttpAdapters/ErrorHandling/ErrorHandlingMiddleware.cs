using Cps360.SyncWithCps.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Cps360.SyncWithCps.Presentation.HttpAdapters.ErrorHandling
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
            _logger = Serilog.Log.ForContext<ErrorHandlingMiddleware>();;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/problem+json; charset=utf-8";
                Error response;

                if (ex is Application.Exceptions.ClientException clientException)
                    response = new ClientError(clientException, context);
                
                else if (ex is Application.Exceptions.BadRequestException domainBadRequestException)
                    response = new BadRequestError(domainBadRequestException, context);

                else if (ex is Application.Exceptions.DomainException domainException)
                    response = new BadRequestError("domain", domainException.ErrorMessage.ToString(), context);

                else if (ex is Presentation.HttpAdapters.ErrorHandling.BadRequestException presentationBadRequestException)
                    response = new BadRequestError(presentationBadRequestException);

                else if (ex is DbUpdateException dbUpdateException && dbUpdateException.InnerException.Message.Contains("duplicate key"))
                    response = new BadRequestError("database", "entityWithTheSameKeyAlreadyExists", context);

                else
                {
                    response = new ServerError(ex, context);
                    _logger.Error(ex, "An unhandled exception accured. {@ErrorDetails}", response);
                }
                
                context.Response.StatusCode = response.Status;
                var serializedResult = Serialize(response);
                await context.Response.WriteAsync(serializedResult);
            }
        }

        private string Serialize(dynamic err) {
            return JsonConvert.SerializeObject(err, new JsonSerializerSettings {
                ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() },
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                MaxDepth = 10
            }); 
        }
    }
}
