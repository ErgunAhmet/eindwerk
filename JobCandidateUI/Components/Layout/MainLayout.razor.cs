using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace JobCandidateUI.Components.Layout
{
    public partial class MainLayout : LayoutComponentBase
    {
        [Inject]
        private IMediator _mediator { get; set; }

        [Inject]
        private AuthenticationStateProvider _authenticationStateProvider { get; set; }
        private string username;

        protected override async Task OnInitializedAsync()
        {
            if (_authenticationStateProvider is not null)
            {
                var provider = await _authenticationStateProvider.GetAuthenticationStateAsync();
                //var objectId = provider[""]
                // Assuming user is the ClaimsPrincipal obtained from AuthenticationState.User
                var userId = provider.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
                var authTime = provider.User.FindFirst("auth_time")?.Value;
                var objectId = provider.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;
                var givenName = provider.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")?.Value;
                var surname = provider.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname")?.Value;
                var email = provider.User.FindFirst("emails")?.Value;

                username = givenName;
            }


        }
    }
}
