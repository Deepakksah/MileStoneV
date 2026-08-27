using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using VideoOS.Common.EnvironmentServices;
using VideoOS.Installer.Interfaces.Content;
using VideoOS.Installer.Logging;
using VideoOS.Installer.RecorderPlugins;
using VideoOS.Mobile.Configuration;
using VideoOS.Mobile.Helpers;
using VideoOS.Mobile.InstallerWizard.Utilities;

namespace VideoOS.Mobile.InstallerWizard;

public class MobileServerCertificateSubscriber : CertificateSubscriberBase
{
	public override Guid Id => new Guid("1279E6B4-0078-4D0A-A017-8310E3250DC1");

	public MobileServerCertificateSubscriber()
		: base(LogManager.InnerLog)
	{
	}

	public override Dictionary<string, string> CollectParameters(CertificatePageContent content)
	{
		Dictionary<string, string> dictionary = base.CollectParameters(content);
		dictionary["HTTPSCERTIFICATEPASSWORD"] = ReadCertificatePasswordFromPreviousInstallationConfig();
		return dictionary;
	}

	public override CertificatePageContent GetContent()
	{
		CertificatePageContent certificatePageContent = new CertificatePageContent();
		ServerToServerCommunicationCertificateGroup serverToServerCommunicationCertificateGroup = new ServerToServerCommunicationCertificateGroup();
		certificatePageContent.ServerCertificateGroups.Add(serverToServerCommunicationCertificateGroup);
		certificatePageContent.ClientEndpoints.Add(new ClientEndpoint(ServerConfiguratorData.MobileServerClientEndpointId, "MobileServerClientEndPointDisplayName", "VideoOS.Mobile.CertificateRegistration.MobileServerRegisterClientRegistrar, VideoOS.Mobile.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89", "VideoOS.Mobile.CertificateRegistration.MobileServerSetManagementServerRegistrar, VideoOS.Mobile.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89", serverToServerCommunicationCertificateGroup.Id));
		certificatePageContent.ServerEndpoints.Add(new ServerEndpoint(ServerConfiguratorData.MobileServerEndpointId, "MobileServerEndpointDisplayName", ServerConfiguratorData.MobileServerCertificateGroupId, "VideoOS.Mobile.CertificateRegistration.MobileServerEndpointRegistrar, VideoOS.Mobile.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89", "VideoOS.Mobile.CertificateRegistration.MobileServerServiceRegistrar, VideoOS.Mobile.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89", null));
		LegacyCertificateThumbprintProvider legacyCertificateThumbprintProvider = new LegacyCertificateThumbprintProvider(new FileService(), new RegistryHelper(), new LogManagerWrapper());
		certificatePageContent.ServerCertificateGroups.Add(new MobileServerCommunicationGroup(legacyCertificateThumbprintProvider.GetMobileServerLegacyEncryption));
		return certificatePageContent;
	}

	private static string ReadCertificatePasswordFromPreviousInstallationConfig()
	{
		LogManager.Write("Starting ReadCertificatePasswordFromPrevousInstallationConfig()...", TraceEventType.Information);
		string text = null;
		InternalConfigBase internalConfigBase = null;
		IRegistryHelper registryInstance = new RegistryHelper().GetRegistryInstance();
		if (registryInstance.HasValue<string>("InstallationFolder", File.Exists))
		{
			LogManager.Write("Old installation exists...", TraceEventType.Information);
			text = registryInstance.Read("InstallationFolder");
			if (!string.IsNullOrWhiteSpace(text))
			{
				LogManager.Write($"Found installationPath = {text}", TraceEventType.Information);
				internalConfigBase = new InternalConfigBase(text);
				internalConfigBase.InitFromConfigurationFile();
			}
		}
		return internalConfigBase?.ServerSettings?.Identification?.CertificatePassword ?? "";
	}
}
