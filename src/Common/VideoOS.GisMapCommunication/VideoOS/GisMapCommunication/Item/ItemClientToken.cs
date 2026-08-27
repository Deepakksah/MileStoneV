using System;
using System.ServiceModel;
using VideoOS.GisMapCommunication.Item.Client;
using VideoOS.Platform;

namespace VideoOS.GisMapCommunication.Item;

public class ItemClientToken : ItemClientBase
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

	public ItemClientToken(ServerId serverId, TimeSpan requestTimeout, Uri uri)
		: base(serverId, requestTimeout, uri)
	{
	}

	protected override GisItemCommandClient CreateClient()
	{
		GisItemCommandClient gisItemCommandClient = new GisItemCommandClient(CreateWSHttpBindingCredentials(), new EndpointAddress(base.FinalUri, CreateSpnIdentity(Name)));
		AddClientHeaderBehavior(gisItemCommandClient);
		AbstractClient<GisItemCommandClient, IGisItemCommand>.EnsureLargeObjectGraphsAllowed(gisItemCommandClient);
		return gisItemCommandClient;
	}
}
