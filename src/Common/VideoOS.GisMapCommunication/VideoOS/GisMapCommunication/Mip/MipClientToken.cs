using System;
using System.ServiceModel;
using VideoOS.GisMapCommunication.Mip.Client;
using VideoOS.Platform;

namespace VideoOS.GisMapCommunication.Mip;

public class MipClientToken : MipClientBase
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

	public MipClientToken(ServerId serverId, TimeSpan requestTimeout, Uri uri)
		: base(serverId, requestTimeout, uri)
	{
	}

	protected override GisMipItemCommandClient CreateClient()
	{
		GisMipItemCommandClient gisMipItemCommandClient = new GisMipItemCommandClient(CreateWSHttpBindingCredentials(), new EndpointAddress(base.FinalUri, CreateSpnIdentity(Name)));
		AddClientHeaderBehavior(gisMipItemCommandClient);
		AbstractClient<GisMipItemCommandClient, IGisMipItemCommand>.EnsureLargeObjectGraphsAllowed(gisMipItemCommandClient);
		return gisMipItemCommandClient;
	}
}
