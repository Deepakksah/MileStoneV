using System;
using VideoOS.Platform;

namespace VideoOS.GisMapCommunication.Layer;

public class LayerClientManager : ClientManagerBase<ILayerClient>
{
	protected override ILayerClient CreateClient(ServerId serverId, TimeSpan requestTimeout, Uri uri)
	{
		return LayerClientFactory.Create(serverId, requestTimeout, uri);
	}
}
