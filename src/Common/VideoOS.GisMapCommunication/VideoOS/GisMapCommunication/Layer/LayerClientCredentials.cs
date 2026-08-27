using System;
using System.ServiceModel;
using VideoOS.GisMapCommunication.Exceptions;
using VideoOS.GisMapCommunication.Layer.Client;
using VideoOS.Platform;
using VideoOS.Platform.Login;
using VideoOS.Platform.Util;

namespace VideoOS.GisMapCommunication.Layer;

public class LayerClientCredentials : LayerClientBase
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

	public LayerClientCredentials(ServerId serverId, TimeSpan requestTimeout, Uri uri)
		: base(serverId, requestTimeout, uri)
	{
	}

	protected override GisLayerCommandClient CreateClient()
	{
		GisLayerCommandClient gisLayerCommandClient = new GisLayerCommandClient(CreateWSHttpBindingCredentials(), new EndpointAddress(base.FinalUri, CreateSpnIdentity(SpnFactory.GetSpn(base.FinalUri))));
		LoginSettings loginSettings = LoginSettingsCache.GetLoginSettings(base.ServerId);
		if (loginSettings == null)
		{
			throw new LoginSettingsNotFoundException(base.ServerId);
		}
		if (loginSettings.IsOAuthIdentity)
		{
			throw new NotImplementedException("MAP: LayerClientCredential missing implementtion for OAuth");
		}
		gisLayerCommandClient.ClientCredentials.Windows.ClientCredential = loginSettings.NetworkCredential;
		AbstractClient<GisLayerCommandClient, IGisLayerCommand>.EnsureLargeObjectGraphsAllowed(gisLayerCommandClient);
		return gisLayerCommandClient;
	}
}
