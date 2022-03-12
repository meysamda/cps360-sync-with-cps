using Cps360.SyncWithCps.Presentation.HttpAdapters.ErrorHandling;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Cps360.SyncWithCps.Presentation.Init
{
    public static class Mvc
    {
        public static void AddCustomizedMvc(this IServiceCollection services, Action<ApiBehaviorOptions> setupApiBehaviorAction = null, Action<MvcOptions> setupMvcAction = null)
        {
            services.AddMvc()
                .AddCustomizedMvcOptions(setupMvcAction)
                .ConfigureCustomizedApiBehaviorOptions(setupApiBehaviorAction);
        }

        private static IMvcBuilder ConfigureCustomizedApiBehaviorOptions(this IMvcBuilder builder, Action<ApiBehaviorOptions> setupAction = null)
        {
            if (setupAction != null)
                builder.ConfigureApiBehaviorOptions(setupAction);

            builder.ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            return builder;
        }

        private static IMvcBuilder AddCustomizedMvcOptions(this IMvcBuilder builder, Action<MvcOptions> setupAction = null)
        {
            if (setupAction != null)
                builder.AddMvcOptions(setupAction);

            builder.AddMvcOptions(options =>
            {
                options.Filters.Add(new ModelStateValidationActionFilter());
            });

            return builder;
        }
    }
}
