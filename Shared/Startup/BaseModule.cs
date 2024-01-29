using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Startup
{
    public abstract class BaseModule
    {
        public void Execute(IServiceCollection services)
        {
            ManualRegister(services);
            AutoRegister(services);
        }

        protected virtual void ManualRegister(IServiceCollection services)
        {
        }

        private void AutoRegister(IServiceCollection services)
        {
            var assemblies = GetAssembliesToScan();

            if (assemblies?.Any() != true)
                return;

            var namingConventions = GetNamingConventions();

            if (namingConventions?.Any() != true)
                return;

            var allTypes = assemblies.SelectMany(x => x.GetTypes());

            var interfaces = allTypes.Where(x => x.IsInterface && namingConventions.Any(xx => xx.IsMatch(x.Name)));

            if (interfaces?.Any() != true)
                return;

            foreach (var i in interfaces)
            {
                var implementation = allTypes.FirstOrDefault(x => !x.IsAbstract && !x.IsInterface && x.GetInterfaces()?.Any(xx => xx == i) == true);

                if (implementation != null)
                    services.AddSingleton(i, implementation);
            }
        }

        protected abstract List<Assembly> GetAssembliesToScan();

        protected virtual List<NamingConvention> GetNamingConventions()
        {
            return null;
        }
    }
}
