using System.Collections.Generic;

namespace CPS360.Sync.CSD.Presentation.ErrorHandling
{
    public abstract class ErrorResponse
    {
        public int Status { get; set;}
        public string TraceId { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
    }
}