using System;
using VideoOS.Platform;

namespace VideoOS.GisMapCommunication.Building;

public class BuildingClientManager : ClientManagerBase<IBuildingClient>
{
	protected override IBuildingClient CreateClient(ServerId serverId, TimeSpan requestTimeout, Uri uri)
	{
		return BuildingClientFactory.Create(serverId, requestTimeout, uri);
	}
}
