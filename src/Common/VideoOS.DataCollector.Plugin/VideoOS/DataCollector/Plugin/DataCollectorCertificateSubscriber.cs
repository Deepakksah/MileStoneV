using System;
using VideoOS.Installer.Interfaces.Content;
using VideoOS.Installer.Logging;

namespace VideoOS.DataCollector.Plugin;

public class DataCollectorCertificateSubscriber : CertificateSubscriberBase
{
	public override Guid Id => new Guid("4382E370-13FD-4932-9A7E-F6CC87E22120");

	public DataCollectorCertificateSubscriber()
		: base(LogManager.InnerLog)
	{
	}

	public override CertificatePageContent GetContent()
	{
		return new CertificatePageContent
		{
			ClientEndpoints = 
			{
				new ClientEndpoint(DataCollectorData.DataCollectorClientId, "DataCollectorClientEndPointDisplayName", "VideoOS.DataCollector.CertificateRegistration.DataCollectorClientEndpointRegistrar, VideoOS.DataCollector.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89", "VideoOS.DataCollector.CertificateRegistration.DataCollectorSetManagementServerRegistrar, VideoOS.DataCollector.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89", ServerCertificateGroup.ServerToServerCommunicationGroupId)
			},
			ServerEndpoints = 
			{
				new ServerEndpoint(DataCollectorData.DataCollectorServerId, "DataCollectorServerEndPointDisplayName", ServerCertificateGroup.ServerToServerCommunicationGroupId, "VideoOS.DataCollector.CertificateRegistration.DataCollectorServerEndpointRegistrar, VideoOS.DataCollector.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89")
			}
		};
	}
}
