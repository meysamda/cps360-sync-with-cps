using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace Cps360.SyncWithCps.Presentation.HttpAdapters.ErrorHandling
{
    public class ClientError : Error
    {
        public string Error { get; set; }
        public string MoreDetails { get; set; }

        public ClientError(Application.Exceptions.ClientException exception, HttpContext context)
        {
            Init();

            Status = (int)exception.ErrorStatusCode;
            Error = exception.ErrorMessage.ToString();
            MoreDetails = exception.MoreDetails;
            TraceId = Activity.Current?.Id ?? context?.TraceIdentifier;
        }

        private void Init()
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5";
            Title = "clientErrorOccurred";
        }
    }
}