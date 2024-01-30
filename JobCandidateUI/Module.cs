using JobCandidateUI.StartupTasks;
using Shared.Startup;
using System.Reflection;
using Shared.Extensions;

namespace JobCandidateUI
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

            RegisterStartupTasks(services);
        }

        private void RegisterStartupTasks(IServiceCollection services)
        {
            services.AddStartupTask<MongoDBStartupTask>();
            services.AddStartupTask<BrokerServiceStartupTask>();
            services.AddStartupTask<ResourceKeyResolverStartupTask>();
        }
    }
}
