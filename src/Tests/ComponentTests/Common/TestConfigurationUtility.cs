using System;
using Microsoft.Extensions.Configuration;

namespace Cps360.SyncWithCps.Tests.ComponentTests.Common
{
    public static class TestConfigurationUtility
    {
        public static IConfiguration GetConfiguration()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (string.IsNullOrEmpty(environmentName))
                environmentName = "Development";

            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", false);
            builder.AddJsonFile($"appsettings.{environmentName}.json", false);
            builder.AddEnvironmentVariables();
            
            return builder.Build();
        }
    }
}