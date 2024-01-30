using Domain.Resources;
using Shared.Startup;
using Shared.Tools;

namespace JobCandidateUI.StartupTasks
{
    public class ResourceKeyResolverStartupTask : IStartupTask
    {
        public int Order => 2;

        public Task ExecuteAsync()
        {
            ResourceKeyResolver.SetResourceManagers(ErrorCodeResource.ResourceManager, PropertyNameResource.ResourceManager);
            return Task.CompletedTask;
        }
    }
}
