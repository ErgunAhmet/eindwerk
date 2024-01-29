using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Startup
{
    public class NamingConvention
    {
        public string StartsWith { get; private set; }
        public string EndsWith { get; private set; }

        public NamingConvention(string startsWith, string endsWith)
        {
            StartsWith = startsWith;
            EndsWith = endsWith;
        }

        public bool IsMatch(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;

            return name.StartsWith(StartsWith) && name.EndsWith(EndsWith);
        }
    }
}
