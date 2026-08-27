using System;
using System.ServiceModel;
using VideoOS.GisMapCommunication.Exceptions;
using VideoOS.GisMapCommunication.Mip.Client;
using VideoOS.Platform;
using VideoOS.Platform.Login;
using VideoOS.Platform.Util;

namespace VideoOS.GisMapCommunication.Mip;

public class MipClientCredentials : MipClientBase
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

	protected override WSMessageEncoding CurrentMessageEncoding => WSMessageEncoding.Text;

	public MipClientCredentials(ServerId serverId, TimeSpan requestTimeout, Uri uri)
		: base(serverId, requestTimeout, uri)
	{
	}

	protected override GisMipItemCommandClient CreateClient()
	{
		GisMipItemCommandClient gisMipItemCommandClient = new GisMipItemCommandClient(CreateWSHttpBindingCredentials(), new EndpointAddress(base.FinalUri, CreateSpnIdentity(SpnFactory.GetSpn(base.FinalUri))));
		LoginSettings loginSettings = LoginSettingsCache.GetLoginSettings(base.ServerId);
		if (loginSettings == null)
		{
			throw new LoginSettingsNotFoundException(base.ServerId);
		}
		if (loginSettings.IsOAuthIdentity)
		{
			throw new NotImplementedException("MAP: MipClientCredential missing implementtion for OAuth");
		}
		gisMipItemCommandClient.ClientCredentials.Windows.ClientCredential = loginSettings.NetworkCredential;
		AbstractClient<GisMipItemCommandClient, IGisMipItemCommand>.EnsureLargeObjectGraphsAllowed(gisMipItemCommandClient);
		return gisMipItemCommandClient;
	}
}
