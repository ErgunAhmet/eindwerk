using IRepository;
using Shared.Startup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class Module : BaseModule
    {
        protected override List<Assembly> GetAssembliesToScan()
        {
            return new List<Assembly> { GetType().Assembly, typeof(ICandidateAccountRepository).Assembly };
        }

        protected override List<NamingConvention> GetNamingConventions()
        {
            return new List<NamingConvention>
            {
                new NamingConvention("I", "Repository")
            };
        }
    }
}
