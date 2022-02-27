using Cps360.SyncWithCps.Presentation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Cps360.SyncWithCps.Tests.ComponentTests.Common
{
    public abstract class HttpClientFixture
    {
        protected readonly IRESTFulApiFactoryClient _client;
        protected readonly IRESTFulApiFactoryClient _authorizedClient;

        protected HttpClientFixture()
        {
            var factory = new WebApplicationFactory<Startup>();
            var client = factory.CreateClient();

            var authorizedClient = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication("Test").AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test",
                        options =>
                        {
                        });
                });
            }).CreateClient();

            _client = new RESTFulApiFactoryClient(client);
            _authorizedClient = new RESTFulApiFactoryClient(authorizedClient);
        }
    }
}