using System;
using System.ServiceModel;
using VideoOS.GisMapCommunication.Layer.Client;
using VideoOS.Platform;

namespace VideoOS.GisMapCommunication.Layer;

public class LayerClientToken : LayerClientBase
{
	protected override SecurityMode CurrentSecurityMode
	{
		get
		{
			if (!(base.Uri.Scheme == Uri.UriSchemeHttps))
			{
				return SecurityMode.None;
			}
			return SecurityMode.Transport;
		}
	}

	protected override WSMessageEncoding CurrentMessageEncoding => WSMessageEncoding.Text;

	public LayerClientToken(ServerId serverId, TimeSpan requestTimeout, Uri uri)
		: base(serverId, requestTimeout, uri)
	{
	}

	protected override GisLayerCommandClient CreateClient()
	{
		GisLayerCommandClient gisLayerCommandClient = new GisLayerCommandClient(CreateWSHttpBindingCredentials(), new EndpointAddress(base.FinalUri, CreateSpnIdentity(Name)));
		AddClientHeaderBehavior(gisLayerCommandClient);
		AbstractClient<GisLayerCommandClient, IGisLayerCommand>.EnsureLargeObjectGraphsAllowed(gisLayerCommandClient);
		return gisLayerCommandClient;
	}
}
