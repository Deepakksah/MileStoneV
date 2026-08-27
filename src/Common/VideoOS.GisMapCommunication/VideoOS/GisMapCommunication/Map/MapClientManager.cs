using System;
using VideoOS.Platform;

namespace VideoOS.GisMapCommunication.Map;

public class MapClientManager : ClientManagerBase<IMapClient>
{
	protected override IMapClient CreateClient(ServerId serverId, TimeSpan requestTimeout, Uri uri)
	{
		return MapClientFactory.Create(serverId, requestTimeout, uri);
	}
}
