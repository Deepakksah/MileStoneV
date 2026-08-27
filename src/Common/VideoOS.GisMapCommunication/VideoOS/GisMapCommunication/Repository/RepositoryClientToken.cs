using System;
using System.ServiceModel;
using VideoOS.GisMapCommunication.Repository.Client;
using VideoOS.Platform;

namespace VideoOS.GisMapCommunication.Repository;

public class RepositoryClientToken : RepositoryClientBase
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

	protected override WSMessageEncoding CurrentMessageEncoding => WSMessageEncoding.Mtom;

	public RepositoryClientToken(ServerId serverId, TimeSpan requestTimeout, Uri uri)
		: base(serverId, requestTimeout, uri)
	{
	}

	protected override GisRepositoryCommandClient CreateClient()
	{
		GisRepositoryCommandClient gisRepositoryCommandClient = new GisRepositoryCommandClient(CreateWSHttpBindingCredentials(), new EndpointAddress(base.FinalUri, CreateSpnIdentity(Name)));
		AddClientHeaderBehavior(gisRepositoryCommandClient);
		AbstractClient<GisRepositoryCommandClient, IGisRepositoryCommand>.EnsureLargeObjectGraphsAllowed(gisRepositoryCommandClient);
		return gisRepositoryCommandClient;
	}
}
