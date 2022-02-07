namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationBinderExtension
    {
        public static T Get<T>(this IConfiguration configuration, string key) where T : new()
        {
            var instance = new T();
            configuration.Bind(key, instance);
            return instance;
        }
    }
}