using IInfrasctructure.MongoDB;
using Infrastructure.MongoDB;
using Microsoft.Extensions.DependencyInjection;
using Shared.Startup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class Module : BaseModule
    {
        protected override List<Assembly> GetAssembliesToScan()
        {
            return new List<Assembly>
            {
                GetType().Assembly
            };
        }

        protected override void ManualRegister(IServiceCollection services)
        {
            base.ManualRegister(services);

            RegisterMongo(services);
        }

        private void RegisterMongo(IServiceCollection services)
        {
            services.AddSingleton<IMongoDBService, MongoDBService>();
        }
    }
}
