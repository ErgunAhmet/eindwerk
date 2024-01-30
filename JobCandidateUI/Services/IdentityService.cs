using Microsoft.AspNetCore.Components.Authorization;

namespace JobCandidateUI.Services
{
    public class IdentityService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public IdentityService(AuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<System.Security.Claims.ClaimsPrincipal> GetUserIdentity()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                return user;
            }

            return null;
        }
    }
}
