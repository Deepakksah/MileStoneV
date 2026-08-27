using System;
using System.ServiceModel;
using VideoOS.GisMapCommunication.Map.Client;
using VideoOS.Platform;

namespace VideoOS.GisMapCommunication.Map;

public class MapClientToken : MapClientBase
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

	public MapClientToken(ServerId serverId, TimeSpan requestTimeout, Uri uri)
		: base(serverId, requestTimeout, uri)
	{
	}

	protected override GisMapCommandClient CreateClient()
	{
		GisMapCommandClient gisMapCommandClient = new GisMapCommandClient(CreateWSHttpBindingCredentials(), new EndpointAddress(base.FinalUri, CreateSpnIdentity(Name)));
		AddClientHeaderBehavior(gisMapCommandClient);
		AbstractClient<GisMapCommandClient, IGisMapCommand>.EnsureLargeObjectGraphsAllowed(gisMapCommandClient);
		return gisMapCommandClient;
	}
}
