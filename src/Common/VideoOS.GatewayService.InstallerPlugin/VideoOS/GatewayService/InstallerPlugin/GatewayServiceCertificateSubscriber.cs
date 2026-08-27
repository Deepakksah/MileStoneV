using System;
using VideoOS.Installer.Interfaces.Content;
using VideoOS.Installer.Logging;

namespace VideoOS.GatewayService.InstallerPlugin;

public class GatewayServiceCertificateSubscriber : CertificateSubscriberBase
{
	public override Guid Id => new Guid("8321C23B-3889-4242-A1AA-4FF9B73C0ED6");

	public GatewayServiceCertificateSubscriber()
		: base(LogManager.InnerLog)
	{
	}

	public override CertificatePageContent GetContent()
	{
		return new CertificatePageContent
		{
			ServerEndpoints = 
			{
				new ServerEndpoint(ServerConfiguratorData.GatewayServiceId, "ApiGatewayServiceEndPointDisplayName", ServerCertificateGroup.ServerToServerCommunicationGroupId, "VideoOS.GatewayService.Registrars.GatewayServiceEndpointRegistrar, VideoOS.GatewayService.Registrars, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89", "VideoOS.GatewayService.Registrars.GatewayServiceRegistrationUpdateRegistrar, VideoOS.GatewayService.Registrars, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89", null)
			},
			ClientEndpoints = 
			{
				new ClientEndpoint(ServerConfiguratorData.GatewayServiceClientId, "ApiGatewayServiceClientEndPointDisplayName", "VideoOS.GatewayService.Registrars.GatewayServiceClientEndpointRegistrar, VideoOS.GatewayService.Registrars, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89", "VideoOS.GatewayService.Registrars.GatewayServiceSetManagementServerRegistrar, VideoOS.GatewayService.Registrars, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89", ServerCertificateGroup.ServerToServerCommunicationGroupId)
			}
		};
	}
}
