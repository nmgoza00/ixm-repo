
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace IXM.Web.DX.Services
{
    public class UserValidationProvider<TUser> : RevalidatingServerAuthenticationStateProvider where TUser : class
    {

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IdentityOptions _options;

        public UserValidationProvider(ILoggerFactory loggerFactory,
            IServiceScopeFactory scopeFactory,
            IOptions<IdentityOptions> options) : base (loggerFactory) 
            { 
            _scopeFactory = scopeFactory;
            _options = options.Value;
            }

        protected override TimeSpan RevalidationInterval => TimeSpan.FromSeconds(30);

        protected override async Task<bool> ValidateAuthenticationStateAsync(
                AuthenticationState authenticationState, CancellationToken cancellationToken)

        {

            var scope = _scopeFactory.CreateScope();
            try
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<TUser>>();
                return await ValidateSecurityStateAsync(userManager, authenticationState.User);
            }
            finally
            {
                if (scope is IAsyncDisposable asyncDisposable)
                {
                    await asyncDisposable.DisposeAsync();

                }
                else
                {
                    scope.Dispose();
                }

            }
        }

        private async Task<bool> ValidateSecurityStateAsync(UserManager<TUser> userManager, ClaimsPrincipal principal)
        {
            var user = await userManager.GetUserAsync(principal);
            if (user == null) 
            { 
                return false;
            } 
            else if (!userManager.SupportsUserSecurityStamp)
            {
                return true;

            }
            else
            {
                var prinicalStamp = principal.FindFirstValue(_options.ClaimsIdentity.SecurityStampClaimType);
                var userStamp = await userManager.GetSecurityStampAsync(user);
                return prinicalStamp == userStamp;
            }

        }




    }
}
