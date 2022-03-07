using System.Linq;
using System.Reflection;
using AutoMapper;

namespace Cps360.SyncWithCps.Tests.IntegrationTests.Common
{
    public class AutoMapperFixture
    {
        private readonly MapperConfiguration _mapperConfiguration;

        public AutoMapperFixture()
        {
            var assemblyNames = new string[] { "Presentation", "Application" };
            var assemblies = assemblyNames.Select(o => Assembly.Load(o)).ToArray();

            var profiles = assemblies
                .SelectMany(o => o.GetExportedTypes())
                .Where(t => typeof(Profile).GetTypeInfo().IsAssignableFrom(t.GetTypeInfo()))
                .Where(t => !t.GetTypeInfo().IsAbstract);

            _mapperConfiguration = new MapperConfiguration(mc =>
            {
                foreach (var profile in profiles)
                {
                    mc.AddProfile(profile);
                }
            });
        }

        public IMapper GetMapper() => _mapperConfiguration.CreateMapper();
        
    }
}