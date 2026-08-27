using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Repositories;

namespace VideoOS.IdentityServer.Filters;

public class ForceLoginFilter(VmsUserManager userManager) : IAsyncAuthorizationFilter, IFilterMetadata
{
	public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
	{
		HttpContext httpContext = context.HttpContext;
		ClaimsPrincipal user = httpContext.User;
		IIdentity identity = user.Identity;
		if (identity != null && identity.IsAuthenticated)
		{
			VmsUser vmsUser = await userManager.GetUserAsync(user);
			if (vmsUser == null || !vmsUser.CanChangePassword)
			{
				context.Result = new RedirectToPageResult("/Account/Login/Index", null, new
				{
					area = "Identity"
				});
			}
		}
	}
}
