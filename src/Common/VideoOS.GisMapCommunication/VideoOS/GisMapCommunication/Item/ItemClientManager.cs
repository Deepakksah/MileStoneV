using System;
using VideoOS.Platform;

namespace VideoOS.GisMapCommunication.Item;

public class ItemClientManager : ClientManagerBase<IItemClient>
{
	protected override IItemClient CreateClient(ServerId serverId, TimeSpan requestTimeout, Uri uri)
	{
		return ItemClientFactory.Create(serverId, requestTimeout, uri);
	}
}
