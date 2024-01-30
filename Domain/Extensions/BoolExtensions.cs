using Domain.Resources;

namespace Domain.Extensions
{
    public static class BoolExtensions
    {
        public static string GetResourceKey(this bool b)
        {
            return b ? nameof(PropertyNameResource.True) : nameof(PropertyNameResource.False);
        }
    }
}
