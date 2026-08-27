using System.Collections.Generic;
using System.IO;
using System.Management.Automation.Runspaces;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;

namespace VideoOS.Server.Failover.Configurator;

public class PrimaryNodeConfigurationBuilder : IConfigurationBuilder
{
	public bool EnableHttps { get; set; }

	public string ConfigurationScriptsPath { get; set; }

	public string SafekitPath { get; set; }

	public string PrimaryNodeAddress { get; set; }

	public string SecondaryNodeAddress { get; set; }

	public string PrimaryNodeName { get; set; }

	public string SecondaryNodeName { get; set; }

	public string HttpsPassword { get; set; }

	public string ModuleName { get; set; }

	public string SqlServerPathData { get; set; }

	public string SqlServerPathLog { get; set; }

	public string VirtualIp { get; set; }

	public string ServerConfigPath { get; set; }

	public string LicenseFilePath { get; set; }

	public string DesktopShortcutIconName { get; set; }

	public string ServerConfiguratorPath { get; set; }

	public string IdpAddress { get; set; }

	public string WebConsolePassword { get; set; }

	public string DataCollectorConfigFilePath { get; set; }

	public IProtectionProvider ProtectionProvider { get; set; }

	private Runspace Runspace { get; }

	private ILogger Log { get; }

	public PrimaryNodeConfigurationBuilder(Runspace runspace, ILogger log)
	{
		Runspace = runspace;
		Log = log;
	}

	public IEnumerable<IConfigurationStep> Build()
	{
		yield return new SetLocation(Runspace, Log)
		{
			Path = ConfigurationScriptsPath
		};
		yield return new StartSafekitServices(Runspace, Log);
		yield return new SetLocation(Runspace, Log)
		{
			Path = ConfigurationScriptsPath
		};
		yield return new ConfigureFirewall(Runspace, Log)
		{
			PrivateSafekitBinaryFolderPath = Path.Combine(SafekitPath, "private\\bin")
		};
		yield return new SetLocation(Runspace, Log)
		{
			Path = ConfigurationScriptsPath
		};
		yield return new ConfigureWebServer(Runspace, Log)
		{
			PrivateSafekitBinaryFolderPath = Path.Combine(SafekitPath, "private\\bin"),
			WebConsolePassword = WebConsolePassword,
			ProtectionProvider = ProtectionProvider,
			ServerConfigPath = ServerConfigPath
		};
		if (EnableHttps)
		{
			yield return new SetLocation(Runspace, Log)
			{
				Path = ConfigurationScriptsPath
			};
			yield return new ConfigureHttps(Runspace, Log)
			{
				SafekitPath = SafekitPath,
				PrimaryNodeAddress = PrimaryNodeAddress,
				SecondaryNodeAddress = SecondaryNodeAddress,
				HttpsPassword = HttpsPassword
			};
		}
		yield return new SetLocation(Runspace, Log)
		{
			Path = ConfigurationScriptsPath
		};
		yield return new ConfigureServices(Runspace, Log);
		yield return new SetLocation(Runspace, Log)
		{
			Path = ConfigurationScriptsPath
		};
		yield return new ConfigureCluster(Runspace, Log)
		{
			SafekitPath = SafekitPath,
			PrimaryNodeAddress = PrimaryNodeAddress,
			SecondaryNodeAddress = SecondaryNodeAddress,
			PrimaryNodeName = PrimaryNodeName,
			SecondaryNodeName = SecondaryNodeName,
			ClusterXmlPath = Path.Combine(ConfigurationScriptsPath, "cluster.xml")
		};
		yield return new SetLocation(Runspace, Log)
		{
			Path = ConfigurationScriptsPath
		};
		yield return new InstallModule(Runspace, Log)
		{
			SafekitPath = SafekitPath,
			ModuleName = ModuleName,
			ConfigurationScriptsPath = ConfigurationScriptsPath
		};
		yield return new SetLocation(Runspace, Log)
		{
			Path = ConfigurationScriptsPath
		};
		yield return new ConfigureAndStartModule(Runspace, Log)
		{
			SafekitPath = SafekitPath,
			ModuleName = ModuleName,
			SecondaryNodeName = SecondaryNodeName,
			SqlServerPathData = SqlServerPathData,
			SqlServerPathLog = SqlServerPathLog,
			VirtualIp = VirtualIp
		};
		yield return new SetLocation(Runspace, Log)
		{
			Path = ConfigurationScriptsPath
		};
		yield return new DisableSafeKitCheckersAndErrorDetection(Runspace, Log)
		{
			SafekitPath = SafekitPath,
			ModuleName = ModuleName
		};
		yield return new UpdateDataCollectorAppConfig(PrimaryNodeAddress, VirtualIp, DataCollectorConfigFilePath, Log);
		yield return new SetLocation(Runspace, Log)
		{
			Path = ConfigurationScriptsPath
		};
		yield return new RegisterServices(Log)
		{
			ServerConfiguratorPath = ServerConfiguratorPath,
			IdpAddress = IdpAddress
		};
		yield return new SetLocation(Runspace, Log)
		{
			Path = ConfigurationScriptsPath
		};
		yield return new EnableSafeKitCheckersAndErrorDetection(Runspace, Log)
		{
			SafekitPath = SafekitPath,
			ModuleName = ModuleName
		};
		yield return new SetLocation(Runspace, Log)
		{
			Path = ConfigurationScriptsPath
		};
		yield return new SetClusterMembershipFlag(Runspace, Log)
		{
			ServerConfigPath = ServerConfigPath,
			IsClusterMember = true,
			ClusterLocalWebserverUriString = WebConsoleHelper.GetWebConsoleAddress(PrimaryNodeAddress, EnableHttps),
			ClusterModuleName = (FailoverConfigurationHelper.IsExternalSqlSetup() ? "videoosnosql" : "videoos"),
			SafekitVirtualAddress = VirtualIp
		};
		yield return new ChangeFailoverLicense(Log, LicenseFilePath, SafekitPath);
		yield return new CreateFailoverDesktopShortcut(EnableHttps, PrimaryNodeAddress, Path.Combine(ConfigurationScriptsPath, DesktopShortcutIconName));
	}
}
