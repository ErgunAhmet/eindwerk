using Microsoft.Extensions.DependencyInjection;
using Shared.Startup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStartupTask<T>(this IServiceCollection services) where T : class, IStartupTask
        {
            services.AddTransient<IStartupTask, T>();
            return services;
        }
    }
}
