using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Startup
{
    public interface IStartupTask
    {
        Task ExecuteAsync();
        int Order { get; }
    }
}
