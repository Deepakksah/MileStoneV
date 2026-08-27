using System;
using System.ServiceModel;
using VideoOS.GisMapCommunication.Exceptions;
using VideoOS.GisMapCommunication.Repository.Client;
using VideoOS.Platform;
using VideoOS.Platform.Login;
using VideoOS.Platform.Util;

namespace VideoOS.GisMapCommunication.Repository;

public class RepositoryClientCredentials : RepositoryClientBase
{
	protected override SecurityMode CurrentSecurityMode
	{
		get
		{
			if (!(base.Uri.Scheme == Uri.UriSchemeHttps))
			{
				return SecurityMode.Message;
			}
			return SecurityMode.TransportWithMessageCredential;
		}
	}

	protected override WSMessageEncoding CurrentMessageEncoding => WSMessageEncoding.Mtom;

	public RepositoryClientCredentials(ServerId serverId, TimeSpan requestTimeout, Uri uri)
		: base(serverId, requestTimeout, uri)
	{
	}

	protected override GisRepositoryCommandClient CreateClient()
	{
		GisRepositoryCommandClient gisRepositoryCommandClient = new GisRepositoryCommandClient(CreateWSHttpBindingCredentials(), new EndpointAddress(base.FinalUri, CreateSpnIdentity(SpnFactory.GetSpn(base.FinalUri))));
		LoginSettings loginSettings = LoginSettingsCache.GetLoginSettings(base.ServerId);
		if (loginSettings == null)
		{
			throw new LoginSettingsNotFoundException(base.ServerId);
		}
		if (loginSettings.IsOAuthIdentity)
		{
			throw new NotImplementedException("MAP: RepositoryClientCredential missing implementtion for OAuth");
		}
		gisRepositoryCommandClient.ClientCredentials.Windows.ClientCredential = loginSettings.NetworkCredential;
		AbstractClient<GisRepositoryCommandClient, IGisRepositoryCommand>.EnsureLargeObjectGraphsAllowed(gisRepositoryCommandClient);
		return gisRepositoryCommandClient;
	}
}
