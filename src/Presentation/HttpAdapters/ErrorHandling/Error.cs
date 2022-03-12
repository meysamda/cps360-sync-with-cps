namespace Cps360.SyncWithCps.Presentation.HttpAdapters.ErrorHandling
{
    public abstract class Error
    {
        public int Status { get; set;}
        public string TraceId { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
    }
}