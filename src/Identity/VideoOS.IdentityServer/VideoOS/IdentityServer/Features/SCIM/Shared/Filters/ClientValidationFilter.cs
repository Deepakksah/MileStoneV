using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using VideoOS.IdentityServer.Data;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Model.Result;

namespace VideoOS.IdentityServer.Features.SCIM.Shared.Filters;

public class ClientValidationFilter(ApplicationDbContext dbContext) : IEndpointFilter
{
	public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
	{
		HttpContext httpContext = context.HttpContext;
		string clientId = httpContext.User.Claims.FirstOrDefault((Claim c) => c.Type == "client_id")?.Value;
		if (string.IsNullOrWhiteSpace(clientId))
		{
			Error error = Error.Validation(400.ToString(), "Client ID is missing.");
			return ScimErrorMapper.ToScimErrorResult(error);
		}
		LoginProvider loginProvider = await dbContext.LoginProviders.FirstOrDefaultAsync((LoginProvider lp) => lp.ScimClients.Select((LoginProviderClient f) => f.ClientId).Contains(clientId) && lp.Enabled);
		if (loginProvider == null)
		{
			Error error2 = Error.Forbidden(403.ToString(), "Client does not have access to login provider.");
			return ScimErrorMapper.ToScimErrorResult(error2);
		}
		httpContext.Items["Scim.ProviderId"] = loginProvider.Id;
		httpContext.Items["Scim.ProviderDisplayName"] = loginProvider.DisplayName;
		return await next(context);
	}
}
