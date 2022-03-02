using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace Cps360.SyncWithCps.Application.CpsPortfolios
{
    public class CpsPortfoliosApiClient : ICpsPortfoliosApiClient
    {
        private static HttpClient Client;
        private HttpClient _client { get => GetClient(); }
        private static readonly object _lock = new object();

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly CpsPortfoliosApiOptions _cpsPortfoliosApiOptions;

        public CpsPortfoliosApiClient(IHttpClientFactory httpClientFactory, CpsPortfoliosApiOptions cpsPortfoliosApiOptions, ILogger<CpsPortfoliosApiClient> logger)
        {
            _httpClientFactory = httpClientFactory;
            _cpsPortfoliosApiOptions = cpsPortfoliosApiOptions;            
        }

        public Task<IEnumerable<CpsPortfolio>> GetCpsPortfolios(int page, int pageSize, CancellationToken cancellationToken)
        {
            var uri = GetPortfoliosPageUri(page, pageSize);
            var restClient = new RestClient(_client);
            var request = new RestRequest(uri);

            return restClient.GetAsync<IEnumerable<CpsPortfolio>>(request, cancellationToken);
        }

        private Uri GetPortfoliosPageUri(int page, int pageSize)
        {
            var skip = page * pageSize;

            var parameters = new List<Tuple<string, string>>();
            parameters.Add(new Tuple<string, string>("skip", skip.ToString()));
            parameters.Add(new Tuple<string, string>("limit", pageSize.ToString()));
            var queryString = string.Join("&", parameters
                .Select(o => $"{o.Item1}={o.Item2}")
                .ToArray());

            return new Uri($"{_cpsPortfoliosApiOptions.GetPortfoliosUrl}?{queryString}");
        }

        private HttpClient GetClient()
        {
            if (Client == null)
            {
                lock (_lock)
                {
                    if (Client == null)
                    {
                        var httpClient = _httpClientFactory.CreateClient();

                        var discoveryDocument = httpClient.GetDiscoveryDocumentAsync(_cpsPortfoliosApiOptions.Authorization.DiscoveryDocumentUrl).Result;
                        if (discoveryDocument.IsError)
                            throw new ApplicationException($"trying to get discovery document from address {_cpsPortfoliosApiOptions.Authorization.DiscoveryDocumentUrl}, error occurred; error details: {discoveryDocument.Error}");

                        var tokenRequest = new ClientCredentialsTokenRequest
                        {
                            Address = discoveryDocument.TokenEndpoint,
                            ClientId = _cpsPortfoliosApiOptions.Authorization.ClientId,
                            ClientSecret = _cpsPortfoliosApiOptions.Authorization.ClientSecret,
                            Scope = _cpsPortfoliosApiOptions.Authorization.Scope
                        };

                        var tokenResponse = httpClient.RequestClientCredentialsTokenAsync(tokenRequest).Result;
                        if (tokenResponse.IsError)
                            throw new ApplicationException($"trying to get token from address {discoveryDocument.TokenEndpoint}, error occurred; error details: {tokenResponse.Error}");

                        httpClient.SetBearerToken(tokenResponse.AccessToken);

                        Client = httpClient;
                    }
                }
            }

            return Client;
        }
    }
}