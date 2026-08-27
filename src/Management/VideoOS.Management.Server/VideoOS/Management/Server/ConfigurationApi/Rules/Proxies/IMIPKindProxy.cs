using System;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public interface IMIPKindProxy
{
	MIPKind GetKind(IGeminiRequestContext requestContext, Guid kind);
}
