using System;
using VideoOS.Platform;

namespace VideoOS.GisMapCommunication.Mip;

public class MipClientManager : ClientManagerBase<IMipClient>
{
	protected override IMipClient CreateClient(ServerId serverId, TimeSpan requestTimeout, Uri uri)
	{
		return MipClientFactory.Create(serverId, requestTimeout, uri);
	}
}
