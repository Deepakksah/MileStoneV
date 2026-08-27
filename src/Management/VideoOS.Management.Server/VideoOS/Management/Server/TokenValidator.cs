using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration.Security;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Management.Server;

public class TokenValidator : TokenHandlerBase
{
	private static readonly ILogger Logger = VideoOS.Utilities.Telemetry.Telemetry.GetILogger("SecurityTokenHandler");

	private IGeminiRequestContext requestContext;

	public TokenValidator(IGeminiRequestContext requestContext)
		: base(Logger)
	{
		this.requestContext = requestContext;
	}

	protected override void AddToken(SecurityToken securityToken)
	{
	}

	protected override SecurityToken FindToken(string tokenId)
	{
		using ISecurityTokenComponent securityTokenComponent = requestContext.CreateSecurityTokenComponent();
		return securityTokenComponent.QueryToken(tokenId);
	}
}
