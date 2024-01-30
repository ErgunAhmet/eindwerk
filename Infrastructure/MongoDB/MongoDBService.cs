using Common;
using IInfrasctructure.MongoDB;
using Shared.Validation;
using System;
using Domain.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.MongoDB
{
    public class MongoDBService : IMongoDBService
    {
        private readonly IServiceProvider _serviceProvider;

        public MongoDBService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task SetupAsync(params Assembly[] assembliesToScan)
        {
            Guard.HasItems(assembliesToScan, nameof(assembliesToScan), nameof(ErrorCodeResource.Property_HasNoItems));

            var types = assembliesToScan.SelectMany(a => a.GetTypes());

            var collecionType = typeof(IMongoDBBaseRepository);
            var typesToIgnpore = new List<Type>
            {
                collecionType,
                typeof(IMongoDBReadRepository<>),
                typeof(IMongoDBReadWriteRepository<>)
            };

            var collections = types.Where(t => collecionType.IsAssignableFrom(t) && t.IsInterface && !typesToIgnpore.Any(x => x == t));

            if (collections?.Any() == true)
            {
                var tasks = new List<Task>();

                foreach (var repository in collections)
                {
                    tasks.Add(SetupColectionAsync(repository));
                }

                await Task.WhenAll(tasks);
            }
        }

        private async Task SetupColectionAsync(Type repositoryType)
        {
            var service = _serviceProvider.GetService(repositoryType);

            if (service is IMongoDBBaseRepository repository)
            {
                await repository.SetupAsync(AppSettings.Instance.Mongo.ConnectionString);
            }
            else
                throw new Exception($"{repositoryType.FullName} does not inherit from {typeof(IMongoDBBaseRepository).FullName}");
        }
    }
}
