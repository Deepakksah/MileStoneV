using System;

namespace VideoOS.Platform.AccessControl.Internal.Providers.Strategies;

public interface IGuidProviderStrategy
{
	Guid Create(Guid namespaceId, params string[] names);

	string GetIconKey(string externalIconKey);
}
