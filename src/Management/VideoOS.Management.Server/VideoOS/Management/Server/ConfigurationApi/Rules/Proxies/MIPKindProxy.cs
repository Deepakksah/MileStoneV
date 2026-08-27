using System;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public class MIPKindProxy : IMIPKindProxy
{
	public MIPKind GetKind(IGeminiRequestContext requestContext, Guid kind)
	{
		return MIPKind.GetKind(requestContext, kind);
	}
}
