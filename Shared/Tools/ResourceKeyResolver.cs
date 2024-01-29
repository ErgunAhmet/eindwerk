using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Tools
{
    public static class ResourceKeyResolver
    {
        private static ResourceManager[] _resourceManagers;

        public static string Resolve(string resourceKey)
        {
            if (string.IsNullOrEmpty(resourceKey) || _resourceManagers?.Any() != true)
                return null;

            foreach (var resourceManager in _resourceManagers)
            {
                var text = resourceManager.GetString(resourceKey);

                if (!string.IsNullOrEmpty(text))
                    return text;
            }

            return null;
        }

        public static void SetResourceManagers(params ResourceManager[] resourceManagers)
        {
            _resourceManagers = resourceManagers;
        }
    }
}
