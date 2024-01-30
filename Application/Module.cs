using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Shared.Broker.DomainEvents;
using Shared.Startup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class Module : BaseModule
    {
        protected override List<Assembly> GetAssembliesToScan()
        {
            return new List<Assembly> { GetType().Assembly };
        }

        protected override void ManualRegister(IServiceCollection services)
        {
            base.ManualRegister(services);

            RegisterServices(services);
            RegisterExternalHandlers(services);
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<IDomainEventProcessor, DomainEventProcessor>();
        }

        private void RegisterExternalHandlers(IServiceCollection services)
        {
        }
    }
}
