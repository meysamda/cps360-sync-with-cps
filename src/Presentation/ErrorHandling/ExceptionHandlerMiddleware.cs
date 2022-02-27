using Cps360.SyncWithCps.Application.Common.DomainExceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Cps360.SyncWithCps.Presentation.ErrorHandling
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
            _logger = Serilog.Log.ForContext<ExceptionHandlerMiddleware>();;
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
                ErrorResponse response;

                if (ex is DomainException domainException)
                    response = new ClientErrorResponse(domainException, context);
                
                else if (ex is DomainBadRequestException domainBadRequestException)
                    response = new BadRequestErrorResponse(domainBadRequestException, context);

                else if (ex is BadRequestException badRequestException)
                    response = new BadRequestErrorResponse(badRequestException);

                else if (ex is DbUpdateException dbUpdateException && dbUpdateException.InnerException.Message.Contains("duplicate key"))
                    response = new BadRequestErrorResponse("database", ErrorMessage.entityWithTheSameKeyAlreadyExists.ToString(), context);

                else
                {
                    response = new ServerErrorResponse(ex, context);
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
