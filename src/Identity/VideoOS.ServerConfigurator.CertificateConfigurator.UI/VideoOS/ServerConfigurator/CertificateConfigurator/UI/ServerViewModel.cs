using System;

namespace VideoOS.ServerConfigurator.CertificateConfigurator.UI;

public class ServerViewModel
{
	public string DisplayName { get; private set; }

	public Guid ServerEndpointId { get; private set; }

	public ServerViewModel(string friendlyName, Guid serverEndpointId)
	{
		DisplayName = friendlyName;
		ServerEndpointId = serverEndpointId;
	}
}
