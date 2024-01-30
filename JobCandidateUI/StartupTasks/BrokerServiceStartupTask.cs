using Shared.Broker;
using Shared.Startup;

namespace JobCandidateUI.StartupTasks
{
    public class BrokerServiceStartupTask : IStartupTask
    {
        private readonly IServiceProvider _serviceProvider;

        public BrokerServiceStartupTask(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public int Order => 2;

        public async Task ExecuteAsync()
        {
            await BrokerService.SetupAsync(_serviceProvider);
        }
    }
}
