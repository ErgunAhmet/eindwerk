using Microsoft.Extensions.DependencyInjection;
using Shared.Startup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAgent
{
    public class Module : BaseModule
    {
        protected override List<Assembly> GetAssembliesToScan()
        {
            return new List<Assembly>
            {
                GetType().Assembly,
                //typeof().Assembly
            };
        }

        protected override void ManualRegister(IServiceCollection services)
        {
            base.ManualRegister(services);
        }

        
    }
}
