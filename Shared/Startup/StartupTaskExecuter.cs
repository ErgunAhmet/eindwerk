using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Startup
{
    public static class StartupTaskExecuter
    {
        public static async Task ExecuteAllAsync(IServiceProvider serviceProvider)
        {
            var startupTasks = serviceProvider.GetServices<IStartupTask>();

            if (startupTasks?.Any() != true)
            {
                return;
            }

            startupTasks = startupTasks.OrderBy(x => x.Order);

            foreach (var startupTask in startupTasks)
            {
                var name = startupTask.GetType().FullName;
                try
                {
                    await startupTask.ExecuteAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
