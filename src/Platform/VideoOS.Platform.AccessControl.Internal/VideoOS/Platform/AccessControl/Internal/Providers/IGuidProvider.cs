using System.Collections.Generic;
using VideoOS.Platform.AccessControl.Internal.Providers.Strategies;

namespace VideoOS.Platform.AccessControl.Internal.Providers;

public interface IGuidProvider
{
	HashSet<IGuidProviderStrategy> GuidProviderStrategies { get; }

	void SetStrategies(HashSet<IGuidProviderStrategy> guidProviderStrategies);
}
