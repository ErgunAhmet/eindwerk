using IInfrasctructure.MongoDB;
using IRepository;
using Shared.Startup;

namespace JobCandidateUI.StartupTasks
{
    public class MongoDBStartupTask : IStartupTask
    {
        private readonly IMongoDBService _mongoDBService;

        public MongoDBStartupTask(IMongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public int Order => 100;

        public async Task ExecuteAsync()
        {
            await _mongoDBService.SetupAsync(typeof(ICandidateAccountRepository).Assembly, typeof(ICandidateAccountRepository).Assembly);
        }
    }
}
