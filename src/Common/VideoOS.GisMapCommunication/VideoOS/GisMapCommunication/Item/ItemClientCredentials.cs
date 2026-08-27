using System;
using System.ServiceModel;
using VideoOS.GisMapCommunication.Exceptions;
using VideoOS.GisMapCommunication.Item.Client;
using VideoOS.Platform;
using VideoOS.Platform.Login;
using VideoOS.Platform.Util;

namespace VideoOS.GisMapCommunication.Item;

public class ItemClientCredentials : ItemClientBase
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

	public ItemClientCredentials(ServerId serverId, TimeSpan requestTimeout, Uri uri)
		: base(serverId, requestTimeout, uri)
	{
	}

	protected override GisItemCommandClient CreateClient()
	{
		GisItemCommandClient gisItemCommandClient = new GisItemCommandClient(CreateWSHttpBindingCredentials(), new EndpointAddress(base.FinalUri, CreateSpnIdentity(SpnFactory.GetSpn(base.FinalUri))));
		LoginSettings loginSettings = LoginSettingsCache.GetLoginSettings(base.ServerId);
		if (loginSettings == null)
		{
			throw new LoginSettingsNotFoundException(base.ServerId);
		}
		if (loginSettings.IsOAuthIdentity)
		{
			throw new NotImplementedException("MAP: ItemClientCredential missing implementtion for OAuth");
		}
		gisItemCommandClient.ClientCredentials.Windows.ClientCredential = loginSettings.NetworkCredential;
		AbstractClient<GisItemCommandClient, IGisItemCommand>.EnsureLargeObjectGraphsAllowed(gisItemCommandClient);
		return gisItemCommandClient;
	}
}
