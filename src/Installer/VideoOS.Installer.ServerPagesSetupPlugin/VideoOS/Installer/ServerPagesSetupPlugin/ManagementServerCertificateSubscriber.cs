using System;
using System.Collections.Generic;
using VideoOS.Installer.Interfaces.Content;
using VideoOS.Installer.ServerConfiguratorPlugin;

namespace VideoOS.Installer.ServerPagesSetupPlugin;

public class ManagementServerCertificateSubscriber : IWizardCommonPageSubscription<CertificatePageContent>
{
	public Guid Id => new Guid("B905D253-9489-4EE0-BC23-576214E813DE");

	public bool BeforeActivate(Dictionary<string, string> parameters)
	{
		if (parameters == null)
		{
			throw new ArgumentNullException("parameters");
		}
		return true;
	}

	public Dictionary<string, string> CollectParameters(CertificatePageContent content)
	{
		return new Dictionary<string, string>();
	}

	public CertificatePageContent GetContent()
	{
		CertificatePageContent certificatePageContent = new CertificatePageContent();
		certificatePageContent.ServerCertificateGroups.Add(new ServerToServerCommunicationCertificateGroup(LegacyEncryptionProvider));
		certificatePageContent.ClientEndpoints.Add(new ClientEndpoint(ServerConfiguratorData.ManagementServerClientId, "ManagementClientEndPointDisplayName", "VideoOS.Server.CertificateRegistration.ManagementServerClientEndpointRegistrar, VideoOS.Server.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89", null, ServerCertificateGroup.ServerToServerCommunicationGroupId));
		certificatePageContent.ServerEndpoints.Add(new ServerEndpoint(ServerConfiguratorData.ManagementServerId, "ManagementServerEndPointDisplayName", ServerCertificateGroup.ServerToServerCommunicationGroupId, "VideoOS.Server.CertificateRegistration.ManagementServerEndpointRegistrar, VideoOS.Server.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89", null, null));
		certificatePageContent.SystemConfigurationElements.Add(new SystemConfigurationElement(ServerConfiguratorData.ManagementServerConfigurationElementId, "ManagementServerSystemConfigurationDisplayName", ConfigurationPurpose.ChangeHostName, SystemFeature.IsManagementServer, "VideoOS.Server.CertificateRegistration.ManagementServerSystemConfigurationRegistrar, VideoOS.Server.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89"));
		certificatePageContent.ServerEndpoints.Add(new ServerEndpoint(ServerConfiguratorData.ReportServerId, "ReportServerEndPointDisplayName", ServerCertificateGroup.ServerToServerCommunicationGroupId, "VideoOS.Server.CertificateRegistration.ReportServerEndpointRegistrar, VideoOS.Server.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89", "VideoOS.Server.CertificateRegistration.ReportServerRegisterService, VideoOS.Server.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89", null));
		certificatePageContent.ServerEndpoints.Add(new ServerEndpoint(ServerConfiguratorData.IncidentManagerServerId, "IncidentManagerServerEndPointDisplayName", ServerCertificateGroup.ServerToServerCommunicationGroupId, "VideoOS.IM.Server.Registrars.IncidentManagerRegisterEndpointRegistrar, VideoOS.IM.Server.Registrars, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89", "VideoOS.IM.Server.Registrars.IncidentManagerServiceRegistrationUpdateRegistrar, VideoOS.IM.Server.Registrars, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89", null));
		certificatePageContent.ClientEndpoints.Add(new ClientEndpoint(ServerConfiguratorData.DownloadManagerClientId, "DownloadManagerClientEndPointDisplayName", null, ServerConfiguratorData.DownloadManagerSetManagementServerImplementationType, ServerCertificateGroup.ServerToServerCommunicationGroupId));
		certificatePageContent.ClientEndpoints.Add(new ClientEndpoint(ServerConfiguratorData.IncidentManagerClientId, "IncidentManagerServerClientEndPointDisplayName", "VideoOS.IM.Server.Registrars.IncidentManagerClientEndpointRegistrar, VideoOS.IM.Server.Registrars, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89", "VideoOS.IM.Server.Registrars.IncidentManagerSetManagementServerRegistrar, VideoOS.IM.Server.Registrars, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89", ServerCertificateGroup.ServerToServerCommunicationGroupId));
		return certificatePageContent;
	}

	private LegacyEncryption LegacyEncryptionProvider()
	{
		return ServerConfigurator.GetManagementServerLegacyEncryption();
	}
}
