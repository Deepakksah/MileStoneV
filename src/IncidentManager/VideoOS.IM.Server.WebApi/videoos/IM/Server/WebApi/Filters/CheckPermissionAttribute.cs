using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using VideoOS.IM.Server.Service.OidcTokens;
using VideoOS.IM.Server.Service.WindowsRegistries;
using VideoOS.IM.Server.WebApi.Utils;

namespace VideoOS.IM.Server.WebApi.Filters;

public class CheckPermissionAttribute : AuthorizeAttribute
{
	private static readonly IWinRegistryService WinRegistryService = new WinRegistryService();

	private readonly string _actionId;

	private readonly string _kind;

	public CheckPermissionAttribute(string actionId, string kind)
	{
		_actionId = actionId;
		_kind = kind;
	}

	public override async Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
	{
		string accessToken = await S2STokenService.Instance.GetAccessTokenAsync();
		string text = (actionContext.RequestContext.Principal as ClaimsPrincipal)?.FindFirst("MigrateSid")?.Value;
		if (string.IsNullOrEmpty(text))
		{
			text = (actionContext.RequestContext.Principal as ClaimsPrincipal)?.FindFirst("sub")?.Value;
			if (string.IsNullOrEmpty(text))
			{
				throw new InvalidOperationException("Claim 'MigrateSid'/'sub'(subject) not found");
			}
		}
		if (!(await HasPermissionAsync(accessToken, text)))
		{
			HttpResponseMessage val = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
			val.ReasonPhrase = "User does not have sufficient permissions to complete the operation";
			actionContext.Response = val;
		}
	}

	private async Task<bool> HasPermissionAsync(string accessToken, string identity)
	{
		string text = WinRegistryService.ReadRegistryValue("ManagementServerAddress", string.Empty);
		if (string.IsNullOrEmpty(text))
		{
			throw new InvalidOperationException("Management Server address not found in the registry");
		}
		SecurityServiceClient client = SecurityServiceHelper.CreateClient(new Uri(text), accessToken);
		try
		{
			return (await client.HasPermissionByActionListAsync(_kind, identity, new string[1] { _actionId }, _kind)).Single();
		}
		finally
		{
			if (client.State == CommunicationState.Opened || client.State == CommunicationState.Created)
			{
				client.Close();
			}
			else
			{
				client.Abort();
			}
		}
	}
}
