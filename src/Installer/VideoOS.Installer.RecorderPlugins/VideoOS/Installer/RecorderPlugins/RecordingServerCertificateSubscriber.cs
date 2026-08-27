using System;
using VideoOS.Common.EnvironmentServices;
using VideoOS.Common.Integration;
using VideoOS.Installer.Interfaces.Content;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.RecorderPlugins;

public class RecordingServerCertificateSubscriber : CertificateSubscriberBase
{
	public override Guid Id => new Guid("0028D315-292F-492A-8A6C-B3781BE36128");

	public RecordingServerCertificateSubscriber()
		: base(LogManager.InnerLog)
	{
	}

	public override CertificatePageContent GetContent()
	{
		string text = base.Parameters["ISFAILOVER"];
		bool num = text != null && string.Equals(text, bool.TrueString, StringComparison.OrdinalIgnoreCase);
		CertificatePageContent certificatePageContent = new CertificatePageContent
		{
			ClientEndpoints = 
			{
				new ClientEndpoint(ServerConfiguratorData.RecorderClientId, "RecordingClientEndPointDisplayName", "VideoOS.Recorder.CertificateRegistration.RecorderClientEndpointRegistrar, VideoOS.Recorder.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89", "VideoOS.Recorder.CertificateRegistration.RecorderSetManagementServerRegistrar, VideoOS.Recorder.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89", ServerCertificateGroup.ServerToServerCommunicationGroupId)
			},
			ServerEndpoints = 
			{
				new ServerEndpoint(ServerConfiguratorData.RecorderServerId, "RecordingServerEndPointDisplayName", ServerCertificateGroup.ServerToServerCommunicationGroupId, "VideoOS.Recorder.CertificateRegistration.RecorderServerEndpointRegistrar, VideoOS.Recorder.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89", "VideoOS.Recorder.CertificateRegistration.RecorderServerRegisterService, VideoOS.Recorder.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89", null),
				new ServerEndpoint(ServerConfiguratorData.ImageServerServerId, "ImageServerEndPointDisplayName", ServerConfiguratorData.ImageServerCertificateGroupId, "VideoOS.Recorder.CertificateRegistration.ImageServerEndpointRegistrar, VideoOS.Recorder.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89")
			}
		};
		if (num)
		{
			certificatePageContent.ServerEndpoints.Add(new ServerEndpoint(ServerConfiguratorData.FailoverServerId, "FailoverServerEndPointDisplayName", ServerCertificateGroup.ServerToServerCommunicationGroupId, "VideoOS.Failover.CertificateRegistration.FailoverServerEndpointRegistrar, VideoOS.Failover.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89"));
			certificatePageContent.ClientEndpoints.Add(new ClientEndpoint(ServerConfiguratorData.FailoverClientId, "FailoverClientEndPointDisplayName", "VideoOS.Failover.CertificateRegistration.FailoverClientEndpointRegistrar, VideoOS.Failover.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89", "VideoOS.Failover.CertificateRegistration.FailoverSetManagementServerRegistrar, VideoOS.Failover.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89", ServerCertificateGroup.ServerToServerCommunicationGroupId));
		}
		LegacyCertificateThumbprintProvider legacyCertificateThumbprintProvider = new LegacyCertificateThumbprintProvider(new FileService(), OemResources.Instance);
		certificatePageContent.ServerCertificateGroups.Add(new ServerToServerCommunicationCertificateGroup(legacyCertificateThumbprintProvider.GetRecordingServerLegacyEncryption));
		certificatePageContent.ServerCertificateGroups.Add(new ImageServerCommunicationGroup(legacyCertificateThumbprintProvider.GetImageServerLegacyEncryption));
		return certificatePageContent;
	}
}
