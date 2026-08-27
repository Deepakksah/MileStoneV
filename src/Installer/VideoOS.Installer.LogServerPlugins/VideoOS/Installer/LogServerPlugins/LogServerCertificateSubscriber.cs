using System;
using VideoOS.Installer.Interfaces.Content;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.LogServerPlugins;

public class LogServerCertificateSubscriber : CertificateSubscriberBase
{
	public override Guid Id => new Guid("895951AE-0B0D-414A-AEA6-CC846B74C238");

	public LogServerCertificateSubscriber()
		: base(LogManager.InnerLog)
	{
	}

	public override CertificatePageContent GetContent()
	{
		return new CertificatePageContent
		{
			ServerEndpoints = 
			{
				new ServerEndpoint(ServerConfiguratorData.LogServerId, "LogServerEndPointDisplayName", ServerCertificateGroup.ServerToServerCommunicationGroupId, "VideoOS.LogServer.Registrars.LogServerCertificateRegistrar, VideoOS.LogServer.Registrars, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89", "VideoOS.LogServer.Registrars.LogServerRegisterServiceRegistrar, VideoOS.LogServer.Registrars, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89", null),
				new ServerEndpoint(ServerConfiguratorData.LegacyLogServerId, "LegacyLogServerEndPointDisplayName", Guid.Empty, null, "VideoOS.LogServer.Registrars.LegacyLogServerRegisterServiceRegistrar, VideoOS.LogServer.Registrars, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89", null)
			},
			ClientEndpoints = 
			{
				new ClientEndpoint(ServerConfiguratorData.LogServerClientId, "LogServerClientEndPointDisplayName", null, "VideoOS.LogServer.Registrars.LogServerSetManagementServerRegistrar, VideoOS.LogServer.Registrars, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89", Guid.Empty)
			}
		};
	}
}
