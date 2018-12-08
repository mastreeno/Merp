using IdentityModel;
using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using Merp.Web.Auth.Models.AccountViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Merp.Web.Auth.WorkerServices
{
    public class AccountControllerWorkerServices
    {
        private readonly IIdentityServerInteractionService _interactionService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountControllerWorkerServices(IIdentityServerInteractionService interactionService, IHttpContextAccessor httpContextAccessor)
        {
            _interactionService = interactionService ?? throw new ArgumentNullException(nameof(interactionService));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<LogoutViewModel> GetLogoutViewModelAsync(string logoutId, bool isUserAuthenticated)
        {
            var viewModel = new LogoutViewModel
            {
                LogoutId = logoutId,
                ShowLogoutPrompt = true
            };

            if (!isUserAuthenticated)
            {
                viewModel.ShowLogoutPrompt = false;
                return viewModel;
            }

            var context = await _interactionService.GetLogoutContextAsync(logoutId);
            if (!context.ShowSignoutPrompt)
            {
                viewModel.ShowLogoutPrompt = false;
                return viewModel;
            }

            return viewModel;
        }

        public async Task<LoggedOutViewModel> GetLoggedOutViewModelAsync(string logoutId, ClaimsPrincipal user)
        {
            var logout = await _interactionService.GetLogoutContextAsync(logoutId);

            var viewModel = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = false,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            if (user.Identity.IsAuthenticated)
            {
                var identityProvider = user.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
                if (identityProvider != null && identityProvider != IdentityServerConstants.LocalIdentityProvider)
                {
                    var providerSupportsSignout = await _httpContextAccessor.HttpContext.GetSchemeSupportsSignOutAsync(identityProvider);
                    if (providerSupportsSignout)
                    {
                        if (viewModel.LogoutId == null)
                        {
                            viewModel.LogoutId = await _interactionService.CreateLogoutContextAsync();
                        }

                        viewModel.ExternalAuthenticationScheme = identityProvider;
                    }
                }
            }

            return viewModel;
        }
    }
}
