using System.Security.Claims;

namespace VideoOS.IdentityServer.Common.ServiceModel;

public interface IOperationContextHelper
{
	string AuthorizationHeaderValue { get; }

	Claim[] CollectCurrentActionClaims(string action, string resource);
}
