using System;
using VideoOS.Platform;

namespace VideoOS.GisMapCommunication.Repository;

public class RepositoryClientManager : ClientManagerBase<IRepositoryClient>
{
	protected override IRepositoryClient CreateClient(ServerId serverId, TimeSpan requestTimeout, Uri uri)
	{
		return RepositoryClientFactory.Create(serverId, requestTimeout, uri);
	}
}
