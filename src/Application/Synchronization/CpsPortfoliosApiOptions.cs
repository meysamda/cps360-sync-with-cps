namespace Cps360.SyncWithCps.Application.Synchronization
{
    public class CpsPortfoliosApiOptions
    {
        public string GetPortfoliosUrl { get; set; }
        public AuthorizationOptions Authorization { get; set; }

        public class AuthorizationOptions
        {
            public string DiscoveryDocumentUrl { get; set; }
            public string ClientId { get; set; }
            public string ClientSecret { get; set; }
            public string Scope { get; set; }
        }
    }
}